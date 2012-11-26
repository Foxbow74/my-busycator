namespace GameCore.Essences.Weapons
{
	public abstract class AbstractRangedWeapon : AbstractWeapon
	{
		protected AbstractRangedWeapon(Material _material) : base(_material) { }

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