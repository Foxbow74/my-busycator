﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;

namespace GameCore.Messages
{
	public abstract class AskMessage : Message
	{
		private List<Tuple<Type, object>> m_parameters;

		protected AskMessage(Act _act, params object[] _params)
		{
			Act = _act;
			foreach (var o in _params)
			{
				AddParameter(o.GetType(), o);
			}
		}

		public Act Act { get; private set; }

		public void AddParameter<T>(T _param)
		{
			if (m_parameters == null) m_parameters = new List<Tuple<Type, object>>();
			m_parameters.Add(new Tuple<Type, object>(typeof (T), _param));
		}

		public void AddParameter(Type _type, object _param)
		{
			if (m_parameters == null) m_parameters = new List<Tuple<Type, object>>();
			m_parameters.Add(new Tuple<Type, object>(_type, _param));
		}

		public IEnumerable<T> GetParameters<T>()
		{
			if (m_parameters == null) yield break;

			foreach (var tuple in m_parameters)
			{
				if (typeof (T).IsAssignableFrom(tuple.Item1)) yield return (T) tuple.Item2;
			}
		}

		public T GetFirstParameter<T>()
		{
			if (m_parameters == null)
			{
				throw new ApplicationException();
			}
			return (T) m_parameters.Where(_tuple => typeof (T).IsAssignableFrom(_tuple.Item1)).Select(_tuple1 => _tuple1.Item2).First();
		}
	}
}