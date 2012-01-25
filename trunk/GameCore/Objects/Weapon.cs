namespace GameCore.Objects
{
	public abstract class Weapon : Item
	{
	}

	public class Axe : Weapon
	{
		public override ETiles Tile
		{
			get { return ETiles.AXE; }
		}

		public override string Name
		{
			get { return "топор"; }
		}
	}

	public class Sword : Weapon
	{
		public override ETiles Tile
		{
			get { return ETiles.SWORD; }
		}

		public override string Name
		{
			get { return "меч"; }
		}
	}
}