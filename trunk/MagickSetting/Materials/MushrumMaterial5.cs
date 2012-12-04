using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	class MushrumMaterial5 : MushrumMaterial
	{
		public MushrumMaterial5() : base("гриб5".AsNoun(ESex.MALE, false)) { }

		public override int MushrumTileIndex
		{
			get { return 5; }
		}
	}
}