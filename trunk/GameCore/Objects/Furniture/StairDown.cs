using GameCore.Mapping.Layers;

namespace GameCore.Objects.Furniture
{
	internal class StairDown : Stair
	{
		public StairDown(WorldLayer _leadToLayer, Material _material)
			: base(_leadToLayer, _material)
		{
		}

		public StairDown(Material _material):base(_material)
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
	}
}