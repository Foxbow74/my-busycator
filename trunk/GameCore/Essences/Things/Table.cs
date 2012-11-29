using GameCore.Creatures;

namespace GameCore.Essences.Things
{
	internal class Table : Thing
	{
		public Table(Material _material) : base(_material) { }

        public override int TileIndex { get { return 12; } }

		public override string Name { get { return "стол"; } }
	}
}