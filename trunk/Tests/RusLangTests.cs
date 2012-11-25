using Microsoft.VisualStudio.TestTools.UnitTesting;
using RusLanguage;

namespace Tests
{
	[TestClass]
	public class RusLangTests
	{
		[TestMethod]
		public void Меди()
		{
			Assert.AreEqual("меди", Sklonenia.ToPadej(EPadej.ROD, "медь", false, ESex.FEMALE));
		}
		[TestMethod]
		public void Табуретки()
		{
			Assert.AreEqual("табуретки", Sklonenia.ToPadej(EPadej.ROD, "табуретка", false, ESex.FEMALE));
		}
		[TestMethod]
		public void Табуреткой()
		{
			Assert.AreEqual("табуреткой", Sklonenia.ToPadej(EPadej.TVOR, "табуретка", false, ESex.FEMALE));
		}
		[TestMethod]
		public void Спесью()
		{
			Assert.AreEqual("спесью", Sklonenia.ToPadej(EPadej.TVOR, "спесь", false, ESex.FEMALE));
		}
		[TestMethod]
		public void Волка()
		{
			Assert.AreEqual("волка", Sklonenia.ToPadej(EPadej.ROD, "волк", false, ESex.MALE));
		}
	}
}
