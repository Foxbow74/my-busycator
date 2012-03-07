using GameCore.Creatures;

namespace GameCore.Objects.Potions
{
	public class Potion : Item
	{
		public Potion(Material _material) : base(_material)
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.POTION; }
		}

		public override string Name
		{
			get { return "булька"; }
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.POTION; }
		}

		public override void Resolve(Creature _creature)
		{
		}

		public void Drinked(Creature _creature)
		{
			//throw new NotImplementedException();
		}

		public bool IsAllowToDrink(Creature _creature)
		{
			return true;
		}
	}
}