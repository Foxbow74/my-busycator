using GameCore.Creatures;
using RusLanguage;

namespace GameCore.Objects.Furniture
{
	internal class Bed : FurnitureThing
	{
		public Bed(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

		public override ETileset Tileset { get { return ETileset.BED; } }

		public override string Name { get { return "кровать"; } }

		public override void Resolve(Creature _creature) { }
	}
}