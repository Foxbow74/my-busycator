using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	class ShrubMaterial4 : ShrubMaterial
	{
		public ShrubMaterial4() : base("куст4".AsNoun(ESex.MALE, false)) { }
		public override int ShroobTileIndex
		{
			get { return 4; }
		}

	}
}