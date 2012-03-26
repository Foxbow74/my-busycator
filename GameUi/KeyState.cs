using System;
using System.Collections.Generic;
using GameCore;

namespace GameUi
{
	public class KeyState
	{
		public KeyState() { PressedKeys = new List<ConsoleKey>(); }

		public List<ConsoleKey> PressedKeys { get; private set; }

		public EKeyModifiers KeyModifiers { get; set; }
	}
}