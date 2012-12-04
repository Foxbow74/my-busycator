using GameCore.AbstractLanguage;

namespace GameCore.Essences
{
	public abstract class Thing : Essence
	{
		private FColor m_lerpColor;

		protected Thing(Noun _name, Material _material) : base(_name, _material) { m_lerpColor = _material == null ? FColor.Empty : _material.LerpColor; }

		public override FColor LerpColor { get { return m_lerpColor; } }

        public override ETileset Tileset{ get { return ETileset.THINGS; } }

		public override EMaterialType AllowedMaterialsType { get { return EMaterialType.METAL|EMaterialType.MINERAL|EMaterialType.WOOD; } }

		public void Paint(FColor _lerpColor) { m_lerpColor = _lerpColor; }
	}
}