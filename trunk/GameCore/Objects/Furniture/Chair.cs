using GameCore.Creatures;

namespace GameCore.Objects.Furniture
{
	class Chair : FurnitureThing
	{
		public Chair(Material _material)
			: base(_material)
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.CHAIR; }
		}

		public override string Name
		{
			get { return "стул"; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
}