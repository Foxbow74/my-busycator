using System;
using System.Diagnostics;
using System.Linq;
using GameCore.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class HelloWorld2 : AbstractGameTest2
	{
		[TestMethod]
		public void ЗапускайтеКабанчика()
		{
			using (new Profiler())
			{
				for (int i = 0; i < 10; i++)
				{
					SendKey(ConsoleKey.Y);
				}
			}
		}
	}
}