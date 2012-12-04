using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	internal class AshMaterial : WoodMaterial
	{
		public AshMaterial()
			: base("��������") { }

		public override FColor LerpColor { get { return FColor.Ash; } }
		public override int TreeTileIndex
		{
			get { return 1; }
		}

		public override Noun TreeName
		{
			get { return "�����".AsNoun(ESex.MALE, false);}
		}
	}
}