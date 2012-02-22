using System.Diagnostics;
using System.Drawing;

namespace GameCore.Mapping
{
	public class Room
	{
		public Room(Rectangle _roomRect, Rectangle _areaRect)
		{
			Debug.WriteLine("Room created " + _roomRect + " inside " + _areaRect);
			RoomRectangle = _roomRect;
			AreaRectangle = _areaRect;
		}

		public Rectangle RoomRectangle { get; private set; }
		public Rectangle AreaRectangle { get; private set; }
	}

	
}
