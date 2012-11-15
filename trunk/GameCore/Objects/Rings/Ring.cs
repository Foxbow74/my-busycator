using GameCore.Creatures;

namespace GameCore.Objects.Rings
{
	internal class Ring : Item
	{
		public Ring(Material _material) : base(_material) { }

		public override ETileset Tileset { get { return ETileset.RING; } }

		public override EThingCategory Category { get { return EThingCategory.RINGS; } }

		public override string Name { get { return "кольцо"; } }

		public override void Resolve(Creature _creature) { }
	}
}