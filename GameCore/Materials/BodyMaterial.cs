namespace GameCore.Materials
{
	internal class BodyMaterial : Material
	{
		public BodyMaterial()
			: base("хуевый") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.WhiteSmoke; } }

		public override EMaterialType MaterialType { get { return EMaterialType.BODY; } }
	}
}