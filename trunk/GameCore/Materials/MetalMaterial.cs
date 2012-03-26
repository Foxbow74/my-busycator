using RusLanguage;

namespace GameCore.Materials
{
	internal abstract class MetalMaterial : Material
	{
		protected MetalMaterial(string _name) : base(_name) { }

		public override EMaterial MaterialType { get { return EMaterial.METAL; } }
	}

	internal class CopperMaterial : MetalMaterial
	{
		public CopperMaterial() : base("медь") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.CoolCopper; } }
	}

	internal class BrassMaterial : MetalMaterial
	{
		public BrassMaterial()
			: base("латунь") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.Brass; } }
	}

	internal class BronzeMaterial : MetalMaterial
	{
		public BronzeMaterial()
			: base("бронза") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.Bronze; } }
	}
}