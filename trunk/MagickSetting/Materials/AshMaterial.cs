using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	internal class AshMaterial : WoodMaterial
	{
		public AshMaterial()
			: base("ясеневый") { }

		public override FColor LerpColor { get { return FColor.Ash; } }
		public override int TreeTileIndex
		{
			get { return 1; }
		}

		public override Noun TreeName
		{
			get { return "ясень".AsNoun(ESex.MALE, false);}
		}
	}
}