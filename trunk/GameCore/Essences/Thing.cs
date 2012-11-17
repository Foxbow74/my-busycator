namespace GameCore.Essences
{
	public abstract class Thing : Essence
	{
		private FColor m_lerpColor;

		protected Thing(Material _material) : base(_material) { m_lerpColor = _material == null ? FColor.Empty : _material.LerpColor; }

		public override FColor LerpColor { get { return m_lerpColor; } }

        public override ETileset Tileset{ get { return ETileset.THINGS; } }

		public override EEssenceCategory Category { get { return EEssenceCategory.THING; } }

		public override EMaterial AllowedMaterials { get { return EMaterial.METAL|EMaterial.MINERAL|EMaterial.WOOD; } }

		public void Paint(FColor _lerpColor) { m_lerpColor = _lerpColor; }
	}
}