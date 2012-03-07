﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture;

namespace GameCore.Mapping
{
	public class LiveMapCell
	{
		private readonly Point m_liveCoords;
		private uint m_seenMask;
		private Point m_inBlockCoords;
		private MapBlock m_mapBlock;

		private readonly List<Item> m_items = new List<Item>();

		public LiveMapCell(LiveMapBlock _liveMapBlock, Point _liveCoords)
		{
			LiveMapBlock = _liveMapBlock;
			m_liveCoords = _liveCoords;
		}

		public FColor Visibility { get; set; }

		public FColor Lighted { get; set; }

		public bool IsSeenBefore { get; private set; }

		public float Rnd { get; private set; }

		public Point OnLiveMapCoords { get; private set; }

		public void SetMapCell(MapBlock _mapBlock, Point _inBlockCoords, Point _worldCoords, float _rnd, Point _onLiveMapCoords, LiveMap _liveMap)
		{
			Rnd = _rnd;
			m_items.Clear();
			Furniture = null;

			WorldCoords = _worldCoords;
			m_inBlockCoords = _inBlockCoords;
			m_mapBlock = _mapBlock;
			m_seenMask = ((UInt32)1) << m_inBlockCoords.X;

			Terrain = _mapBlock.Map[_inBlockCoords.X, _inBlockCoords.Y];
			TerrainAttribute = TerrainAttribute.GetAttribute(Terrain);

			IsSeenBefore = (_mapBlock.SeenCells[_inBlockCoords.Y] & m_seenMask) != 0;
			OnLiveMapCoords = _onLiveMapCoords;

			ClearTemp();
		}

		public void SetIsSeenBefore()
		{
			if (!IsSeenBefore)
			{
				IsSeenBefore = true;
				m_mapBlock.SeenCells[m_inBlockCoords.Y] |= m_seenMask;
			}
		}

		public void ClearTemp()
		{
			Visibility = FColor.Empty;
			Lighted = FColor.Black;
		}

		public int BlockRandomSeed
		{
			get { return LiveMapBlock.MapBlock.RandomSeed; }
		}

		public override string ToString()
		{
			return m_liveCoords +  " WC:" + (WorldCoords == null ? "<null>" : WorldCoords.ToString()); ;
		}

		internal void AddItemIntenal(Item _item)
		{
			m_items.Add(_item);
		}

		public void AddItem(Item _item)
		{
			m_mapBlock.AddObject(_item, m_inBlockCoords);
			m_items.Add(_item);
		}

		public IEnumerable<Item> Items{get { return m_items; }}

		public FurnitureThing Furniture { get; set; }
		
		public Creature Creature
		{
			get
			{
				var creature = LiveMapBlock.Creatures.FirstOrDefault(_creature => _creature.LiveCoords==LiveCoords);
				return creature;
			}
		}

		public ETerrains Terrain { get; private set; }

		public TerrainAttribute TerrainAttribute { get; private set; }

		public bool IsCanShootThrough
		{
			get { return TerrainAttribute.IsCanShootThrough; }
		}

		public IEnumerable<ITileInfoProvider> TileInfoProviders
		{
			get
			{
				if(Furniture!=null)
				{
					yield return Furniture;
				}
				foreach (var item in Items)
				{
					yield return item;
				}
				var cr = Creature;
				if (cr != null)
				{
					yield return cr;
				}
			}
		}


		public IEnumerable<ITileInfoProvider> FoggedTileInfoProviders
		{
			get
			{
				if (ThingHelper.Is<Stair>(Furniture))
				{
					yield return Furniture;
				}
			}
		}

		public Point LiveCoords
		{
			get { return m_liveCoords; }
		}

		public Item ResolveFakeItem(Creature _creature, FakedItem _fakeItem)
		{
			RemoveItem(_fakeItem);
			var item = (Item)_fakeItem.ResolveFake(_creature);
			AddItem(item);
			return item;
		}

		public Thing ResolveFakeFurniture(Creature _creature)
		{
			var fakedThing = (FakedFurniture)Furniture;
			m_mapBlock.RemoveObject(fakedThing, m_inBlockCoords);
			Furniture = (FurnitureThing)fakedThing.ResolveFake(_creature);
			m_mapBlock.AddObject(Furniture, m_inBlockCoords);
			return Furniture;
		}

		public IEnumerable<ThingDescriptor> GetAllAvailableItemDescriptors<TThing>(Creature _creature) where TThing:Thing
		{
			foreach (var item in Items.OfType<TThing>())
			{
				yield return new ThingDescriptor(item, LiveCoords, null);
			}
			if (typeof(TThing).IsAssignableFrom(typeof(Item)))
			{
				var furniture = Furniture as Container;
				if (furniture != null && !furniture.IsClosed(this, _creature))
				{
					foreach (var item in furniture.GetItems(_creature).Items.OfType<TThing>())
					{
						yield return new ThingDescriptor(item, LiveCoords, furniture);
					}
				}
			}
		}

		public float Opacity
		{
			get
			{
				var attr = TerrainAttribute;
				var opacity = attr.Opacity;
				if (opacity < 1 && Furniture != null)
				{
					opacity += Furniture.Opacity;
				}
				if (opacity < 1 && Creature != null)
				{
					opacity += Creature.Opacity;
				}
				if (LiveCoords == World.TheWorld.Avatar.LiveCoords)
				{
					opacity += World.TheWorld.Avatar.Opacity;
				}
				if (opacity < 1)
				{
					opacity += Items.Sum(_item => _item.Opacity);
				}
				return opacity;
			}
		}

		public FColor TransparentColor
		{
			get
			{
				//if (Terrain == ETerrains.RED_BRICK_WINDOW)
				//{
				//    return new FColor(1f, 1f, 0, 0);
				//}
				var attr = TerrainAttribute;
				var opacity = attr.Opacity;
				if (opacity < 1 && Furniture != null)
				{
					opacity += Furniture.Opacity;
				}
				if (opacity < 1 && Creature != null)
				{
					opacity += Creature.Opacity;
				}
				if (opacity < 1 && LiveCoords == World.TheWorld.Avatar.LiveCoords)
				{
					opacity += World.TheWorld.Avatar.Opacity;
				}
				if (opacity < 1)
				{
					opacity += Items.Sum(_item => _item.Opacity);
				}
				var trancparence = 1f - Math.Min(1f, opacity);

				return new FColor(trancparence, 1f, 1f, 1f);
			}
		}

		public LiveMapBlock LiveMapBlock { get; private set; }

		public Point WorldCoords { get; private set; }

		public float GetIsPassableBy(Creature _creature)
		{
			if (Creature != null) return 0f;
			if (Furniture != null)
			{
				if (Furniture.Is<Door>() && Furniture.IsClosed(this, _creature))
				{
					return 0f;
				}
			}
			if (_creature is Missile)
			{
				return TerrainAttribute.IsCanShootThrough ? 1f : 0f;
			}
			return TerrainAttribute.IsPassable;
		}

		public void RemoveItem(Item _item)
		{
			if(!m_items.Remove(_item))
			{
				throw new ApplicationException();
			}
			m_mapBlock.RemoveObject(_item, m_inBlockCoords);
		}
	}
}