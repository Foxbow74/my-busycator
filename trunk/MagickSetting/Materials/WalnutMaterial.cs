using GameCore;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class WalnutMaterial : WoodMaterial
	{
		public WalnutMaterial()
			: base("орех") { }

		public override FColor LerpColor { get { return FColor.Walnut; } }
		public override int TreeTileIndex
		{
			get { return 3; }
		}
	}
}