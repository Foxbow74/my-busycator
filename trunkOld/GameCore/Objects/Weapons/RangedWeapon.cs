namespace GameCore.Objects.Weapons
{
	public abstract class RangedWeapon : Weapon
	{
		public override EThingCategory Category
		{
			get { return EThingCategory.MISSILE_WEAPON; }
		}
	}
}