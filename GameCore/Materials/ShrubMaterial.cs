namespace GameCore.Materials
{
	public abstract class ShrubMaterial: Material
	{
		protected ShrubMaterial(string _name) : base(string.Empty) { ShroobName = _name; }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override EMaterial MaterialType { get { return EMaterial.SHRUB; } }

		public string ShroobName { get; private set; }
		
		public abstract int ShroobTileIndex { get; }
	}
}
