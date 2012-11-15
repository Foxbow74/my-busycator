using GameCore.Mapping.Layers;

namespace GameCore.Objects.Furnitures
{
	internal class StairUp : Stair
	{
		public StairUp(WorldLayer _leadToLayer, Material _material)
			: base(_leadToLayer, _material) { }

		public StairUp(Material _material) : base(_material) { }

        public override int TileIndex { get { return 11; } }

		public override string Name { get { return "лестница вверх"; } }
	}
}