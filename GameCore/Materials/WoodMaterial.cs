namespace GameCore.Materials
{
	public abstract class WoodMaterial : Material
	{
		protected WoodMaterial(string _name)
			: base(_name) { }

		public override EMaterial MaterialType { get { return EMaterial.WOOD; } }

		public abstract int TreeTileIndex { get; }
	}
}