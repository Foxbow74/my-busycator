namespace GameCore.Acts
{
	public enum EActResults
	{
		/// <summary>
		/// 	����������, ���� �������� ����������, �������� ������ ��� ������� ���
		/// </summary>
		NOTHING_HAPPENS,
		/// <summary>
		/// 	��������� �������
		/// </summary>
		DONE,
		/// <summary>
		/// 	��������� ���������
		/// </summary>
		NEED_ADDITIONAL_PARAMETERS,
		/// <summary>
		/// 	�������� �����������
		/// </summary>
		FAIL,
		/// <summary>
		/// 	�������� ����������������
		/// </summary>
		SHOULD_BE_REMOVED_FROM_QUEUE
	}
}