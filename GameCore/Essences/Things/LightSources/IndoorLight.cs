using GameCore.AbstractLanguage;
using GameCore.Misc;

namespace GameCore.Essences.Things.LightSources
{
	public class IndoorLight : LightSourceThing, ISpecial
	{
		public IndoorLight(LightSource _lightSource, Material _material)
			: base(EALNouns.IndoorLight, _lightSource, _material) { }

        public override int TileIndex
        {
            get
            {
                return 0;
            }
        }
	}
}