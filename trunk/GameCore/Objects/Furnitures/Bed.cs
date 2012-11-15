using GameCore.Creatures;
using RusLanguage;

namespace GameCore.Objects.Furnitures
{
	internal class Bed : Furniture
	{
		public Bed(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 0; } }

		public override string Name { get { return "кровать"; } }

		public override void Resolve(Creature _creature) { }
	}
}