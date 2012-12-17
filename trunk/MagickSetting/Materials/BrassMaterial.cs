using GameCore;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class BrassMaterial : MetalMaterial
	{
		public BrassMaterial()
			: base("латунный") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.Brass; } }
	}
}