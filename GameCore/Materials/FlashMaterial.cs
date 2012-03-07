namespace GameCore.Materials
{
	class FlashMaterial : Material
	{
		public FlashMaterial()
			: base("плоть")
		{
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