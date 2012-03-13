namespace GameCore.Materials
{
	abstract class WoodMaterial : Material
	{
		protected WoodMaterial(string _name)
			: base(_name)
		{
		}

		public override EMaterial MaterialType
		{
			get { return EMaterial.WOOD; }
		}
	}

	class OakMaterial : WoodMaterial
	{
		public OakMaterial()
			: base("дуб")
		{
		}

		public override FColor LerpColor
		{
			get { return FColor.DarkOak; }
		}
	}


	class MappleMaterial : WoodMaterial
	{
		public MappleMaterial()
			: base("клен")
		{
		}

		public override FColor LerpColor
		{
			get { return FColor.Maple; }
		}
	}
}