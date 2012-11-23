using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore.Mapping.Layers
{
	internal class ConnectionPoint
	{
		public ConnectionPoint(Point _begin, Point _end, Room _room, EDirections _dir)
		{
			Begin = _begin;
			End = _end;
			Room = _room;
			Dir = _dir;
		}

		internal Point Begin { get; private set; }
		internal Point End { get; private set; }
		internal Room Room { get; private set; }
		internal EDirections Dir { get; private set; }
		internal Point BlockId { get { return BaseMapBlock.GetBlockId(End); } }

		public override string ToString() { return End + " from " + Begin + " rm:" + Room.RoomRectangle; }
	}

	internal class Connector
	{
		public Connector(params Room[] _rooms) { Rooms = new List<Room>(_rooms); }
		public List<Room> Rooms { get; private set; }
	}
}