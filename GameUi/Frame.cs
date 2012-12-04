using GameCore;

namespace GameUi
{
	public class Frame
	{
		public static Frame Frame1;
		public static Frame Frame2;
		public static Frame Frame3;

		static Frame()
		{
			Frame1 = new Frame(ETileset.FRAME1);
			Frame2 = new Frame(ETileset.FRAME2);
			Frame3 = new Frame(ETileset.FRAME3);
		}

		public Frame(ETileset _tileset)
		{
			TopLeft = _tileset.GetTile(0);
			Top = _tileset.GetTile(1);
			TopRight = _tileset.GetTile(2);
			Right = _tileset.GetTile(3);
			BottmoRight = _tileset.GetTile(4);
			Bottom = _tileset.GetTile(5);
			BottomLeft = _tileset.GetTile(6);
			Left = _tileset.GetTile(7);
		}

		public ATile Left { get; private set; }
		public ATile Top { get; private set; }
		public ATile Right { get; private set; }
		public ATile Bottom { get; private set; }

		public ATile TopLeft { get; private set; }
		public ATile TopRight { get; private set; }
		public ATile BottmoRight { get; private set; }
		public ATile BottomLeft { get; private set; }
	}
}