using System;
using GameCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class HelloWorld : AbstractGameTest0
	{
		[TestMethod]
		public void СнегПодНогами()
		{
			Assert.AreEqual(ETerrains.ETERNAL_SNOW, Avatar[0, 0].Terrain);
		}

		[TestMethod]
		public void ШагВПустоту()
		{
			var current = Avatar[0, 0].WorldCoords;
			SendKey(ConsoleKey.UpArrow);
			Assert.AreEqual(current, Avatar[0, 0].WorldCoords);
		}


		[TestMethod]
		public void ТыКто()
		{
			var current = Avatar[0, 0].WorldCoords;
			SendKey(ConsoleKey.Y);
		}
	}
}
