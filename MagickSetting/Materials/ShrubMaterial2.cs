using GameCore.Materials;

namespace MagicSetting.Materials
{
	class ShrubMaterial2 : ShrubMaterial
	{
		public ShrubMaterial2() : base("куст2") { }
		public override int ShroobTileIndex
		{
			get { return 2; }
		}
	}
}