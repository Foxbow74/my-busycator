using GameCore;
using GameCore.Materials;

namespace MagicSetting.Materials
{
	internal class OakMaterial : WoodMaterial
	{
		public OakMaterial()
			: base("���") { }

		public override FColor LerpColor { get { return FColor.DarkOak; } }

		public override int TreeTileIndex
		{
			get { return 0; }
		}
	}
}