using System;
using System.Collections.Generic;
using GameCore.Misc;
using Rectangle = System.Drawing.Rectangle;

namespace GameCore.Mapping
{
	public class Room
	{
		public Room(Rectangle _roomRect, Rectangle _areaRect, Point _blockId)
		{
			BlockId = _blockId;
			RoomRectangle = _roomRect;
			AreaRectangle = _areaRect;
			ConnectedTo = new Dictionary<Room, Point[]>();
		}

		public Point BlockId
		{
			get; private set; 
		}

		public Rectangle RoomRectangle { get; private set; }
		public Rectangle AreaRectangle { get; private set; }

		private bool m_isConnected;
		public bool IsConnected 
		{ 
			get
			{
				return m_isConnected;
			}
			set
			{
				if (m_isConnected == value)
				{
					return;
				}
				if (!value)
				{
					throw new NotImplementedException();
				}
				m_isConnected = true;
				foreach (var room in ConnectedTo.Keys)
				{
					room.IsConnected = true;
				}
			}
		}

		internal void Connect(Room _room, Point[] _pnt)
		{
			if (_room.IsConnected)
			{
				IsConnected = true;
			}
			if (ConnectedTo.ContainsKey(_room)) return;
			ConnectedTo.Add(_room, _pnt);
			_room.Connect(this, _pnt);
		}

		public Dictionary<Room, Point[]> ConnectedTo { get; private set; }

	}

	
}
