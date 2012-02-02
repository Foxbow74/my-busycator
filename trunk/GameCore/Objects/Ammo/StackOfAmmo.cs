using GameCore.Creatures;

namespace GameCore.Objects.Ammo
{
	abstract class StackOfAmmo:Item
	{
		public override EThingCategory Category
		{
			get { return EThingCategory.MISSILES; }
		}

		public int Count { get; set; }

		public override void Resolve(Creature _creature)
		{
			Count = (int)(_creature.GetLuckRandom * 25);
		}
	}
}
