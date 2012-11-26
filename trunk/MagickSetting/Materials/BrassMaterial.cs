using GameCore;
using GameCore.Materials;
using RusLanguage;

namespace MagicSetting.Materials
{
	internal class BrassMaterial : MetalMaterial
	{
		public BrassMaterial()
			: base("������") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.Brass; } }
	}
}