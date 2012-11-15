using GameCore.Creatures;

namespace GameCore.Objects.Furniture
{
	internal class Table : FurnitureThing
	{
		public Table(Material _material) : base(_material) { }

		public override ETileset Tileset { get { return ETileset.TABLE; } }

		public override string Name { get { return "стол"; } }

		public override void Resolve(Creature _creature) { }
	}
}