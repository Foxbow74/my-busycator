using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class UnitTest1 : AbstractGameTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			World.TheWorld.KeyPressed(ConsoleKey.UpArrow, EKeyModifiers.NONE);
		}
	}

	[TestClass]
	public abstract class AbstractGameTest
	{
		private static int m_test = 0;
		
		static AbstractGameTest()
		{
			World.LetItBeeee();
		}

		[TestInitialize]
		public void Initialize()
		{
	
		}

		[TestCleanupAttribute]
		public void Cleanup()
		{
		}
	}
}
