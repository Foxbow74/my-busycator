using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	internal class OakMaterial : WoodMaterial
	{
		public OakMaterial() : base("�������") { }

		public override FColor LerpColor { get { return FColor.DarkOak; } }

		public override int TreeTileIndex
		{
			get { return 0; }
		}

		public override Noun TreeName
		{
			get { return "���".AsNoun(ESex.MALE, false); }
		}
	}
}