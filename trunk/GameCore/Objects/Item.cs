namespace GameCore.Objects
{
	/// <summary>
	/// 	��, ��� �������� ����� ����� � �������� � ���������
	/// </summary>
	public abstract class Item : Thing
	{
		public override float Opaque
		{
			get { return 0.1f; }
		}
	}
}