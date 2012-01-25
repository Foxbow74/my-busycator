using Microsoft.Xna.Framework.Graphics;

namespace RGL1
{
	public enum EFrameTiles
	{
		FRAME_L,
		FRAME_R,
		FRAME_T,
		FRAME_B,
		FRAME_TL,
		FRAME_TR,
		FRAME_BL,
		FRAME_BR,
		SOLID,
		SIMPLE,
	}

	public class Frame
	{
		public static Frame SimpleFrame = new Frame(EFrameTiles.SIMPLE);
		public static Frame GoldFrame = new Frame(EFrameTiles.FRAME_L, EFrameTiles.FRAME_TL, EFrameTiles.FRAME_T, EFrameTiles.FRAME_TR, EFrameTiles.FRAME_R, EFrameTiles.FRAME_BR, EFrameTiles.FRAME_B, EFrameTiles.FRAME_BL);

		public EFrameTiles Left { get; private set; }
		public EFrameTiles Top { get; private set; }
		public EFrameTiles Right { get; private set; }
		public EFrameTiles Bottom { get; private set; }

		public EFrameTiles TopLeft { get; private set; }
		public EFrameTiles TopRight { get; private set; }
		public EFrameTiles BottmoRight { get; private set; }
		public EFrameTiles BottomLeft { get; private set; }

		public Frame(EFrameTiles _tile)
		{
			Left = Top = Right = Bottom = TopLeft = TopRight = BottomLeft = BottmoRight = _tile;
		}

		public Frame(EFrameTiles _left, EFrameTiles _topLeft, EFrameTiles _top, EFrameTiles _topRight, EFrameTiles _right, EFrameTiles _bottomRight, EFrameTiles _bottom, EFrameTiles _bottomLeft)
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

		public void Draw(SpriteBatch _spriteBatch, int _col, int _row, int _width, int _height)
		{
			TopLeft.DrawAtCell(_spriteBatch, _col, _row);
			TopRight.DrawAtCell(_spriteBatch, _col + _width - 1, _row);
			BottomLeft.DrawAtCell(_spriteBatch, _col, _row + _height - 1);
			BottmoRight.DrawAtCell(_spriteBatch, _col + _width - 1, _row + _height - 1);

			for (int i = 1; i < _width - 1; i++)
			{
				Top.DrawAtCell(_spriteBatch, _col + i, _row);
				Bottom.DrawAtCell(_spriteBatch, _col + i, _row + _height - 1);
			}
			for (int j = 1; j < _height - 1; j++)
			{
				Left.DrawAtCell(_spriteBatch, _col, _row + j);
				Right.DrawAtCell(_spriteBatch, _col + _width - 1, _row + j);
			}
		}

	}
}