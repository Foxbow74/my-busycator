using System;
using System.Collections.Generic;
using GameCore.Essences;
using GameCore.Misc;

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
			Objects = new List<Tuple<Essence, Point>>();
		}

		public List<Tuple<Essence, Point>> Objects { get; private set; }

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

		public void RemoveEssence(Essence _item, Point _inBlockCoords) { Objects.Remove(new Tuple<Essence, Point>(_item, _inBlockCoords)); }

		public bool AddEssence(Essence _essence, Point _inBlockCoords)
		{
			if (_essence is StackOfItems)
			{
				foreach (var tuple in Objects)
				{
					if (tuple.Item2 == _inBlockCoords && tuple.Item1.Equals(_essence))
					{
						var stack = tuple.Item1 as StackOfItems;
						if (stack != null)
						{
							stack.Add((StackOfItems)_essence);
							return false;
						}

					}
				}
			}
			if(_essence is IRemoteActivation)
			{
				var ra = _essence as IRemoteActivation;
				World.TheWorld.RegisterRemoteActivation(ra.MechanismId, ra, ToWorldCoords(_inBlockCoords));
			}

			Objects.Add(new Tuple<Essence, Point>(_essence, _inBlockCoords));
			return true;
		}
	}
}