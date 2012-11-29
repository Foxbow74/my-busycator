namespace GameCore.Materials
{
	public abstract class MetalMaterial : Material
	{
		protected MetalMaterial(string _name) : base(_name) { }

		public override EMaterialType MaterialType { get { return EMaterialType.METAL; } }
	}
}