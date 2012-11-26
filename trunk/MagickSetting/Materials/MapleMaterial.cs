using GameCore;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class MapleMaterial : WoodMaterial
	{
		public MapleMaterial()
			: base("клен") { }

		public override FColor LerpColor { get { return FColor.Maple; } }
		public override int TreeTileIndex
		{
			get { return 2; }
		}
	}
}