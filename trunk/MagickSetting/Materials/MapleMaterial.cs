using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class MapleMaterial : WoodMaterial
	{
		public MapleMaterial()
			: base("кленовый") { }

		public override FColor LerpColor { get { return FColor.Maple; } }
		public override int TreeTileIndex
		{
			get { return 2; }
		}

		public override Noun TreeName
		{
			get { return "клен".AsNoun(ESex.MALE, false); }
		}
	}
}