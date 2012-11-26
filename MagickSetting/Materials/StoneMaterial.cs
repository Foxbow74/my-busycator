using GameCore;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class StoneMaterial : MineralMaterial
	{
		public StoneMaterial()
			: base("������") { }

		public override FColor LerpColor { get { return FColor.DarkGray; } }
	}
}