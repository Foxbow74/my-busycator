using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	class MushrumMaterial1 : MushrumMaterial
	{
		public MushrumMaterial1() : base("����1".AsNoun(ESex.MALE, false)) { }

		public override int MushrumTileIndex
		{
			get { return 1; }
		}
	}
}