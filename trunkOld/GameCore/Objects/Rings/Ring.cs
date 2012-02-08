using GameCore.Creatures;

namespace GameCore.Objects.Rings
{
	class Ring : Item
	{
		public override ETiles Tile
		{
			get { return ETiles.RING; }
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.RINGS; }
		}

		public override string Name
		{
			get { return "кольцо"; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
}