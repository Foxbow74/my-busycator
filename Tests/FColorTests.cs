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
			//Assert.AreEqual(FColor.White, FColor.White.Screen(FColor.Black));
			//Assert.AreEqual(FColor.White, FColor.Black.Screen(FColor.White));

			var halfWhite = new FColor(0.5f, 1f, 1f, 1f);
			Assert.AreEqual(FColor.White, halfWhite.Screen(halfWhite));

			Assert.AreEqual(FColor.White, FColor.Red.UpdateAlfa(0.5f).Screen(new FColor(0.5f,0,1f,0)));
		}
	}
}