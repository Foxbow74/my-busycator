using GameCore;
using GameCore.Materials;
using RusLanguage;

namespace MagicSetting.Materials
{
	internal class WillowMaterial : WoodMaterial
	{
		public WillowMaterial()
			: base("ива") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.WildWillow; } }
		public override int TreeTileIndex
		{
			get { return 4; }
		}
	}
}