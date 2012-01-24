using System;
using System.Linq;
using GameCore.Creatures;
using GameCore.Objects;
using Graphics;
using Object = GameCore.Objects.Object;

namespace GameCore
{
	public class MapCell
	{
		public Point WorldCoords { get; private set; }

		public Point BlockCoords { get { return Block.Point; } }

		public ETerrains Terrain { get; private set; }

		public Object Object { get; private set; }

		public Creature Creature { get; private set; }

		public int BlockRandomSeed { get { return Block.RandomSeed; } }

		public float IsPassable
		{
			get
			{
				if (Creature != null) return 0f;
				if (Object != null) return 0f;
				return TerrainAttribute.IsPassable;
			}
		}

		private TerrainAttribute m_terrainAttribute = null;
		public TerrainAttribute TerrainAttribute
		{
			get
			{
				if(m_terrainAttribute==null)
				{
					m_terrainAttribute = TerrainAttribute.GetAttribute(Terrain);
				}
				return m_terrainAttribute;
			}
		}

		private MapBlock Block { get; set; }

		/// <summary>
		/// Координаты ячейки в блоке
		/// </summary>
		private readonly Point m_localPoint;

		internal MapCell(MapBlock _block, int _x, int _y, Point _worldCoords)
		{
			m_localPoint = new Point(_x, _y);

			Block = _block;
			WorldCoords = _worldCoords;
			Terrain = _block.Map[_x, _y];
			Object = null;

			if (_block.ObjectsExists)
			{
				var tuple = _block.Objects.FirstOrDefault(_tuple => _tuple.Item2.X == _x && _tuple.Item2.Y == _y);
				if (tuple != null)
				{
					Object = tuple.Item1;
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

		public Object ResolveFakeItem(FakeItem _o)
		{
			var o = _o.Resolve();
			Block.Objects.Add(new Tuple<Object, Point>(o, m_localPoint));
			Block.Objects.Remove(new Tuple<Object, Point>(_o, m_localPoint));
			return o;
		}

		public void RemoveObjectFromBlock()
		{
			if(Object==null) throw new ArgumentNullException();
			Block.Objects.Remove(new Tuple<Object, Point>(Object, m_localPoint));
		}
	}
}