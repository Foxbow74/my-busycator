using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class HelloWorld2 : AbstractGameTest2
	{
		[TestMethod]
		public void Взаимодействие()
		{
			SendKey(ConsoleKey.Y);
		}
	}
}