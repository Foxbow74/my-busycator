using GameCore.Materials;

namespace MagicSetting.Materials
{
	class MushrumMaterial1 : MushrumMaterial
	{
		public MushrumMaterial1() : base("гриб1") { }

		public override int MushrumTileIndex
		{
			get { return 1; }
		}
	}
}