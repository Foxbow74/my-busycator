using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Acts
{
	public abstract class Act
	{
		#region Fields

		private List<Tuple<Type, object>> m_parameters;

		#endregion

		#region .ctor

		protected Act()
		{
			Count = 1;
		}

		#endregion

		#region Methods

		public abstract EActResults Do(Creature _creature);

		protected EActResults Find(Creature _creature, Func<Essence, LiveMapCell, bool> _predicate, out LiveMapCell _liveMapCell)
		{
			_liveMapCell = null;

			var list = new List<Point>();
			foreach (var point in Point.NearestDPoints)
			{
				var cc = _creature[point];
				if (_predicate(cc.Thing, cc))
				{
					list.Add(point);
				}
				else if (cc.GetAllAvailableItemDescriptors<Thing>(_creature).Any(_descriptor => _predicate(_descriptor.Essence, cc)))
				{
					list.Add(point);
				}
			}
			if (_creature.GetBackPackItems().Any(_descriptor => _predicate(_descriptor.Essence, null)))
			{
				list.Add(Point.Zero);
			}

			var coords = list.Distinct().ToList();

			if (GetParameter<Point>().Any())
			{
				coords = coords.Intersect(GetParameter<Point>()).ToList();
			}

			if (!coords.Any())
			{
				return EActResults.QUICK_FAIL;
			}
			if (coords.Count() > 1)
			{
				MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.ASK_DIRECTION));
				return EActResults.NEED_ADDITIONAL_PARAMETERS;
			}
			_liveMapCell = _creature[coords.First()];
			return EActResults.NONE;
		}

		#endregion

		#region parameters

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

		public T GetFirstParameter<T>()
		{
			if (m_parameters != null)
			{
				foreach (var tuple in m_parameters)
				{
					if (typeof (T).IsAssignableFrom(tuple.Item1))
					{
						return (T) tuple.Item2;
					}
				}
			}
			throw new InstanceNotFoundException();
		}

		public IEnumerable<T> GetParameter<T>()
		{
			if (m_parameters == null) yield break;

			foreach (var tuple in m_parameters)
			{
				if (typeof (T).IsAssignableFrom(tuple.Item1)) yield return (T) tuple.Item2;
			}
		}

		public bool TryGetParameter<T>(out T _value)
		{
			if (m_parameters != null)
			{
				foreach (var tuple in m_parameters)
				{
					if (typeof (T).IsAssignableFrom(tuple.Item1))
					{
						_value = (T) tuple.Item2;
						return true;
					}
				}
			}
			_value = default(T);
			return false;
		}

		#endregion

		#region Properties

		public abstract EActionCategory Category { get; }
		public abstract IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get; }

		/// <summary>
		/// 	Сколько раз произведено действие (например взято 10 топоров)
		/// </summary>
		public int Count { get; protected set; }

		public virtual string HelpKeys
		{
			get
			{
				var sb = new StringBuilder();
				foreach (var tuple in ConsoleKeys)
				{
					var keyName = tuple.Item1.ToString().ToLower();
					var keyModifiers = tuple.Item2;

					switch (tuple.Item1)
					{
						case ConsoleKey.OemComma:
							if ((keyModifiers & EKeyModifiers.SHIFT) == EKeyModifiers.SHIFT)
							{
								keyName = "<";
								keyModifiers ^= EKeyModifiers.SHIFT;
							}
							else
							{
								keyName = ",";
							}
							break;
						case ConsoleKey.OemPeriod:
							if ((keyModifiers & EKeyModifiers.SHIFT) == EKeyModifiers.SHIFT)
							{
								keyName = ">";
								keyModifiers ^= EKeyModifiers.SHIFT;
							}
							else
							{
								keyName = ".";
							}
							break;
						case ConsoleKey.Oem2:
							if ((keyModifiers & EKeyModifiers.SHIFT) == EKeyModifiers.SHIFT)
							{
								keyName = "?";
								keyModifiers ^= EKeyModifiers.SHIFT;
							}
							else
							{
								keyName = "/";
							}

							break;
					}
					var modifierName = "";
					foreach (EKeyModifiers modifiers in Enum.GetValues(typeof (EKeyModifiers)))
					{
						if ((keyModifiers & modifiers) == modifiers)
						{
							switch (modifiers)
							{
								case EKeyModifiers.NONE:
									break;
								case EKeyModifiers.SHIFT:
									var upper = keyName.ToUpper();
									if (upper != keyName)
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
					if (sb.Length == 0)
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

		public abstract string HelpText { get; }

		public bool IsCancelled { get; set; }

		public abstract string Name { get; }

		public int TakeTicks
		{
			get { return TakeTicksOnSingleAction*Count; }
		}

		protected abstract int TakeTicksOnSingleAction { get; }

		#endregion
	}
}