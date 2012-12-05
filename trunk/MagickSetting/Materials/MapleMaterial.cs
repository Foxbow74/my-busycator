using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
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

		public override EALNouns TreeName
		{
			get { return EALNouns.Maple; }
		}
	}
}