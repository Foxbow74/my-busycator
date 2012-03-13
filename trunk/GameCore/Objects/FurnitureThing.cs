namespace GameCore.Objects
{
	public abstract class FurnitureThing : Thing
	{
		private FColor m_lerpColor = FColor.Empty;

		protected FurnitureThing(Material _material) : base(_material)
		{
		}

		public override FColor LerpColor
		{
			get { return m_lerpColor; }
		}

		public void Paint(FColor _lerpColor)
		{
			m_lerpColor = _lerpColor;
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.FURNITURE; }
		}

		public override EMaterial AllowedMaterials
		{
			get
			{
				return EMaterial.WOOD|EMaterial.MINERAL|EMaterial.METAL;
			}
		}
	}
}