using System;
using System.Diagnostics;
using GameCore;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Essences;
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

			noun = noun + "брадобрея".AsNoun(ESex.FEMALE, true);
			Assert.AreEqual("лысой брадобрее", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonIt()
		{
			var noun = "яйцо".AsNoun(ESex.IT, true);
			Assert.AreEqual("яйцу", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысому яйцу", noun.To(EPadej.DAT));

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
			Assert.AreEqual("лысым брадобреям", noun.To(EPadej.DAT));
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
			var noun = "яйцо".AsNoun(ESex.IT, true);
			Assert.AreEqual("яйцу", noun.To(EPadej.DAT));

			noun = noun + "лысый".AsAdj();
			Assert.AreEqual("лысому яйцу", noun.To(EPadej.DAT));

			noun = noun + "брадобрей".AsNoun(ESex.MALE, true);
			Assert.AreEqual("лысому яйцу", noun.To(EPadej.DAT));
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

	[TestClass]
	public class AbstractLanguageTestEnums
	{
		[TestMethod]
		public void EEquipmentPlacesTest()
		{
			var lp = new RusLanguageProcessor();
			foreach (EEquipmentPlaces places in Enum.GetValues(typeof(EEquipmentPlaces)))
			{
				Debug.WriteLine(lp.GetString(EALSentence.NONE, lp.AsNoun(places)));
			}
		}

		[TestMethod]
		public void ETacticsTest()
		{
			var lp = new RusLanguageProcessor();
			foreach (ETactics places in Enum.GetValues(typeof(ETactics)))
			{
				Debug.WriteLine(lp.GetString(EALSentence.TACTICK_CHANGED, lp.AsNoun(places)));
			}
		}

		[TestMethod]
		public void EActionCategoryTest()
		{
			var lp = new RusLanguageProcessor();
			foreach (EActionCategory places in Enum.GetValues(typeof(EActionCategory)))
			{
				Debug.WriteLine(lp.GetString(EALSentence.NONE, lp.AsNoun(places)));
			}
		}

		
		[TestMethod]
		public void ETerrainsTest()
		{
			var lp = new RusLanguageProcessor();
			foreach (ETerrains places in Enum.GetValues(typeof(ETerrains)))
			{
				Debug.WriteLine(lp.GetString(EALSentence.GENERAL, lp.AsNoun(places)));
			}
		}

		[TestMethod]
		public void EItemCategoryTest()
		{
			var lp = new RusLanguageProcessor();
			foreach (EItemCategory places in Enum.GetValues(typeof(EItemCategory)))
			{
				Debug.WriteLine(lp.GetString(EALSentence.GENERAL, lp.AsNoun(places)));
			}
		}

	}

	[TestClass]
	public class AbstractLanguageNounsTestEnums
	{
		[TestInitialize]
		public void Initialize()
		{
			MagickSetting.MagicSettingProvider.Init();

			foreach (ETileset tileset in Enum.GetValues(typeof(ETileset)))
			{
				TileSetInfoProvider.SetOpacity(tileset, 0, 0f);
			}

			if (World.TheWorld == null)
			{
				World.LetItBeeee(new RusLanguageProcessor());
			}
		}

		[TestMethod]
		public void EALNounsTest()
		{
			var lp = new RusLanguageProcessor();
			foreach (EALNouns places in Enum.GetValues(typeof(EALNouns)))
			{
				Debug.WriteLine(lp.GetString(EALSentence.GENERAL, lp.AsNoun(places)));
			}
		}
	}
}
