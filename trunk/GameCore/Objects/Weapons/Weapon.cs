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

		public override System.Collections.Generic.IEnumerable<EMaterial> AllowedMaterials
		{
			get 
			{
				yield return EMaterial.METAL;
				yield return EMaterial.WOOD;
				yield return EMaterial.MINERAL;
			}
		}
	}
}