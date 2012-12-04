using GameCore.AbstractLanguage;

namespace GameCore.Materials
{
	public abstract class ShrubMaterial: Material
	{
		protected ShrubMaterial(Noun _name) : base(string.Empty) { ShroobName = _name; }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override EMaterialType MaterialType { get { return EMaterialType.SHRUB; } }

		public Noun ShroobName { get; private set; }
		
		public abstract int ShroobTileIndex { get; }
	}
}
