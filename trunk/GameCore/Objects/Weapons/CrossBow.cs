namespace GameCore.Objects.Weapons
{
	public class CrossBow: Weapon
	{
		public override ETiles Tile
		{
			get { return ETiles.CROSSBOW; }
		}

		public override EThingCategory Category
		{
			get
			{
				return EThingCategory.MISSILE_WEAPON;
			}
		}

		public override string Name
		{
			get { return "арбалет"; }
		}
	}
}