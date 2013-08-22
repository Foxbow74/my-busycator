using GameCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class FColorTests
	{
		[TestMethod]
		public void Screen()
		{
			var halfWhite = new FColor(0.5f, 1f, 1f, 1f);
			var result = new FColor(0.75f, 1f, 1f, 1f);
			Assert.AreEqual(result, halfWhite.Screen(halfWhite));
		}
	}
}