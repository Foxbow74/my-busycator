using GameCore.AbstractLanguage;
using GameCore.Misc;

namespace GameCore.Essences.Things.LightSources
{
	internal class IndoorLight : LightSourceThing, ISpecial
	{
		public IndoorLight(LightSource _lightSource, Material _material)
			: base("����������".AsNoun(ESex.MALE, false), _lightSource, _material) { }

        public override int TileIndex
        {
            get
            {
                return 0;
            }
        }
	}
}