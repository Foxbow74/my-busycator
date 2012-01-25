#region

using System;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;

#endregion

namespace GameCore.Map
{
	public class MapCell
	{
		/// <summary>
		/// 	Координаты ячейки в блоке
		/// </summary>
		private readonly Point m_localPoint;

		private TerrainAttribute m_terrainAttribute;

		internal MapCell(MapBlock _block, Point _inBlockCoords, Point _worldCoords)
		{
			m_localPoint = _inBlockCoords;

			Block = _block;
			WorldCoords = _worldCoords;
			Terrain = _block.Map[_inBlockCoords.X, _inBlockCoords.Y];
			Thing = null;

			if (_block.ObjectsExists)
			{
				var tuple = _block.Objects.FirstOrDefault(_tuple => _tuple.Item2 == _inBlockCoords);
				if (tuple != null)
				{
					Thing = tuple.Item1;
				}
			}

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

		public Thing Thing { get; private set; }

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
				//if (Object != null) return 0f;
				return TerrainAttribute.IsPassable;
			}
		}

		public TerrainAttribute TerrainAttribute
		{
			get
			{
				if (m_terrainAttribute == null)
				{
					m_terrainAttribute = TerrainAttribute.GetAttribute(Terrain);
				}
				return m_terrainAttribute;
			}
		}

		private MapBlock Block { get; set; }

		public Thing ResolveFakeItem()
		{
			var o = ((FakeItem) Thing).Resolve();
			Block.Objects.Remove(new Tuple<Thing, Point>(Thing, m_localPoint));
			Block.Objects.Add(new Tuple<Thing, Point>(o, m_localPoint));
			Thing = o;
			return o;
		}

		public void RemoveObjectFromBlock()
		{
			if (Thing == null) throw new ArgumentNullException();
			Block.Objects.Remove(new Tuple<Thing, Point>(Thing, m_localPoint));
			Thing = null;
		}
	}
}