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

		public override EEssenceCategory Category { get { return EEssenceCategory.MISSILE_WEAPON; } }
	}
}