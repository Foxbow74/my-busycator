namespace GameCore.Materials
{
	internal class BodyMaterial : Material
	{
		public BodyMaterial()
			: base("плоть") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.WhiteSmoke; } }

		public override EMaterial MaterialType { get { return EMaterial.BODY; } }
	}
}