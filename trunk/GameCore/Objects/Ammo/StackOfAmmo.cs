using GameCore.Creatures;

namespace GameCore.Objects.Ammo
{
	abstract class StackOfAmmo : StackOfItems
	{
		public override EThingCategory Category
		{
			get { return EThingCategory.MISSILES; }
		}

		protected override int GetStartCount(Creature _creature)
		{
			return (int)(_creature.GetLuckRandom * 25) + 1;
		}
	}
}
