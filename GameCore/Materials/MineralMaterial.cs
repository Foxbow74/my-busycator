namespace GameCore.Materials
{
	public abstract class MineralMaterial : Material
	{
		protected MineralMaterial(string _name)
			: base(_name) { }

		public override EMaterial MaterialType { get { return EMaterial.MINERAL; } }
	}
}