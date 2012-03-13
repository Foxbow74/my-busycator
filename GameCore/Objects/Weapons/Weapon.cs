using GameCore.Creatures;

namespace GameCore.Objects.Weapons
{
	public abstract class Weapon : Item
	{
		protected Weapon(Material _material) : base(_material)
		{
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.WEAPON; }
		}

		public override void Resolve(Creature _creature)
		{
		}

		public override EMaterial AllowedMaterials
		{
			get 
			{
				return EMaterial.METAL|EMaterial.WOOD|EMaterial.MINERAL;
			}
		}
	}
}