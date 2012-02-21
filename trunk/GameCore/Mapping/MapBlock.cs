using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Mapping
{
	public class MapBlock
	{
		//Координаты блока в блочных координатах

		public const int SIZE = 20;

		private readonly List<Tuple<Thing, Point>> m_objects = new List<Tuple<Thing, Point>>();
		private readonly List<Tuple<Creature, Point>> m_creatures = new List<Tuple<Creature, Point>>();

		public MapBlock(Point _point)
		{
			LightSources = new List<Tuple<Point, LightSource>>();
			BlockId = _point;
			Map = new ETerrains[SIZE,SIZE];
			RandomSeed = World.Rnd.Next();
			SeenCells = new uint[SIZE];
		}

		public Point BlockId { get; private set; }

		public int RandomSeed { get; private set; }

		public ETerrains[,] Map { get; private set; }

		public List<Tuple<Creature, Point>> Creatures
		{
			get { return m_creatures; }
		}

		public List<Tuple<Thing, Point>> Objects
		{
			get { return m_objects; }
		}

		public UInt32[] SeenCells { get; private set; }

		public void AddObject(Thing _thing, Point _inBlockCoords)
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

		public List<Tuple<Point, LightSource>> LightSources { get; set; }

		public void AddLightSource(Point _point, LightSource _lightSource)
		{
			LightSources.Add(new Tuple<Point, LightSource>(_point, _lightSource));
		}

		public void AddCreature(Creature _creature, Point _inBlockCoords)
		{
			m_creatures.Add(new Tuple<Creature, Point>(_creature, _inBlockCoords));
		}

		public void RemoveCreature(Creature _creature)
		{
			if(m_creatures.RemoveAll(_tuple => _tuple.Item1 == _creature)==0)
			{
				throw new ApplicationException();
			}
		}
	}
}