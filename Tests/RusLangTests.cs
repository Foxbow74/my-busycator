using Microsoft.VisualStudio.TestTools.UnitTesting;
using RusLanguage;

namespace Tests
{
	[TestClass]
	public class RusLangTests
	{
		[TestMethod]
		public void TestMethod1()
		{
			Assert.AreEqual("табуретки", Sklonenia.ToPadej(EPadej.ROD, "табуретка", false, ESex.FEMALE));
			Assert.AreEqual("табуреткой", Sklonenia.ToPadej(EPadej.TVOR, "табуретка", false, ESex.FEMALE));

			Assert.AreEqual("спесью", Sklonenia.ToPadej(EPadej.TVOR, "спесь", false, ESex.FEMALE));
		}
	}
}
