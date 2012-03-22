using RusLanguage;

namespace GameCore.Materials
{
	class FlashMaterial : Material
	{
		public FlashMaterial()
			: base("плоть")
		{
			Sex = ESex.FEMALE;
		}

		public override FColor LerpColor
		{
			get { return FColor.WhiteSmoke; }
		}

		public override EMaterial MaterialType
		{
			get { return EMaterial.FLASH; }
		}
	}
}