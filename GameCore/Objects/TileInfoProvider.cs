namespace GameCore.Objects
{
	public class TileInfoProvider : ITileInfoProvider
	{
		static TileInfoProvider() { HeapOfItems = new TileInfoProvider(ETiles.HEAP_OF_ITEMS); }

		public TileInfoProvider(ETiles _tile) : this(_tile, FColor.Empty, EDirections.DOWN) { }

		public TileInfoProvider(ETiles _tile, FColor _colorMultiplier, EDirections _dir)
		{
			Tile = _tile;
			LerpColor = _colorMultiplier;
			Direction = _dir;
		}

		public static ITileInfoProvider HeapOfItems { get; private set; }

		#region ITileInfoProvider Members

		public EDirections Direction { get; private set; }

		public ETiles Tile { get; private set; }

		public FColor LerpColor { get; private set; }

		#endregion
	}
}