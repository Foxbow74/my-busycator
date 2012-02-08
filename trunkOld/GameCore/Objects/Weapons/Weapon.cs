using GameCore.Creatures;

namespace GameCore.Objects.Weapons
{
	public abstract class Weapon : Item
	{
		public override EThingCategory Category
		{
			get { return EThingCategory.WEAPON; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
}