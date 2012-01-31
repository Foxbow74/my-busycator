namespace GameCore.Objects
{
	/// <summary>
	/// 	��, ��� �������� ����� ����� � �������� � ���������
	/// </summary>
	public abstract class Item : Thing
	{
		public override float Opaque
		{
			get { return 0; }
		}

		public override int GetHashCode()
		{
			return CalcHashCode();
		}

		protected int CalcHashCode()
		{
			return GetType().GetHashCode();
		}
	}
}