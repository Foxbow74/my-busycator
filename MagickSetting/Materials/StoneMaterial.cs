using GameCore;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class StoneMaterial : MineralMaterial
	{
		public StoneMaterial()
			: base("камень") { }

		public override FColor LerpColor { get { return FColor.DarkGray; } }
	}
}