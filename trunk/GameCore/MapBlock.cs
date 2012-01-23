using System;
using System.Collections.Generic;
using GameCore.Creatures;
using Graphics;
using Object = GameCore.Objects.Object;

namespace GameCore
{
	public class MapBlock
	{
		//Координаты блока в блочных координатах
		public Point Point { get; private set; }

		public const int SIZE = 20;

		public int RandomSeed { get; private set; }

		public ETerrains[,] Map { get; private set; }

		private List<Tuple<Object, Point>> m_objects;
		private List<Creature> m_creatures;

		public MapBlock(Point _point)
		{
			Point = _point;
			Map = new ETerrains[SIZE, SIZE];
			RandomSeed = World.Rnd.Next();
		}

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

		public bool ObjectsExists
		{
			get { return m_objects != null && m_objects.Count > 0; }
		}

		public bool CreaturesExists
		{
			get { return m_creatures != null && m_creatures.Count > 0; }
		}


		public List<Creature> Creatures
		{
			get
			{
				return m_creatures ?? (m_creatures = new List<Creature>());
			}
		}

		public List<Tuple<Object, Point>> Objects
		{
			get { return m_objects ?? (m_objects = new List<Tuple<Object, Point>>()); }
		}
	}
}