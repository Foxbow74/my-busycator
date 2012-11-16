using GameCore.Creatures;

namespace GameCore.Essences.Weapons
{
	public abstract class Weapon : Item
	{
		protected Weapon(Material _material) : base(_material) { }

		public override EEssenceCategory Category { get { return EEssenceCategory.WEAPON; } }

		public override EMaterial AllowedMaterials { get { return EMaterial.METAL | EMaterial.WOOD | EMaterial.MINERAL; } }
		public override void Resolve(Creature _creature) { }
	}
}