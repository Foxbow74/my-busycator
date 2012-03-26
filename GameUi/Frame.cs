using GameCore;

namespace GameUi
{
	public class Frame
	{
		public static Frame SimpleFrame = new Frame(ETiles.SIMPLE);

		public static Frame GoldFrame = new Frame(ETiles.FRAME_L,
		                                          ETiles.FRAME_TL,
		                                          ETiles.FRAME_T,
		                                          ETiles.FRAME_TR,
		                                          ETiles.FRAME_R,
		                                          ETiles.FRAME_BR,
		                                          ETiles.FRAME_B,
		                                          ETiles.FRAME_BL);

		public Frame(ETiles _tile) { Left = Top = Right = Bottom = TopLeft = TopRight = BottomLeft = BottmoRight = _tile.GetTile(); }

		public Frame(ETiles _left,
		             ETiles _topLeft,
		             ETiles _top,
		             ETiles _topRight,
		             ETiles _right,
		             ETiles _bottomRight,
		             ETiles _bottom,
		             ETiles _bottomLeft)
		{
			Left = _left.GetTile();
			Top = _top.GetTile();
			Right = _right.GetTile();
			Bottom = _bottom.GetTile();
			TopLeft = _topLeft.GetTile();
			TopRight = _topRight.GetTile();
			BottomLeft = _bottomLeft.GetTile();
			BottmoRight = _bottomRight.GetTile();
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