using GameCore.AbstractLanguage;

namespace GameCore.Materials
{
	public abstract class ShrubMaterial: Material
	{
		protected ShrubMaterial(EALNouns _name) : base(null) { ShroobName = _name; }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override EMaterialType MaterialType { get { return EMaterialType.SHRUB; } }

		public EALNouns ShroobName { get; private set; }
		
		public abstract int ShroobTileIndex { get; }
	}
}
