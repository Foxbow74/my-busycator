namespace GameCore.Objects
{
	public abstract class FurnitureThing : Thing
	{
		private FColor m_lerpColor;

		protected FurnitureThing(Material _material) : base(_material) { m_lerpColor = _material == null ? FColor.Empty : _material.LerpColor; }

		public override FColor LerpColor { get { return m_lerpColor; } }

		public override EThingCategory Category { get { return EThingCategory.FURNITURE; } }

		public override EMaterial AllowedMaterials { get { return EMaterial.WOOD | EMaterial.MINERAL | EMaterial.METAL; } }
		public void Paint(FColor _lerpColor) { m_lerpColor = _lerpColor; }
	}
}