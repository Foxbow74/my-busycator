using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Materials;

namespace MagickSetting.Materials
{
	internal class SpruceMaterial : WoodMaterial
	{
		public SpruceMaterial()
			: base("еловый") { Sex = ESex.IT; }

		public override FColor LerpColor { get { return FColor.LimedSpruce; } }
		public override int TreeTileIndex
		{
			get { return 6; }
		}

		public override EALNouns TreeName
		{
			get { return EALNouns.Spruce; }
		}
	}
}