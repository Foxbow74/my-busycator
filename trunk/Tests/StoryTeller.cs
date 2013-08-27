using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class StoryTeller : AbstractGameTest2
	{
		[TestMethod]
		public void Битвище()
		{
			2.Repeat(() => SendKey(ConsoleKey.NumPad3));
			40.Repeat(() => SendKey(ConsoleKey.Y));
			10.Repeat(() => SendKey(ConsoleKey.NumPad3));
			100.Repeat(() =>
			           	{
							SendKey(ConsoleKey.NumPad1);
							SendKey(ConsoleKey.NumPad4);
							SendKey(ConsoleKey.NumPad7);
							SendKey(ConsoleKey.NumPad8);
							SendKey(ConsoleKey.NumPad9);
							SendKey(ConsoleKey.NumPad6);
							SendKey(ConsoleKey.NumPad3);
							SendKey(ConsoleKey.NumPad2);

							SendKey(ConsoleKey.T);
							SendKey(ConsoleKey.T);

							5.Repeat(() => SendKey(ConsoleKey.NumPad5));
			           	});

			
			//Debug.WriteLine(Avatar[0, 0].LiveMapBlock.Creatures.Count());
		}
	}
}