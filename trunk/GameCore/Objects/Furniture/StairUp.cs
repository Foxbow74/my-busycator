using GameCore.Mapping.Layers;

namespace GameCore.Objects.Furniture
{
	internal class StairUp : Stair
	{
		public StairUp(WorldLayer _leadToLayer, Material _material)
			: base(_leadToLayer, _material)
		{
		}

		public StairUp(Material _material):base(_material)
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
	}
}