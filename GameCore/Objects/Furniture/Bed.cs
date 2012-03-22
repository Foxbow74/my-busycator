using GameCore.Creatures;
using RusLanguage;

namespace GameCore.Objects.Furniture
{
	class Bed : FurnitureThing
	{
		public Bed(Material _material)
			: base(_material)
		{
			Sex = ESex.FEMALE;
		}

		public override ETiles Tile
		{
			get { return ETiles.BED; }
		}

		public override string Name
		{
			get { return "кровать"; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
}