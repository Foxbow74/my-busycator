namespace Graphics
{
	public class Frame
	{
		public static Frame SimpleFrame = new Frame(Tiles.FrameTile);

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
	}
}