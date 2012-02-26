using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;
using Point = GameCore.Misc.Point;

namespace GameCore.Mapping
{
	public class MapBlock
	{
		public delegate void UpdatedDelegate();

		public event UpdatedDelegate Updated;

		public void CastUpdated()
		{
			if(Updated!=null)
			{
				Updated();
			}
		}

		public enum EMapBlockState
		{
			CREATED,
			PREREADY,
			COMPLETE,
		}

		//Координаты блока в блочных координатах

		public const int SIZE = 20;
		public readonly static Rectangle Rect = new Rectangle(0,0,SIZE,SIZE);

		public MapBlock(Point _point)
		{
			ConnectionPoints = new Dictionary<Point, Room>();
			Rooms = new List<Room>();
			Creatures = new List<Tuple<Creature, Point>>();
			Objects = new List<Tuple<Thing, Point>>();
			LightSources = new List<Tuple<Point, LightSource>>();

			BlockId = _point;
			Map = new ETerrains[SIZE,SIZE];
			RandomSeed = World.Rnd.Next();
			SeenCells = new uint[SIZE];
			State = EMapBlockState.CREATED;
		}

		public EMapBlockState State { get; internal set; }

		public Point BlockId { get; private set; }

		public int RandomSeed { get; private set; }

		public ETerrains[,] Map { get; private set; }

		protected Dictionary<Point, Room> ConnectionPoints { get; private set; }

		public List<Tuple<Creature, Point>> Creatures { get; private set; }

		public List<Tuple<Thing, Point>> Objects { get; private set; }

		public List<Room> Rooms { get; private set; }

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
			Creatures.Add(new Tuple<Creature, Point>(_creature, _inBlockCoords));
		}

		public void RemoveCreature(Creature _creature)
		{
			if(Creatures.RemoveAll(_tuple => _tuple.Item1 == _creature)==0)
			{
				throw new ApplicationException();
			}
		}

		public Rectangle Rectangle()
		{
			return new Rectangle(new System.Drawing.Point(BlockId.X * SIZE, BlockId.Y * SIZE), new Size(SIZE, SIZE));
		}
	}
}