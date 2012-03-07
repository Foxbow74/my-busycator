namespace GameCore.Materials
{
	abstract class MineralMaterial : Material
	{
		protected MineralMaterial(string _name)
			: base(_name)
		{
		}

		public override EMaterial MaterialType
		{
			get { return EMaterial.MINERAL; }
		}
	}

	class StoneMaterial : MineralMaterial
	{
		public StoneMaterial()
			: base("камень")
		{
		}

		public override FColor LerpColor
		{
			get { return FColor.DarkGray; }
		}
	}
}