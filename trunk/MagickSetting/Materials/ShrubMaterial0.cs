using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	class ShrubMaterial0 : ShrubMaterial
	{
		public ShrubMaterial0() : base(EALNouns.Shrub0) { }

		public override int ShroobTileIndex
		{
			get { return 0; }
		}
	}
}