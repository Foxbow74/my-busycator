using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	class ShrubMaterial5 : ShrubMaterial
	{
		public ShrubMaterial5() : base("����5".AsNoun(ESex.MALE, false)) { }
		public override int ShroobTileIndex
		{
			get { return 5; }
		}

	}
}