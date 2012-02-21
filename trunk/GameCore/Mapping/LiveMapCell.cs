using System;
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

		public void SetMapCell(MapBlock _mapBlock, Point _inBlockCoords, Point _worldCoords)
		{
			m_items.Clear();
			Furniture = null;

			WorldCoords = _worldCoords;
			m_inBlockCoords = _inBlockCoords;
			m_mapBlock = _mapBlock;
			m_seenMask = ((UInt32)1) << m_inBlockCoords.X;

			Terrain = _mapBlock.Map[_inBlockCoords.X, _inBlockCoords.Y];
			TerrainAttribute = TerrainAttribute.GetAttribute(Terrain);

			IsSeenBefore = (_mapBlock.SeenCells[_inBlockCoords.Y] & m_seenMask) != 0;
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
			Visibility = Lighted = FColor.Empty;
		}

		public int BlockRandomSeed
		{
			get { return LiveMapBlock.MapBlock.RandomSeed; }
		}

		public override string ToString()
		{
			return m_liveCoords +  " WC:" + (WorldCoords == null ? "<null>" : WorldCoords.ToString()); ;
		}

		public void AddItem(Item _item)
		{
			m_items.Add(_item);
		}

		public IEnumerable<Item> Items{get { return m_items; }}

		public Thing Furniture { get; set; }
		
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

		//public Point WorldCoords { get; private set; }

		public ETiles Tile
		{
			get
			{
				var cr = Creature;
				if (cr != null)
				{
					return cr.Tile;
				}
				var cnt = 0;
				foreach (var item in Items)
				{
					cnt++;
					return cnt > 1 ? ETiles.HEAP_OF_ITEMS : item.Tile;
				}
				return Furniture != null ? Furniture.Tile : ETiles.NONE;
			}
		}

		public Point LiveCoords
		{
			get { return m_liveCoords; }
		}


		public Item ResolveFakeItem(Creature _creature, FakedItem _fakeItem)
		{
			var item = (Item)_fakeItem.ResolveFake(_creature);
			if (!m_items.Remove(_fakeItem))
			{
				throw new NotImplementedException("Нет тут такого!");
			}
			m_items.Add(item);
			return item;
		}

		public Furniture ResolveFakeFurniture(Creature _creature)
		{
			Furniture = ((FakedThing)Furniture).ResolveFake(_creature);
			return (Furniture)Furniture;
		}

		public IEnumerable<ThingDescriptor> GetAllAvailableItemDescriptors(Creature _creature)
		{
			foreach (var item in Items)
			{
				yield return new ThingDescriptor(item, LiveCoords, null);
			}
			var furniture = Furniture as Container;
			if (furniture != null && !furniture.IsClosed(this, _creature))
			{
				foreach (var item in furniture.GetItems(_creature).Items)
				{
					yield return new ThingDescriptor(item, LiveCoords, furniture);
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
				if (Terrain == ETerrains.WINDOW)
				{
					return new FColor(1f, 1f, 0, 0);
				}
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
				if (Furniture.IsDoor(this, _creature) && Furniture.IsClosed(this, _creature))
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
		}
	}
}