using System;
using GameCore;
using GameCore.Essences;
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
			SendKey(ConsoleKey.Y);
		}

		[TestMethod]
		public void Шмотки()
		{
			var stack = EssenceHelper.GetRandomFakedItem<StackOfAmmo>(World.Rnd);
			Avatar[1, 0].AddItem(stack);
			var s = (StackOfAmmo)Avatar[1, 0].ResolveFakeItem(Avatar, stack);
			s.Count = 10;
			SendKey(ConsoleKey.NumPad6);
			SendKey(ConsoleKey.OemComma);
			SendKey(ConsoleKey.NumPad5);
		}
	}
}
