using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	class MushrumMaterial2 : MushrumMaterial
	{
		public MushrumMaterial2() : base("����2".AsNoun(ESex.MALE, false)) { }

		public override int MushrumTileIndex
		{
			get { return 2; }
		}
	}
}