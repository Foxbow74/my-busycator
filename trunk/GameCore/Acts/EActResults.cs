namespace GameCore.Acts
{
	public enum EActResults
	{
		/// <summary>
		/// происходит, если действие невозможно, например съесть еду которой нет и текущее действие заместилось например ожиданием
		/// </summary>
		ACT_REPLACED,
		
		/// <summary>
		/// выполнено успешно
		/// </summary>
		DONE,

		/// <summary>
		/// требуется уточнение
		/// </summary>
		NEED_ADDITIONAL_PARAMETERS,

		/// <summary>
		/// Действие провалилось
		/// </summary>
		FAIL,

		/// <summary>
		/// Операция провалена, но время почти не потрачено
		/// </summary>
		QUICK_FAIL,

		/// <summary>
		/// Введена для вложенных вызовов, означает что размышления продолжаются
		/// </summary>
	    NONE
	}
}