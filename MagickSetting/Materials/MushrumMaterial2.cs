using GameCore.Materials;

namespace MagicSetting.Materials
{
	class MushrumMaterial2 : MushrumMaterial
	{
		public MushrumMaterial2() : base("гриб2") { }

		public override int MushrumTileIndex
		{
			get { return 2; }
		}
	}
}