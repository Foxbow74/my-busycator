﻿using GameCore.AbstractLanguage;
using GameCore.Mapping.Layers;

namespace GameCore.Essences.Things
{
	internal class StairDown : Stair
	{
		public StairDown(WorldLayer _leadToLayer, Material _material)
			: base(EALNouns.StairDown, _leadToLayer, _material) { }

		public StairDown(Material _material) : base(_material) { }

        public override int TileIndex { get { return 10; } }
	}
}