namespace GameCore.Objects
{
	/// <summary>
	/// 	��, ��� �������� ����� ����� � ��������
	/// </summary>
	public abstract class Item : Thing
	{
		protected Item(Material _material) : base(_material) { }

		public override EMaterial AllowedMaterials { get { return EMaterial.METAL | EMaterial.MINERAL | EMaterial.WOOD; } }
	}
}