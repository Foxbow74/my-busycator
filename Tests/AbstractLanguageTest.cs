using System.Diagnostics;
using System.Threading.Tasks;
using GameCore;
using LanguagePack;
using GameCore.AbstractLanguage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class AbstractLanguageTest
	{
		[TestMethod]
		public void Simplest()
		{
			var noun = "козел".AsNoun(ESex.MALE, true);
			noun = noun + "брадобрей".AsTitle(ESex.MALE, true);
			noun = noun + "лысый".AsAdv();
		}

		[TestMethod]
		public void NounToString()
		{
			//Parallel.For(1, 200, _l => Debug.WriteLine(_l));

			var noun = "козел".AsNoun(ESex.MALE, true);
			Assert.AreEqual("козел", noun.To(EPadej.IMEN));

			noun = noun + "лысый".AsAdv();
			Assert.AreEqual("лысый козел", noun.To(EPadej.IMEN));

			noun = noun + "брадобрей".AsTitle(ESex.MALE, true);
			Assert.AreEqual("лысый козел-брадобрей", noun.To(EPadej.IMEN));
		}

		[TestMethod]
		public void Sklon()
		{
			var noun = "баран".AsNoun(ESex.MALE, true);
			Assert.AreEqual("барану", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdv();
			Assert.AreEqual("лысому барану", noun.To(EPadej.DAT));

			noun = noun + "брадобрей".AsTitle(ESex.MALE, true);
			Assert.AreEqual("лысому барану-брадобрею", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonFemale()
		{
			var noun = "коза".AsNoun(ESex.FEMALE, true);
			Assert.AreEqual("козе", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdv();
			Assert.AreEqual("лысой козе", noun.To(EPadej.DAT));

			noun = noun + "брадобрей".AsTitle(ESex.MALE, true);
			Assert.AreEqual("лысой козе-брадобрею", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonIt()
		{
			var noun = "€йцо".AsNoun(ESex.IT, true);
			Assert.AreEqual("€йцу", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdv();
			Assert.AreEqual("лысому €йцу", noun.To(EPadej.DAT));

			noun = noun + "брадобрей".AsTitle(ESex.MALE, true);
			Assert.AreEqual("лысому €йцу-брадобрею", noun.To(EPadej.DAT));
		}
		
		[TestMethod]
		public void SklonPlural()
		{
			var noun = "комары".AsNoun(ESex.PLURAL, true);
			Assert.AreEqual("комарам", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdv();
			Assert.AreEqual("лысым комарам", noun.To(EPadej.DAT));

			noun = noun + "брадобреи".AsTitle(ESex.PLURAL, true);
			Assert.AreEqual("лысым комарам-брадобре€м", noun.To(EPadej.DAT));
		}
	}
}
