#region

using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;

#endregion

namespace GameCore.Map
{
	public class MapBlock
	{
		//Координаты блока в блочных координатах

		public const int SIZE = 20;

		private List<Creature> m_creatures;
		private List<Tuple<Thing, Point>> m_objects;

		public MapBlock(Point _point)
		{
			Point = _point;
			Map = new ETerrains[SIZE,SIZE];
			RandomSeed = World.Rnd.Next();
		}

		public Point Point { get; private set; }
		public int RandomSeed { get; private set; }

		public ETerrains[,] Map { get; private set; }

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
			get { return m_creatures ?? (m_creatures = new List<Creature>()); }
		}

		public List<Tuple<Thing, Point>> Objects
		{
			get { return m_objects ?? (m_objects = new List<Tuple<Thing, Point>>()); }
		}

		public static Point GetBlockCoords(Point _point)
		{
			var blockCoords = new Point(GetBlockCoord(_point.X), GetBlockCoord(_point.Y));
			return blockCoords;
		}

		private static int GetBlockCoord(int _i)
		{
			if (_i < 0)
			{
				return -(Math.Abs(_i + 1)/SIZE + 1);
			}
			return _i/SIZE;
		}

		public static Point GetInBlockCoords(Point _point)
		{
			return new Point((SIZE + (_point.X%SIZE))%SIZE, (SIZE + (_point.Y%SIZE))%SIZE);
		}
	}
}