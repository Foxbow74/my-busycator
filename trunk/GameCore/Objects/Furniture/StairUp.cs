using GameCore.Creatures;
using GameCore.Mapping.Layers;

namespace GameCore.Objects.Furniture
{
	internal class StairUp : Stair
	{
		public StairUp(WorldLayer _leadToLayer)
		{
			LeadToLayer = _leadToLayer;
		}

		public StairUp()
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.STAIR_UP; }
		}

		public override string Name
		{
			get { return "лестница вверх"; }
		}

		public override void Resolve(Creature _creature)
		{
			LeadToLayer = World.TheWorld.GenerateNewLayer(_creature, this);
		}
	}
}