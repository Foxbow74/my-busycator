using GameCore;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class PineMaterial : WoodMaterial
	{
		public PineMaterial()
			: base("сосна") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.PineTree; } }
		public override int TreeTileIndex
		{
			get { return 5; }
		}
	}
}