using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Creatures;

namespace GameCore.Acts
{
	public abstract class Act
	{
		private List<Tuple<Type, object>> m_parameters;

		protected Act()
		{
			Count = 1;
		}

		protected abstract int TakeTicksOnSingleAction { get; }

		/// <summary>
		/// Сколько раз произведено действие (например взято 10 топоров)
		/// </summary>
		public int Count { get; protected set; }

		public int TakeTicks { get { return TakeTicksOnSingleAction * Count; } }

		public bool IsCancelled { get; set; }

		public abstract EActResults Do(Creature _creature, bool _silence);

		public void AddParameter<T>(T _param)
		{
			if (m_parameters == null) m_parameters = new List<Tuple<Type, object>>();
			m_parameters.Add(new Tuple<Type, object>(typeof(T), _param));
		}

		public void AddParameter(Type _type, object _param)
		{
			if (m_parameters == null) m_parameters = new List<Tuple<Type, object>>();
			m_parameters.Add(new Tuple<Type, object>(_type, _param));
		}

		public IEnumerable<T> GetParameter<T>()
		{
			if (m_parameters == null) yield break;

			foreach (var tuple in m_parameters)
			{
				if (typeof (T).IsAssignableFrom(tuple.Item1)) yield return (T) tuple.Item2;
			}
		}

		public abstract IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get; }

		public abstract string Name { get; }
		
		public abstract string HelpText { get; }

		public virtual string HelpKeys
		{
			get
			{
				var sb = new StringBuilder();
				foreach (var tuple in ConsoleKeys)
				{
					var keyName = tuple.Item1.ToString().ToLower();
					switch (tuple.Item1)
					{
						case ConsoleKey.OemComma:
							keyName = ",";
							break;
						case ConsoleKey.Oem2:
							keyName = "?";
							break;
					}
					var modifierName = "";
					foreach (EKeyModifiers modifiers in Enum.GetValues(typeof(EKeyModifiers)))
					{
						if((tuple.Item2 & modifiers)==modifiers)
						{
							switch (modifiers)
							{
								case EKeyModifiers.NONE:
									break;
								case EKeyModifiers.SHIFT:
									var upper = keyName.ToUpper();
									if(upper!=keyName)
									{
										keyName = upper;
									}
									else
									{
										modifierName += "Shift+";
									}
									break;
								case EKeyModifiers.CTRL:
									modifierName += "^";
									break;
								case EKeyModifiers.ALT:
									modifierName += "Alt+";
									break;
								default:
									throw new ArgumentOutOfRangeException();
							}
						}
					}
					var s = string.Format("{0}{1}", modifierName, keyName);
					if (sb.Length==0)
					{
						sb.Append(s);
					} 
					else
					{
						sb.Append(", " + s);
					}
				}
				return sb.ToString();
			}
		}

		public abstract EActionCategory Category { get; }
	}
}