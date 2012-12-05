using GameCore.AbstractLanguage;

namespace GameCore.Essences.Weapons
{
	public abstract class AbstractWeapon : Item, IWeapon
	{
		protected AbstractWeapon(Noun _name, Material _material) : base(_name, _material) { }
        public override ETileset Tileset { get { return ETileset.WEAPONS; } }
		public override EItemCategory Category { get { return EItemCategory.WEAPON; } }
		public override EMaterialType AllowedMaterialsType { get { return EMaterialType.METAL | EMaterialType.WOOD | EMaterialType.MINERAL; } }
	}
}