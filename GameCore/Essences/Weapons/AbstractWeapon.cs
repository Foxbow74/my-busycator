using GameCore.Creatures;

namespace GameCore.Essences.Weapons
{
	public abstract class AbstractWeapon : Item, IWeapon
	{
		protected AbstractWeapon(Material _material) : base(_material) { }
        public override ETileset Tileset { get { return ETileset.WEAPONS; } }
		public override EItemCategory Category { get { return EItemCategory.WEAPON; } }
		public override EMaterialType AllowedMaterialsType { get { return EMaterialType.METAL | EMaterialType.WOOD | EMaterialType.MINERAL; } }
		public override void Resolve(Creature _creature) { }
	}
}