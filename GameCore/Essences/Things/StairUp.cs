using GameCore.AbstractLanguage;
using GameCore.Mapping.Layers;

namespace GameCore.Essences.Things
{
	internal class StairUp : Stair
	{
		public StairUp(WorldLayer _leadToLayer, Material _material)
			: base(EALNouns.StairUp, _leadToLayer, _material)
		{
			
		}

		public StairUp(Material _material) : base(_material) { }

        public override int TileIndex { get { return 11; } }
	}
}