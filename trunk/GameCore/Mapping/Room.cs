using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace GameCore.Mapping
{
	public class Room
	{
		public Room(Rectangle _roomRect, Rectangle _areaRect)
		{
			Debug.WriteLine("Room created " + _roomRect + " inside " + _areaRect);
			RoomRectangle = _roomRect;
			AreaRectangle = _areaRect;
			ConnectedTo = new Dictionary<Room, Misc.Point>();
		}

		public Rectangle RoomRectangle { get; private set; }
		public Rectangle AreaRectangle { get; private set; }

		private bool m_isConnected = false;
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
				m_isConnected = value;
				foreach (var room in ConnectedTo.Keys)
				{
					room.IsConnected = true;
				}
			}
		}

		internal void Connect(Room _room, Misc.Point _pnt)
		{
			if (_room.IsConnected)
			{
				IsConnected = true;
			}
			if (ConnectedTo.ContainsKey(_room)) return;
			ConnectedTo.Add(_room, _pnt);
			_room.Connect(this, _pnt);

		}

		public Dictionary<Room, Misc.Point> ConnectedTo { get; private set; }

	}

	
}
