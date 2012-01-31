using GameCore.Creatures;

namespace GameCore.Objects.Jewelry
{
	public class Ring : Jevelry
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
			get { return "������"; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
}