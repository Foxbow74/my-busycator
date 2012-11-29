using System;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Essences.Things.LightSources
{
	internal class IndoorLight : LightSourceThing, ISpecial
	{
		public IndoorLight(LightSource _lightSource, Material _material)
			: base(_lightSource, _material) { }

        public override int TileIndex
        {
            get
            {
                return 0;
            }
        }

		public override string Name { get { return "светильник"; } }
	}
}