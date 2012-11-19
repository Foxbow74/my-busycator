namespace GameCore.Essences.Weapons
{
	public abstract class RangedWeapon : Weapon
	{
		protected RangedWeapon(Material _material) : base(_material) { }

        public override ETileset Tileset
        {
            get
            {
                return ETileset.RANGED_WEAPONS;
            }
        }

		public override EItemCategory Category { get { return EItemCategory.MISSILE_WEAPON; } }
	}
}