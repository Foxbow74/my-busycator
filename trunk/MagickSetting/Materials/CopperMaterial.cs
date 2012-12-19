using GameCore;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	internal class CopperMaterial : MetalMaterial
	{
		public CopperMaterial() : base("медный") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.CoolCopper; } }
	}
}