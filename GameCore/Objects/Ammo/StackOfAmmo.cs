using GameCore.Creatures;

namespace GameCore.Objects.Ammo
{
	internal abstract class StackOfAmmo : StackOfItems
	{
		protected StackOfAmmo(Material _material) : base(_material) { }

		public override EThingCategory Category { get { return EThingCategory.MISSILES; } }

		protected override int GetStartCount(Creature _creature) { return (int) (_creature.GetLuckRandom*25) + 1; }
	}
}