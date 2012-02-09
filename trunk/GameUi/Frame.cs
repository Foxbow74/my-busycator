namespace GameUi
{
	public class Frame
	{
		public static Frame SimpleFrame = new Frame(EFrameTiles.SIMPLE);

		public static Frame GoldFrame = new Frame(EFrameTiles.FRAME_L, EFrameTiles.FRAME_TL, EFrameTiles.FRAME_T,
		                                          EFrameTiles.FRAME_TR, EFrameTiles.FRAME_R, EFrameTiles.FRAME_BR,
		                                          EFrameTiles.FRAME_B, EFrameTiles.FRAME_BL);

		public Frame(EFrameTiles _tile)
		{
			Left = Top = Right = Bottom = TopLeft = TopRight = BottomLeft = BottmoRight = _tile;
		}

		public Frame(EFrameTiles _left, EFrameTiles _topLeft, EFrameTiles _top, EFrameTiles _topRight, EFrameTiles _right,
		             EFrameTiles _bottomRight, EFrameTiles _bottom, EFrameTiles _bottomLeft)
		{
			Left = _left;
			Top = _top;
			Right = _right;
			Bottom = _bottom;
			TopLeft = _topLeft;
			TopRight = _topRight;
			BottomLeft = _bottomLeft;
			BottmoRight = _bottomRight;
		}

		public EFrameTiles Left { get; private set; }
		public EFrameTiles Top { get; private set; }
		public EFrameTiles Right { get; private set; }
		public EFrameTiles Bottom { get; private set; }

		public EFrameTiles TopLeft { get; private set; }
		public EFrameTiles TopRight { get; private set; }
		public EFrameTiles BottmoRight { get; private set; }
		public EFrameTiles BottomLeft { get; private set; }
	}
}