namespace GameCore.Objects.Weapons
{
	public class CrossBow : RangedWeapon
	{
		public override ETiles Tile
		{
			get { return ETiles.CROSSBOW; }
		}

		public override string Name
		{
			get { return "арбалет"; }
		}
	}
}