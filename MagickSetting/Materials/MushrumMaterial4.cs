using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	class MushrumMaterial4 : MushrumMaterial
	{
		public MushrumMaterial4() : base("гриб4".AsNoun(ESex.MALE, false)) { }

		public override int MushrumTileIndex
		{
			get { return 4; }
		}
	}
}