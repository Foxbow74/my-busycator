using System;
using System.Linq;
using GameCore.Objects;
using Graphics;
using Object = GameCore.Objects.Object;

namespace GameCore
{
	public class MapCell
	{
		public Point WorldCoords { get; private set; }

		public ETerrains Terrain { get; private set; }

		public Object Object { get; private set; }

		public int BlockRandomSeed { get { return Block.RandomSeed; } }

		private MapBlock Block { get; set; }

		private Point m_localPoint;

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
		}

		public Object ResolveFakeItem(FakeItem _o)
		{
			var o = _o.Resolve();
			Block.Objects.Add(new Tuple<Object, Point>(o, m_localPoint));
			Block.Objects.Remove(new Tuple<Object, Point>(_o, m_localPoint));
			return o;

		}
	}
}