using GameCore.Creatures;
using RusLanguage;

namespace GameCore.Objects.Potions
{
	public class Potion : Item
	{
		public Potion(Material _material) : base(_material) { Sex = ESex.FEMALE; }

		public override ETiles Tileset { get { return ETiles.POTION; } }

		public override string Name { get { return "булька"; } }

		public override EThingCategory Category { get { return EThingCategory.POTION; } }

		public override EMaterial AllowedMaterials { get { return EMaterial.MINERAL; } }

		public override void Resolve(Creature _creature) { }

		public void Drinked(Creature _creature)
		{
			//throw new NotImplementedException();
		}

		public bool IsAllowToDrink(Creature _creature) { return true; }
	}
}