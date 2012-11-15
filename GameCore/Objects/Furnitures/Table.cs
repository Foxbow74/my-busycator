using GameCore.Creatures;

namespace GameCore.Objects.Furnitures
{
	internal class Table : Furniture
	{
		public Table(Material _material) : base(_material) { }

        public override int TileIndex { get { return 12; } }

		public override string Name { get { return "стол"; } }

		public override void Resolve(Creature _creature) { }
	}
}