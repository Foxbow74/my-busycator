using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	internal class PineMaterial : WoodMaterial
	{
		public PineMaterial()
			: base("сосновый") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.PineTree; } }

		public override int TreeTileIndex
		{
			get { return 5; }
		}

		public override Noun TreeName
		{
			get { return "сосна".AsNoun(ESex.FEMALE, false); }
		}
	}
}