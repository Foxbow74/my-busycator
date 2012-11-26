using GameCore;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class CopperMaterial : MetalMaterial
	{
		public CopperMaterial() : base("����") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.CoolCopper; } }
	}
}