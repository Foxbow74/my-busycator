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
		/// 	требуетс€ уточнение
		/// </summary>
		NEED_ADDITIONAL_PARAMETERS,
		/// <summary>
		/// 	ƒействие провалилось
		/// </summary>
		FAIL,
		/// <summary>
		/// ќпераци€ провалена, но врем€ почти не потрачено
		/// </summary>
		QUICK_FAIL,
	}
}