using GameCore.Creatures;

namespace GameCore.Essences.Ammo
{
	internal abstract class StackOfAmmo : StackOfItems
	{
		protected StackOfAmmo(Material _material) : base(_material) { }

		public override EEssenceCategory Category { get { return EEssenceCategory.MISSILES; } }

		protected override int GetStartCount(Creature _creature) { return (int) (_creature.GetLuckRandom*25) + 1; }
	}
}