namespace GameCore.Materials
{
	public abstract class MushrumMaterial : Material
	{
		protected MushrumMaterial(string _name) : base(string.Empty) { MushrumName = _name; }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override EMaterial MaterialType { get { return EMaterial.MUSHRUM; } }

		public abstract int MushrumTileIndex { get; }

		public string MushrumName { get; private set; }
	}
}