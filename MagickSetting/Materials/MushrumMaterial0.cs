using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	class MushrumMaterial0 : MushrumMaterial
	{
		public MushrumMaterial0() : base(EALNouns.Mushrum0) { }

		public override int MushrumTileIndex
		{
			get { return 0; }
		}
	}
}