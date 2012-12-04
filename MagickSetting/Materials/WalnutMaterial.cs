using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	internal class WalnutMaterial : WoodMaterial
	{
		public WalnutMaterial() : base("��������") { }

		public override FColor LerpColor { get { return FColor.Walnut; } }
		public override int TreeTileIndex
		{
			get { return 3; }
		}

		public override Noun TreeName
		{
			get { return "����".AsNoun(ESex.MALE, false); }
		}
	}
}