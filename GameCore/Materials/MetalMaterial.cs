using RusLanguage;

namespace GameCore.Materials
{
	abstract class MetalMaterial:Material
	{
		protected MetalMaterial(string _name) : base(_name)
		{
		}

		public override EMaterial MaterialType
		{
			get { return EMaterial.METAL; }
		}
	}

	class CopperMaterial:MetalMaterial
	{
		public CopperMaterial() : base("медь")
		{
			Sex = ESex.FEMALE;
		}

		public override FColor LerpColor
		{
			get { return FColor.CoolCopper; }
		}
	}

	class BrassMaterial : MetalMaterial
	{
		public BrassMaterial()
			: base("латунь")
		{
			Sex = ESex.FEMALE;
		}

		public override FColor LerpColor
		{
			get { return FColor.Brass; }
		}
	}

	class BronzeMaterial : MetalMaterial
	{
		public BronzeMaterial()
			: base("бронза")
		{
			Sex = ESex.FEMALE;
		}

		public override FColor LerpColor
		{
			get { return FColor.Bronze; }
		}
	}
}
