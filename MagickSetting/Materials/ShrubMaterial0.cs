using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	class ShrubMaterial0 : ShrubMaterial
	{
		public ShrubMaterial0() : base("куст0".AsNoun(ESex.MALE, false)) { }

		public override int ShroobTileIndex
		{
			get { return 0; }
		}
	}
}