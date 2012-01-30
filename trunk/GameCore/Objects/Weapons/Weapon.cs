namespace GameCore.Objects.Weapons
{
	public abstract class Weapon : Item
	{
		public override void Resolve(Creatures.Creature _creature)
		{
			
		}

		public override EItemCategory Category
		{
			get { return EItemCategory.WEAPON; }
		}
	}
}