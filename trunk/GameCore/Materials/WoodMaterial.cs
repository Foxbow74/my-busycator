namespace GameCore.Materials
{
	public abstract class WoodMaterial : Material
	{
		protected WoodMaterial(string _name)
			: base(_name) { }

		public override EMaterialType MaterialType { get { return EMaterialType.WOOD; } }

		public abstract int TreeTileIndex { get; }
	}
}