using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Mapping
{
	public class BaseMapBlock
	{
		public static readonly Rct Rect = new Rct(0, 0, Constants.MAP_BLOCK_SIZE, Constants.MAP_BLOCK_SIZE);

		public BaseMapBlock(Point _blockId)
		{
			Rooms = new List<Room>();
			WorldCoords = _blockId*Constants.MAP_BLOCK_SIZE;
			BlockId = _blockId;
			Map = new ETerrains[Constants.MAP_BLOCK_SIZE,Constants.MAP_BLOCK_SIZE];
			RandomSeed = World.Rnd.Next();
			Objects = new List<Tuple<Thing, Point>>();
		}

		public List<Tuple<Thing, Point>> Objects { get; private set; }

		public Point WorldCoords { get; private set; }

		public Point BlockId { get; private set; }

		public int RandomSeed { get; protected set; }

		public ETerrains[,] Map { get; private set; }

		public List<Room> Rooms { get; private set; }

		public static Point GetBlockId(Point _point)
		{
			var blockCoords = new Point(GetBlockCoord(_point.X), GetBlockCoord(_point.Y));
			return blockCoords;
		}

		private static int GetBlockCoord(int _i)
		{
			if (_i < 0)
			{
				return -(Math.Abs(_i + 1)/Constants.MAP_BLOCK_SIZE + 1);
			}
			return _i/Constants.MAP_BLOCK_SIZE;
		}

		public static Point GetInBlockCoords(Point _point) { return new Point((Constants.MAP_BLOCK_SIZE + (_point.X%Constants.MAP_BLOCK_SIZE))%Constants.MAP_BLOCK_SIZE, (Constants.MAP_BLOCK_SIZE + (_point.Y%Constants.MAP_BLOCK_SIZE))%Constants.MAP_BLOCK_SIZE); }

		public Rct Rct() { return new Rct(BlockId*Constants.MAP_BLOCK_SIZE, Constants.MAP_BLOCK_SIZE, Constants.MAP_BLOCK_SIZE); }

		public void AddRoom(Room _room)
		{
			Rooms.Add(_room);
			_room.AddedToBlock(this);
		}

		public Point ToWorldCoords(Point _point) { return WorldCoords + _point; }

		public void RemoveObject(Thing _item, Point _inBlockCoords) { Objects.Remove(new Tuple<Thing, Point>(_item, _inBlockCoords)); }

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
	}

	public class MapBlock : BaseMapBlock
	{
		public MapBlock(Point _blockId) : base(_blockId)
		{
			Creatures = new List<Tuple<Creature, Point>>();
			SeenCells = new uint[Constants.MAP_BLOCK_SIZE];
		}

		public MapBlock(Point _blockId, BaseMapBlock _baseMapBlock) : this(_blockId)
		{
			foreach (var point in Rect.AllPoints)
			{
				Map[point.X, point.Y] = _baseMapBlock.Map[point.X, point.Y];
			}
			foreach (var room in _baseMapBlock.Rooms)
			{
				Rooms.Add(room);
			}
			RandomSeed = _baseMapBlock.RandomSeed;
		}

		public List<Tuple<Creature, Point>> Creatures { get; private set; }

		public UInt32[] SeenCells { get; private set; }

		public IEnumerable<Tuple<ILightSource, Point>> LightSources
		{
			get
			{
				foreach (var tuple in Objects)
				{
					if (tuple.Item1 is ILightSource)
					{
						yield return new Tuple<ILightSource, Point>((ILightSource) tuple.Item1, tuple.Item2);
					}
					else if (tuple.Item1.Light != null)
					{
						yield return new Tuple<ILightSource, Point>(tuple.Item1.Light, tuple.Item2);
					}
				}
				foreach (var tuple in Creatures)
				{
					if (tuple.Item1.IsAvatar)
					{
					}
					if (tuple.Item1.Light != null) yield return new Tuple<ILightSource, Point>(tuple.Item1.Light, tuple.Item2);
				}
			}
		}

		public void AddCreature(Creature _creature, Point _inBlockCoords)
		{
			if (_creature is FakedCreature)
			{
				_creature = (Creature) ((FakedCreature) _creature).ResolveFake(World.TheWorld.Avatar);
			}
			Creatures.Add(new Tuple<Creature, Point>(_creature, _inBlockCoords));
		}

		public void RemoveCreature(Creature _creature)
		{
			if (Creatures.RemoveAll(_tuple => _tuple.Item1 == _creature) == 0)
			{
				throw new ApplicationException();
			}
		}

		public void AvatarLeftLayer()
		{
			foreach (var creature in Creatures)
			{
				creature.Item1.ClearActPool();
			}
		}
	}
}