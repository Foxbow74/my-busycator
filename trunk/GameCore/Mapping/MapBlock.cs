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

		public const int SIZE = 32;
		public readonly static Rct Rect = new Rct(0,0,SIZE,SIZE);

		public MapBlock(Point _blockId)
		{
			ConnectionPoints = new Dictionary<Point, Room>();
			Rooms = new List<Room>();
			Creatures = new List<Tuple<Creature, Point>>();
			Objects = new List<Tuple<Thing, Point>>();

			WorldCoords = _blockId*SIZE;
			BlockId = _blockId;
			Map = new ETerrains[SIZE,SIZE];
			RandomSeed = World.Rnd.Next();
			SeenCells = new uint[SIZE];
		}

		public Point WorldCoords { get; private set; }

		public Point BlockId { get; private set; }

		public int RandomSeed { get; private set; }

		public ETerrains[,] Map { get; private set; }

		protected Dictionary<Point, Room> ConnectionPoints { get; private set; }

		public List<Tuple<Creature, Point>> Creatures { get; private set; }

		public List<Tuple<Thing, Point>> Objects { get; private set; }

		public List<Room> Rooms { get; private set; }

		public UInt32[] SeenCells { get; private set; }


		public void RemoveObject(Thing _item, Point _inBlockCoords)
		{
			Objects.Remove(new Tuple<Thing, Point>(_item, _inBlockCoords));
		}

		public bool AddObject(Thing _thing, Point _inBlockCoords)
		{
			if (_thing is StackOfItems)
			{
				var stack = Objects.Where(_tuple => _tuple.Item2 == _inBlockCoords && _tuple.Item1.Equals(_thing))
					.Select(_tuple => _tuple.Item1).OfType<StackOfItems>()
					.FirstOrDefault();
				if (stack != null)
				{
					stack.Add((StackOfItems) _thing);
					return false;
				}
			}

			Objects.Add(new Tuple<Thing, Point>(_thing, _inBlockCoords));
			return true;
		}

		private static Point GetWorldCoord(Point _blockId)
		{
			return _blockId * SIZE;
		}

		public static Point GetBlockId(Point _point)
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

		public IEnumerable<Tuple<ILightSource, Point>> LightSources
		{
			get
			{
				foreach (var tuple in Objects)
				{
					if (tuple.Item1 is ILightSource)
					{
						yield return new Tuple<ILightSource, Point>((ILightSource)tuple.Item1, tuple.Item2);
					}
					else if (tuple.Item1.Light!=null)
					{
						yield return new Tuple<ILightSource, Point>(tuple.Item1.Light, tuple.Item2);
					}
				}
				foreach (var tuple in Creatures)
				{
					if (tuple.Item1.Light != null) yield return new Tuple<ILightSource, Point>(tuple.Item1.Light, tuple.Item2);
				}
			}
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

		public Rct Rct()
		{
			return new Rct(BlockId*SIZE, SIZE, SIZE);
		}

		public void AddRoom(Room _room)
		{
			Rooms.Add(_room);
			_room.AddedToBlock(this);
		}

		public Point ToWorldCoords(Point _point)
		{
			return WorldCoords + _point;
		}
	}
}