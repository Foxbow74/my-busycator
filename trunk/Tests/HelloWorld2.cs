using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Messages;
using GameCore.Misc;
using LanguagePack;
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
				for (var i = 0; i < 20; i++)
				{
					SendKey(ConsoleKey.Y);
				}
			}
		}
	}
}