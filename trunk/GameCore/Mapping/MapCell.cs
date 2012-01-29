using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture;

namespace GameCore.Mapping
{
	public class MapCell
	{
		private readonly UInt32 m_seenMask;

		/// <summary>
		/// Координаты ячейки в блоке
		/// </summary>
		private readonly Point m_inBlockCoords;

		private TerrainAttribute m_terrainAttribute;

		internal MapCell(MapBlock _block, Point _inBlockCoords, Point _worldCoords)
		{
			m_inBlockCoords = _inBlockCoords;

			m_seenMask = ((UInt32)1) << m_inBlockCoords.X;
			IsSeenBefore = (_block.SeenCells[m_inBlockCoords.Y] & m_seenMask) != 0;

			Block = _block;
			WorldCoords = _worldCoords;
			Terrain = _block.Map[_inBlockCoords.X, _inBlockCoords.Y];

			if (_block.CreaturesExists)
			{
				var creature = _block.Creatures.FirstOrDefault(_creature => _creature.Coords == _worldCoords);
				if (creature != null)
				{
					Creature = creature;
				}
			}
		}

		public Point WorldCoords { get; private set; }

		public Point BlockCoords
		{
			get { return Block.Point; }
		}

		public ETerrains Terrain { get; private set; }

		public Thing Thing
		{
			get
			{
				if (!Block.IsObjectsExists) return null;
				var tuples = Block.Objects.Where(_tuple => _tuple.Item2 == m_inBlockCoords).ToArray();
				if(!tuples.Any())
				{
					return null;
				}
				if(tuples.Length==1)
				{
					return tuples[0].Item1;
				}
				else
				{
					return new Heap(Block, m_inBlockCoords);
				}
			}
		}

		public Creature Creature { get; private set; }

		public int BlockRandomSeed
		{
			get { return Block.RandomSeed; }
		}

		public float IsPassable
		{
			get
			{
				if (Creature != null) return 0f;
				if(Thing!=null)
				{
					if(Thing is Door && ((Door) Thing).LockType != LockType.OPEN)
					{
						return 0f;
					}
				}
				return TerrainAttribute.IsPassable;
			}
		}

		public TerrainAttribute TerrainAttribute
		{
			get { return m_terrainAttribute ?? (m_terrainAttribute = TerrainAttribute.GetAttribute(Terrain)); }
		}

		private MapBlock Block { get; set; }

		public Thing ResolveFakeItem(Creature _creature)
		{
			var o = ((IFaked) Thing).ResolveFake(_creature);
			Block.Objects.Remove(new Tuple<Thing, Point>(Thing, m_inBlockCoords));
			Block.Objects.Add(new Tuple<Thing, Point>(o, m_inBlockCoords));
			return o;
		}

		public void RemoveObjectFromBlock()
		{
			if (Thing == null) throw new ArgumentNullException();
			Block.Objects.Remove(new Tuple<Thing, Point>(Thing, m_inBlockCoords));
		}

		public IEnumerable<ThingDescriptor> GetAllAvailableItems(Creature _creature)
		{
			if(Thing==null) yield break;
			if (Thing.IsItem(this, _creature))
			{
				if (Thing is IFaked)
				{
					ResolveFakeItem(_creature);
				}
				yield return new ThingDescriptor(Thing, WorldCoords, null);
			}
			if (Thing is Container && !Thing.IsClosed(this, _creature))
			{
				var container = (Container) Thing;
				foreach (var item in container.GetItems(_creature).Items)
				{
					yield return new ThingDescriptor(item, WorldCoords, container);
				}
			}
		}

		public bool IsSeenBefore
		{
			get; private set;
		}

		public bool IsVisibleNow { get; set; }

		public void SetIsSeenBefore()
		{
			Block.SeenCells[m_inBlockCoords.Y] |= m_seenMask;
		}
	}
}