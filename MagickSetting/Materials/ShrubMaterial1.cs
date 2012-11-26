using GameCore.Materials;

namespace MagicSetting.Materials
{
	class ShrubMaterial1 : ShrubMaterial
	{
		public ShrubMaterial1() : base("куст1") { }

		public override int ShroobTileIndex
		{
			get { return 1; }
		}
	}
}