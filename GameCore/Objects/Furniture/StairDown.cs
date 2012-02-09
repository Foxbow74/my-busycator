﻿using GameCore.Creatures;
using GameCore.Mapping.Layers;

namespace GameCore.Objects.Furniture
{
	internal class StairDown : Stair
	{
		public StairDown(WorldLayer _leadToLayer)
		{
			LeadToLayer = _leadToLayer;
		}

		public StairDown()
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.STAIR_DOWN; }
		}

		public override string Name
		{
			get { return "лестница вниз"; }
		}

		public override void Resolve(Creature _creature)
		{
			LeadToLayer = World.TheWorld.GenerateNewLayer(_creature, this);
		}
	}
}