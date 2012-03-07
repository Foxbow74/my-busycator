namespace GameCore.Objects
{
	/// <summary>
	/// 	��, ��� �������� ����� ����� � ��������
	/// </summary>
	public abstract class Item : Thing
	{
		protected Item(Material _material) : base(_material)
		{
		}

		public override System.Collections.Generic.IEnumerable<EMaterial> AllowedMaterials
		{
			get
			{
				yield return EMaterial.FLASH;
				yield return EMaterial.METAL;
				yield return EMaterial.MINERAL;
				yield return EMaterial.WOOD;
			}
		}
	}
}