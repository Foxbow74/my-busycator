using GameCore.PathFinding;

namespace GameCore.Objects
{
	public class TileInfoProvider:ITileInfoProvider
	{
		public static ITileInfoProvider HeapOfItems { get; private set; }

		static TileInfoProvider()
		{
			HeapOfItems = new TileInfoProvider(ETiles.HEAP_OF_ITEMS);
		}
		
		public TileInfoProvider(ETiles _tile):this(_tile, FColor.Empty,EDirections.DOWN)
		{}

		public TileInfoProvider(ETiles _tile, FColor _colorMultiplier, EDirections _dir)
		{
			Tile = _tile;
			LerpColor = _colorMultiplier;
			Direction = _dir;
		}

		public EDirections Direction { get; private set; }

		public ETiles Tile { get; private set; }

		public FColor LerpColor { get; private set; }
	}
}