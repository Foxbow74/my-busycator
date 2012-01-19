using System;
using System.Collections.Generic;
using Object = GameCore.Objects.Object;

namespace GameCore
{
	class MapBlock
	{
		public const int SIZE = 20;

		public int RandomSeed { get; private set; }

		public MapBlock()
		{
			Map = new ETerrains[SIZE, SIZE];
			RandomSeed = World.Rnd.Next();
		}

		public ETerrains[,] Map { get; private set; }

		public static Point GetBlockCoords(Point _point)
		{
			var blockCoords = new Point(GetBlockCoord(_point.X), GetBlockCoord(_point.Y));
			return blockCoords;
		}

		private static int GetBlockCoord(int _i)
		{
			if(_i<0)
			{
				return -(Math.Abs(_i + 1)/ SIZE + 1);
			}
			return _i / SIZE;
		}

		public static Point GetInBlockCoords(Point _point)
		{
			return new Point((SIZE + (_point.X % SIZE)) % SIZE, (SIZE + (_point.Y % SIZE)) % SIZE);
		}

		private List<Tuple<Object, Point>> m_objects;

		public bool ObjectsExists
		{
			get { return m_objects != null && m_objects.Count > 0; }
		}

		public List<Tuple<Object, Point>> Objects
		{
			get { return m_objects ?? (m_objects = new List<Tuple<Object, Point>>()); }
		}
	}
}