using GameCore;

namespace GameUi
{
	public class Frame
	{
		public static Frame SimpleFrame = new Frame(ETiles.FRAME1);

		public static Frame GoldFrame = new Frame(ETiles.FRAME2);

		public Frame(ETiles _tile)
		{
			TopLeft = _tile.GetTile(0);
			Top = _tile.GetTile(1);
			TopRight = _tile.GetTile(2);
			Right = _tile.GetTile(3);
			BottmoRight = _tile.GetTile(4);
			Bottom = _tile.GetTile(5);
			BottomLeft = _tile.GetTile(6);
			Left = _tile.GetTile(7);
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