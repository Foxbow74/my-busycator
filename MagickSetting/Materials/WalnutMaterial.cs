using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	internal class WalnutMaterial : WoodMaterial
	{
		public WalnutMaterial() : base("ореховый") { }

		public override FColor LerpColor { get { return FColor.Walnut; } }
		public override int TreeTileIndex
		{
			get { return 3; }
		}

		public override Noun TreeName
		{
			get { return "орех".AsNoun(ESex.MALE, false); }
		}
	}
}