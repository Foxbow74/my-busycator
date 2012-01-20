using Microsoft.Xna.Framework;

namespace Graphics
{
	public class Frame
	{
		public static Frame SimpleFrame = new Frame(Tiles.FrameTile);
		public static Frame GoldFrame = new Frame(Tiles.Frame_L, Tiles.Frame_TL, Tiles.Frame_T, Tiles.Frame_TR, Tiles.Frame_R, Tiles.Frame_BR, Tiles.Frame_B, Tiles.Frame_BL);

		public Tile Left { get; private set; }
		public Tile Top { get; private set; }
		public Tile Right { get; private set; }
		public Tile Bottom { get; private set; }

		public Tile TopLeft { get; private set; }
		public Tile TopRight { get; private set; }
		public Tile BottmoRight { get; private set; }
		public Tile BottomLeft { get; private set; }

		public Frame(Tile _tile)
		{
			Left = Top = Right = Bottom = TopLeft = TopRight = BottomLeft = BottmoRight = _tile;
		}

		public Frame(Tile _left, Tile _topLeft, Tile _top, Tile _topRight, Tile _right, Tile _bottomRight, Tile _bottom, Tile _bottomLeft)
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
	}
}