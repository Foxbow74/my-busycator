namespace GameCore.Acts
{
	public enum EActResults
	{
		/// <summary>
		/// ����������, ���� �������� ����������, �������� ������ ��� ������� ��� � ������� �������� ����������� �������� ���������
		/// </summary>
		ACT_REPLACED,
		
		/// <summary>
		/// ��������� �������
		/// </summary>
		DONE,

		/// <summary>
		/// ��������� ���������
		/// </summary>
		NEED_ADDITIONAL_PARAMETERS,

		/// <summary>
		/// �������� �����������
		/// </summary>
		FAIL,

		/// <summary>
		/// �������� ���������, �� ����� ����� �� ���������
		/// </summary>
		QUICK_FAIL,

		/// <summary>
		/// ������� ��� ��������� �������, �������� ��� ����������� ������������
		/// </summary>
	    NONE,

		WORLD_STAYS_UNCHANGED,
	}
}