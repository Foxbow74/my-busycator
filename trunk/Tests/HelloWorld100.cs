using System;
using GameCore;
using GameCore.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class HelloWorld100 : AbstractGameTest100
	{
		[TestMethod]
		public void ВечныйБег()
		{
			using (new Profiler())
			{
				for (int i = 0; i < 10000; i++)
				{
					switch (World.Rnd.Next(8))
					{
						case 0:
							SendKey(ConsoleKey.NumPad4);
							break;
						case 1:
							SendKey(ConsoleKey.NumPad8);
							break;
						case 2:
							SendKey(ConsoleKey.NumPad2);
							break;
						case 3:
							SendKey(ConsoleKey.NumPad6);
							break;
						case 4:
							SendKey(ConsoleKey.NumPad1);
							break;
						case 5:
							SendKey(ConsoleKey.NumPad3);
							break;
						case 6:
							SendKey(ConsoleKey.NumPad4);
							break;
						case 7:
							SendKey(ConsoleKey.NumPad9);
							break;
					}
				}
			}
		}
	}
}