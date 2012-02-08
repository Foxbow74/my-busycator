namespace GameCore.Objects.Weapons
{
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
}