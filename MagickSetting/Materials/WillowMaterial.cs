using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	internal class WillowMaterial : WoodMaterial
	{
		public WillowMaterial(): base("������") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.WildWillow; } }
		public override int TreeTileIndex
		{
			get { return 4; }
		}

		public override Noun TreeName
		{
			get { return "���".AsNoun(ESex.FEMALE, false); }
		}
	}
}