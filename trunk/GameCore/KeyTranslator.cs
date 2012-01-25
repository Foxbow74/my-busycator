﻿#region

using System;

#endregion

namespace GameCore
{
	[Flags]
	public enum EKeyModifiers
	{
		NONE = 0x0,
		SHIFT = 0x1,
		CTRL = 0x2,
		ALT = 0x4,
	}

	public static class KeyTranslator
	{
		public static ECommands TranslateKey(ConsoleKey _keys, EKeyModifiers _modifiers)
		{
			switch (_keys)
			{
				case ConsoleKey.OemComma:
					return ECommands.TAKE;
				case ConsoleKey.O:
					return ECommands.OPEN;
			}
			return ECommands.NONE;
		}
	}
}