namespace GameCore.Objects.Weapons
{
	public abstract class Weapon : Item
	{
		public override void Resolve(Creatures.Creature _creature)
		{
			
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.WEAPON; }
		}
	}
}