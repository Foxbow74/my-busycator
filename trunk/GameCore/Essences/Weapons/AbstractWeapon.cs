using GameCore.Creatures;

namespace GameCore.Essences.Weapons
{
	public abstract class AbstractWeapon : Item, IWeapon
	{
		protected AbstractWeapon(Material _material) : base(_material) { }
        public override ETileset Tileset { get { return ETileset.WEAPONS; } }
		public override EItemCategory Category { get { return EItemCategory.WEAPON; } }
		public override EMaterial AllowedMaterials { get { return EMaterial.METAL | EMaterial.WOOD | EMaterial.MINERAL; } }
		public override void Resolve(Creature _creature) { }
	}
}