using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	class MushrumMaterial0 : MushrumMaterial
	{
		public MushrumMaterial0() : base("гриб0".AsNoun(ESex.MALE, false)) { }

		public override int MushrumTileIndex
		{
			get { return 0; }
		}
	}
}