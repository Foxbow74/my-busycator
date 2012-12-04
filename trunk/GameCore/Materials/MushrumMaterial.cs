using GameCore.AbstractLanguage;

namespace GameCore.Materials
{
	public abstract class MushrumMaterial : Material
	{
		protected MushrumMaterial(Noun _name) : base(string.Empty) { MushrumName = _name; }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override EMaterialType MaterialType { get { return EMaterialType.MUSHRUM; } }

		public abstract int MushrumTileIndex { get; }

		public Noun MushrumName { get; private set; }
	}
}