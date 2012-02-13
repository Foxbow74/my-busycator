using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore.Creatures;
using GameCore.Objects;
using GameCore.Objects.Furniture;
using Point = GameCore.Misc.Point;

namespace GameCore.Mapping
{
	public class MapCell
	{
		/// <summary>
		/// 	Координаты ячейки в блоке
		/// </summary>
		private readonly Point m_inBlockCoords;

		private readonly UInt32 m_seenMask;

		private TerrainAttribute m_terrainAttribute;

		internal MapCell(MapBlock _block, Point _inBlockCoords, Point _worldCoords)
		{
			m_inBlockCoords = _inBlockCoords;

			m_seenMask = ((UInt32) 1) << m_inBlockCoords.X;
			IsSeenBefore = (_block.SeenCells[m_inBlockCoords.Y] & m_seenMask) != 0;

			Block = _block;
			WorldCoords = _worldCoords;
			Terrain = _block.Map[_inBlockCoords.X, _inBlockCoords.Y];
		}

		public Point WorldCoords { get; private set; }

		public Point BlockCoords
		{
			get { return Block.Point; }
		}

		public ETerrains Terrain { get; private set; }

		public Thing Furniture
		{
			get
			{
				if (!Block.IsObjectsExists) return null;
				var tuple = Block.Objects.FirstOrDefault(_tuple => _tuple.Item2 == m_inBlockCoords && _tuple.Item1.IsFurniture());
				if (tuple == null)
				{
					return null;
				}
				return tuple.Item1;
			}
		}

		public IEnumerable<Item> Items
		{
			get
			{
				if (!Block.IsObjectsExists) yield break;
				var items = Block.Objects.Where(_tuple => _tuple.Item2 == m_inBlockCoords).Select(_tuple => _tuple.Item1).OfType<Item>().ToArray();
				foreach (var item in items)
				{
					yield return item;
				}
			}
		}

		public ETiles Tile
		{
			get
			{
				var cr = Creature;
				if(cr!=null) return cr.Tile;
				var cnt = 0;
				if (Block.IsObjectsExists)
				{
					foreach (var tuple in Block.Objects)
					{
						if (tuple.Item2 != m_inBlockCoords) continue;
						cnt++;
						if (cnt > 1)
						{
							return ETiles.HEAP_OF_ITEMS;
						}
						return tuple.Item1.Tile;
					}
				}
				var fr = Furniture;
				if(fr!=null) return fr.Tile;
				return ETiles.NONE;
			}
		}

		public Creature Creature
		{
			get
			{
				if (!Block.CreaturesExists) return null;
				return Block.Creatures.FirstOrDefault(_creature => _creature.Coords.Equals(WorldCoords));
			}
		}

		public int BlockRandomSeed
		{
			get { return Block.RandomSeed; }
		}

		public TerrainAttribute TerrainAttribute
		{
			get { return m_terrainAttribute ?? (m_terrainAttribute = TerrainAttribute.GetAttribute(Terrain)); }
		}

		private MapBlock Block { get; set; }
		public bool IsSeenBefore { get; private set; }

		public bool IsVisibleNow { get; set; }

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
				if(Terrain==ETerrains.WINDOW)
				{
					return new FColor(1f,1f,0,0);
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
				if (opacity < 1)
				{
					opacity += Items.Sum(_item => _item.Opacity);
				}
				var trancparence = 1f - opacity;

				return new FColor(trancparence, 1f,1f,1f);
			}
		}

		public bool IsCanShootThrough
		{
			get { return TerrainAttribute.IsCanShootThrough; }
		}

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

		public Item ResolveFakeItem(Creature _creature, FakedItem _fakeItem)
		{
			var item = (Item) _fakeItem.ResolveFake(_creature);
			if(!Block.Objects.Remove(new Tuple<Thing, Point>(_fakeItem, m_inBlockCoords)))
			{
				throw new NotImplementedException("Нет тут такого!");
			}
			Block.AddObject(m_inBlockCoords, item);
			return item;
		}

		public Furniture ResolveFakeFurniture(Creature _creature, FakedThing _fakeFurniture)
		{
			var furniture = (Furniture) _fakeFurniture.ResolveFake(_creature);
			Block.Objects.Remove(new Tuple<Thing, Point>(_fakeFurniture, m_inBlockCoords));
			Block.AddObject(m_inBlockCoords, furniture);
			return furniture;
		}

		public void RemoveFurnitureFromBlock()
		{
			if (Furniture == null) throw new ArgumentNullException();
			Block.Objects.Remove(new Tuple<Thing, Point>(Furniture, m_inBlockCoords));
		}

		public void RemoveItemFromBlock(Item _item)
		{
			Block.Objects.Remove(new Tuple<Thing, Point>(_item, m_inBlockCoords));
		}

		public void AddObjectToBlock(Thing _thing)
		{
			Block.AddObject(m_inBlockCoords, _thing);
		}

		public IEnumerable<ThingDescriptor> GetAllAvailableItemDescriptors(Creature _creature)
		{
			foreach (var item in Items)
			{
				yield return new ThingDescriptor(item, WorldCoords, null);
			}
			var furniture = Furniture as Container;
			if (furniture != null && !furniture.IsClosed(this, _creature))
			{
				var inside = furniture.GetItems(_creature).Items.Select(_item => new ThingDescriptor(_item, WorldCoords, furniture));
				foreach (var thingDescriptor in inside)
				{
					yield return thingDescriptor;
				}
			}
		}

		public void SetIsSeenBefore()
		{
			Block.SeenCells[m_inBlockCoords.Y] |= m_seenMask;
		}

		public FColor Lighted { get; set; }
		//public float Visibility { get; set; }
	}
}