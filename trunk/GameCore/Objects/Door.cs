using Graphics;

namespace GameCore.Objects
{
	public class Door:Object
	{
		public override Tile Tile
		{
			get { return Tiles.DoorTile; }
		}

		public override string Name
		{
			get { return "дверь"; }
		}
	}
}
