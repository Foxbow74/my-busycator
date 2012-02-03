using System;
using System.Linq;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Mapping
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
			SeenCells = new uint[SIZE];
		}

		public Point Point { get; private set; }
		public int RandomSeed { get; private set; }

		public ETerrains[,] Map { get; private set; }

		public bool IsObjectsExists
		{
			get { return m_objects != null && m_objects.Count > 0; }
		}

		internal bool CreaturesExists
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

		public void AddObject(Point _inBlockCoords, Thing _thing)
		{
			if (_thing is StackOfItems)
			{
				var stack = Objects.Where(_tuple => _tuple.Item2 == _inBlockCoords && _tuple.Item1.Equals(_thing))
					.Select(_tuple => _tuple.Item1).OfType<StackOfItems>()
					.FirstOrDefault();
				if (stack != null)
				{
					stack.Add((StackOfItems) _thing);
					return;
				}
			}

			Objects.Add(new Tuple<Thing, Point>(_thing, _inBlockCoords));
		}

		public UInt32[] SeenCells { get; private set; }

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