using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	class ShrubMaterial2 : ShrubMaterial
	{
		public ShrubMaterial2() : base("куст2".AsNoun(ESex.MALE, false)) { }
		public override int ShroobTileIndex
		{
			get { return 2; }
		}
	}
}