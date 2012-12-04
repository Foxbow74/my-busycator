using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	class ShrubMaterial1 : ShrubMaterial
	{
		public ShrubMaterial1() : base("куст1".AsNoun(ESex.MALE, false)) { }

		public override int ShroobTileIndex
		{
			get { return 1; }
		}
	}
}