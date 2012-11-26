using GameCore.Materials;

namespace MagicSetting.Materials
{
	class MushrumMaterial3 : MushrumMaterial
	{
		public MushrumMaterial3() : base("гриб3") { }

		public override int MushrumTileIndex
		{
			get { return 3; }
		}
	}
}