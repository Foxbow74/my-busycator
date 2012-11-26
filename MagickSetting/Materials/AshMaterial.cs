using GameCore;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class AshMaterial : WoodMaterial
	{
		public AshMaterial()
			: base("ясень") { }

		public override FColor LerpColor { get { return FColor.Ash; } }
		public override int TreeTileIndex
		{
			get { return 1; }
		}
	}
}