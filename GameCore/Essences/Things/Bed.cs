using GameCore.Creatures;

namespace GameCore.Essences.Things
{
	internal class Bed : Thing
	{
		public Bed(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 0; } }

		public override string Name { get { return "кровать"; } }

		public override void Resolve(Creature _creature) { }
	}
}