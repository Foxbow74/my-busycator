using GameCore;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	internal class BronzeMaterial : MetalMaterial
	{
		public BronzeMaterial()
			: base("���������") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.Bronze; } }
	}
}