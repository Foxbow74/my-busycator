using System;
using System.Linq;
using GameCore.Misc;
using Microsoft.Xna.Framework.Input;

namespace RGL1
{
	public static class KeyHelper
	{

		private static readonly Keys[] m_keyModificators = new[]
		                                            	{
		                                            		Keys.RightShift, Keys.LeftShift, Keys.RightControl, Keys.LeftControl,
		                                            		Keys.RightAlt, Keys.LeftAlt
		                                            	};

		private static readonly Keys[] m_moveKeys = new[]
		                                     	{
		                                     		Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad2,
		                                     		Keys.NumPad3, Keys.NumPad4, Keys.NumPad5,
		                                     		Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.NumPad5, Keys.Home
		                                     		, Keys.PageUp,
		                                     		Keys.PageDown, Keys.End
		                                     	};

		public static Keys[] KeyModificators
		{
			get { return m_keyModificators; }
		}

		public static Keys[] MoveKeys
		{
			get { return m_moveKeys; }
		}

		public static Point GetDirection(ConsoleKey _key)
		{
			if(!MoveKeys.Contains((Keys)_key))
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
