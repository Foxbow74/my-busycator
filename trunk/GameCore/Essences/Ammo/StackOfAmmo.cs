using GameCore.Creatures;
using GameCore.Essences.Weapons;

namespace GameCore.Essences.Ammo
{
	internal abstract class StackOfAmmo : StackOfItems, IWeapon
	{
		protected StackOfAmmo(Material _material) : base(_material) { }

        public override ETileset Tileset
        {
            get { return ETileset.MISSILES; }
        }

		public override EItemCategory Category { get { return EItemCategory.MISSILES; } }

		protected override int GetStartCount(Creature _creature) { return (int) (_creature.GetLuckRandom*25) + 1; }
	}
}