using GameCore.Mapping.Layers;

namespace GameCore.Objects.Furnitures
{
	internal class StairDown : Stair
	{
		public StairDown(WorldLayer _leadToLayer, Material _material)
			: base(_leadToLayer, _material) { }

		public StairDown(Material _material) : base(_material) { }

        public override int TileIndex { get { return 10; } }

		public override string Name { get { return "лестница вниз"; } }
	}
}