using GameCore.Materials;

namespace MagicSetting.Materials
{
	class ShrubMaterial3 : ShrubMaterial
	{
		public ShrubMaterial3() : base("куст3") { }
		public override int ShroobTileIndex
		{
			get { return 3; }
		}

	}
}