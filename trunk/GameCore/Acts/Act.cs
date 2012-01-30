#region

using System;
using System.Collections.Generic;
using GameCore.Creatures;

#endregion

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
	}

	public abstract class Act
	{
		private List<Tuple<Type, object>> m_parameters;

		private int m_takeTicks;

		protected Act(int _takeTicks)
		{
			m_takeTicks = _takeTicks;
			Count = 1;
		}

		/// <summary>
		/// Сколько раз произведено действие (например взято 10 топоров)
		/// </summary>
		public int Count { get; protected set; }

		public int TakeTicks { get { return m_takeTicks*Count; } }

		public bool IsCancelled { get; set; }

		public abstract EActResults Do(Creature _creature, bool _silence);

		public void AddParameter<T>(T _param)
		{
			if (m_parameters == null) m_parameters = new List<Tuple<Type, object>>();
			m_parameters.Add(new Tuple<Type, object>(typeof (T), _param));
		}

		public IEnumerable<T> GetParameter<T>()
		{
			if (m_parameters == null) yield break;

			foreach (var tuple in m_parameters)
			{
				if (typeof (T).IsAssignableFrom(tuple.Item1)) yield return (T) tuple.Item2;
			}
		}
	}
}