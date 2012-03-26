using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore
{
	[Flags] public enum EKeyModifiers
	{
		NONE = 0x0,
		SHIFT = 0x1,
		CTRL = 0x2,
		ALT = 0x4,
	}

	public static class KeyTranslator
	{
		private static readonly ConsoleKey[] m_moveKeys = new[]
		                                                  	{
		                                                  		ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow,
		                                                  		ConsoleKey.RightArrow, ConsoleKey.NumPad1, ConsoleKey.NumPad2,
		                                                  		ConsoleKey.NumPad3, ConsoleKey.NumPad4, ConsoleKey.NumPad5,
		                                                  		ConsoleKey.NumPad6, ConsoleKey.NumPad7, ConsoleKey.NumPad8,
		                                                  		ConsoleKey.NumPad9, ConsoleKey.Home
		                                                  		, ConsoleKey.PageUp,
		                                                  		ConsoleKey.PageDown, ConsoleKey.End
		                                                  	};

		private static readonly Dictionary<Tuple<ConsoleKey, EKeyModifiers>, Type> m_acts =
			new Dictionary<Tuple<ConsoleKey, EKeyModifiers>, Type>();

		static KeyTranslator()
		{
			foreach (var type in GetActTypes())
			{
				var act = GetAct(type);
				foreach (var tuple in act.ConsoleKeys)
				{
					m_acts.Add(tuple, type);
				}
			}
		}

		public static ConsoleKey[] MoveKeys { get { return m_moveKeys; } }

		public static IEnumerable<Act> RegisteredActs { get { return m_acts.Select(_pair => _pair.Value).Distinct().Select(GetAct); } }

		public static Act TranslateKey(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var tuple = new Tuple<ConsoleKey, EKeyModifiers>(_key, _modifiers);
			Type type;
			if (!m_acts.TryGetValue(tuple, out type))
			{
				return null;
			}
			var act = GetAct(type);
			act.AddParameter(_key);
			act.AddParameter(_modifiers);
			return act;
		}

		private static Act GetAct(Type _type) { return (Act) Activator.CreateInstance(_type); }

		private static IEnumerable<Type> GetActTypes()
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.GetTypes())
				{
					if (typeof (Act).IsAssignableFrom(type) && !type.IsAbstract && !typeof (ISpecial).IsAssignableFrom(type))
					{
						yield return type;
					}
				}
			}
		}

		public static Point GetDirection(ConsoleKey _key)
		{
			if (!MoveKeys.Contains(_key))
			{
				return null;
			}

			var dx = (_key == ConsoleKey.LeftArrow ? -1 : 0) + (_key == ConsoleKey.RightArrow ? 1 : 0);
			var dy = (_key == ConsoleKey.UpArrow ? -1 : 0) + (_key == ConsoleKey.DownArrow ? 1 : 0);

			dx += (_key == ConsoleKey.NumPad4 ? -1 : 0) + (_key == ConsoleKey.NumPad6 ? 1 : 0);

			dx += (_key == ConsoleKey.NumPad7 ? -1 : 0) + (_key == ConsoleKey.NumPad9 ? 1 : 0);
			dx += (_key == ConsoleKey.NumPad1 ? -1 : 0) + (_key == ConsoleKey.NumPad3 ? 1 : 0);

			dx += (_key == ConsoleKey.Home ? -1 : 0) + (_key == ConsoleKey.PageUp ? 1 : 0);
			dx += (_key == ConsoleKey.End ? -1 : 0) + (_key == ConsoleKey.PageDown ? 1 : 0);

			dy += (_key == ConsoleKey.NumPad8 ? -1 : 0) + (_key == ConsoleKey.NumPad2 ? 1 : 0);

			dy += (_key == ConsoleKey.NumPad7 ? -1 : 0) + (_key == ConsoleKey.NumPad1 ? 1 : 0);
			dy += (_key == ConsoleKey.NumPad9 ? -1 : 0) + (_key == ConsoleKey.NumPad3 ? 1 : 0);

			dy += (_key == ConsoleKey.Home ? -1 : 0) + (_key == ConsoleKey.End ? 1 : 0);
			dy += (_key == ConsoleKey.PageUp ? -1 : 0) + (_key == ConsoleKey.PageDown ? 1 : 0);

			return new Point(dx, dy);
		}
	}
}