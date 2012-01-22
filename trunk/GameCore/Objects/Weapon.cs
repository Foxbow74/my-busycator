using Graphics;

namespace GameCore.Objects
{
	public abstract class Weapon : Item
	{
	}

	public class Axe : Weapon
	{
		public override Tile Tile { get { return Tiles.Axe; } }

		public override string Name
		{
			get { return "топор"; }
		}
	}

	public class Sword : Weapon
	{
		public override Tile Tile { get { return Tiles.SwordTile; } }

		public override string Name
		{
			get { return "меч"; }
		}
	}
}