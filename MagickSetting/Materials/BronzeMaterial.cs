using GameCore;
using GameCore.Materials;
using RusLanguage;

namespace MagicSetting.Materials
{
	internal class BronzeMaterial : MetalMaterial
	{
		public BronzeMaterial()
			: base("������") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.Bronze; } }
	}
}