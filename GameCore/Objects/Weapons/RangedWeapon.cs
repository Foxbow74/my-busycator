namespace GameCore.Objects.Weapons
{
	public abstract class RangedWeapon : Weapon
	{
		protected RangedWeapon(Material _material) : base(_material)
		{
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.MISSILE_WEAPON; }
		}
	}
}