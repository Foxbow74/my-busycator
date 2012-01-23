using Graphics;

namespace GameCore.Objects
{
	public abstract class Container:Item
	{
	}

	public class Chest:Container
	{
		public override Tile Tile
		{
			get { return Tiles.ChestTile; }
		}

		public override string Name
		{
			get { return "сундук"; }
		}
	}
}
