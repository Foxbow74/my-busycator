namespace GameCore.Objects.Weapons
{
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