using GameCore;
using LanguagePack;
using GameCore.AbstractLanguage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class AbstractLanguageTestYes
	{
		[TestInitialize]
		public void Initialize()
		{
			Sklonenia.YesNoAnswer = true;
		}

		[TestMethod]
		public void Simplest()
		{
			var noun = "козел".AsNoun(ESex.MALE, true);
			noun = noun + "брадобрей".AsNoun(ESex.MALE, true);
			noun = noun + "лысый".AsAdj();
		}

		[TestMethod]
		public void NounToString()
		{
			//Parallel.For(1, 200, _l => Debug.WriteLine(_l));

			var noun = "козел".AsNoun(ESex.MALE, true);
			Assert.AreEqual("козел", noun.To(EPadej.IMEN));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысый козел", noun.To(EPadej.IMEN));

			noun = noun + "брадобрей".AsNoun(ESex.MALE, true);
			Assert.AreEqual("лысый брадобрей", noun.To(EPadej.IMEN));
		}

		[TestMethod]
		public void Sklon()
		{
			var noun = "баран".AsNoun(ESex.MALE, true);
			Assert.AreEqual("барану", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысому барану", noun.To(EPadej.DAT));

			noun = noun + "брадобрей".AsNoun(ESex.MALE, true);
			Assert.AreEqual("лысому брадобрею", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonFemale()
		{
			var noun = "коза".AsNoun(ESex.FEMALE, true);
			Assert.AreEqual("козе", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысой козе", noun.To(EPadej.DAT));

			noun = noun + "брадобрей".AsNoun(ESex.MALE, true);
			Assert.AreEqual("лысой брадобрею", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonIt()
		{
			var noun = "€йцо".AsNoun(ESex.IT, true);
			Assert.AreEqual("€йцу", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысому €йцу", noun.To(EPadej.DAT));

			noun = noun + "брадобрей".AsNoun(ESex.MALE, true);
			Assert.AreEqual("лысому брадобрею", noun.To(EPadej.DAT));
		}
		
		[TestMethod]
		public void SklonPlural()
		{
			var noun = "комары".AsNoun(ESex.PLURAL, true);
			Assert.AreEqual("комарам", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысым комарам", noun.To(EPadej.DAT));

			noun = noun + "брадобреи".AsNoun(ESex.PLURAL, true);
			Assert.AreEqual("лысым брадобре€м", noun.To(EPadej.DAT));
		}
	}

	[TestClass]
	public class AbstractLanguageTestNo
	{
		[TestInitialize]
		public void Initialize()
		{
			Sklonenia.YesNoAnswer = false;
		}

		[TestMethod]
		public void NounToString()
		{
			//Parallel.For(1, 200, _l => Debug.WriteLine(_l));

			var noun = "козел".AsNoun(ESex.MALE, true);
			Assert.AreEqual("козел", noun.To(EPadej.IMEN));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысый козел", noun.To(EPadej.IMEN));

			noun = noun + "брадобрей".AsNoun(ESex.MALE, true);
			Assert.AreEqual("лысый козел", noun.To(EPadej.IMEN));
		}

		[TestMethod]
		public void Sklon()
		{
			var noun = "баран".AsNoun(ESex.MALE, true);
			Assert.AreEqual("барану", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысому барану", noun.To(EPadej.DAT));

			noun = noun + "брадобрей".AsNoun(ESex.MALE, true);
			Assert.AreEqual("лысому барану", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonFemale()
		{
			var noun = "коза".AsNoun(ESex.FEMALE, true);
			Assert.AreEqual("козе", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысой козе", noun.To(EPadej.DAT));

			noun = noun + "брадобрей".AsNoun(ESex.MALE, true);
			Assert.AreEqual("лысой козе", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonIt()
		{
			var noun = "€йцо".AsNoun(ESex.IT, true);
			Assert.AreEqual("€йцу", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысому €йцу", noun.To(EPadej.DAT));

			noun = noun + "брадобрей".AsNoun(ESex.MALE, true);
			Assert.AreEqual("лысому €йцу", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonPlural()
		{
			var noun = "комары".AsNoun(ESex.PLURAL, true);
			Assert.AreEqual("комарам", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысым комарам", noun.To(EPadej.DAT));

			noun = noun + "брадобреи".AsNoun(ESex.PLURAL, true);
			Assert.AreEqual("лысым комарам", noun.To(EPadej.DAT));
		}
	}
}
