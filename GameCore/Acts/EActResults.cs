namespace GameCore.Acts
{
	public enum EActResults
	{
		/// <summary>
		/// 	происходит, если действие невозможно, например съесть еду которой нет
		/// </summary>
		NOTHING_HAPPENS,
		/// <summary>
		/// 	выполнено успешно
		/// </summary>
		DONE,
		/// <summary>
		/// 	требуется уточнение
		/// </summary>
		NEED_ADDITIONAL_PARAMETERS,
		/// <summary>
		/// 	Действие провалилось
		/// </summary>
		FAIL,
		/// <summary>
		/// 	Существо самоуничтожилось
		/// </summary>
		SHOULD_BE_REMOVED_FROM_QUEUE
	}
}