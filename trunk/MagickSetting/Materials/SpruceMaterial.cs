using GameCore;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class SpruceMaterial : WoodMaterial
	{
		public SpruceMaterial()
			: base("ель") { Sex = ESex.IT; }

		public override FColor LerpColor { get { return FColor.LimedSpruce; } }
		public override int TreeTileIndex
		{
			get { return 6; }
		}
	}
}