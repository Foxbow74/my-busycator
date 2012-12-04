using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore;
using GameCore.Creatures;
using GameCore.Creatures.Dummies;
using GameCore.Essences;
using LanguagePack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class RusLangSimpleTests
	{
		[TestMethod]
		public void Меди()
		{
			Assert.AreEqual("меди", Sklonenia.NounToPadej(EPadej.ROD, "медь", false, ESex.FEMALE));
		}

		[TestMethod]
		public void Крысы()
		{
			Assert.AreEqual("крысы", Sklonenia.NounToPadej(EPadej.ROD, "крыса", false, ESex.FEMALE));
		}
		[TestMethod]
		public void Табуретки()
		{
			Assert.AreEqual("табуретки", Sklonenia.NounToPadej(EPadej.ROD, "табуретка", false, ESex.FEMALE));
		}
		[TestMethod]
		public void Табуреткой()
		{
			Assert.AreEqual("табуреткой", Sklonenia.NounToPadej(EPadej.TVOR, "табуретка", false, ESex.FEMALE));
		}
		[TestMethod]
		public void Спесью()
		{
			Assert.AreEqual("спесью", Sklonenia.NounToPadej(EPadej.TVOR, "спесь", false, ESex.FEMALE));
		}
		[TestMethod]
		public void Волка()
		{
			Assert.AreEqual("волка", Sklonenia.NounToPadej(EPadej.ROD, "волк", false, ESex.MALE));
		}

		[TestMethod]
		public void Кольца()
		{
			Assert.AreEqual("кольца", Sklonenia.NounToPadej(EPadej.ROD, "кольцо", false, ESex.IT));
		}

		[TestMethod]
		public void КольцоВин()
		{
			Assert.AreEqual("кольцо", Sklonenia.NounToPadej(EPadej.VIN, "кольцо", false, ESex.IT));
		}

		[TestMethod]
		public void Холодцом()
		{
			Assert.AreEqual("Холодцом", Sklonenia.NounToPadej(EPadej.TVOR, "Холодец", false, ESex.MALE));
		}

		[TestMethod]
		public void Камнем()
		{
			Assert.AreEqual("камнем", Sklonenia.NounToPadej(EPadej.TVOR, "камень", false, ESex.MALE));
		}
		
		protected static IEnumerable<string> SklonTest(string _noun, ESex _sex, bool _isCreature = false)
		{
			if (string.IsNullOrEmpty(_noun)) yield break;
			foreach (EPadej padej in Enum.GetValues(typeof(EPadej)))
			{
				yield return Sklonenia.NounToPadej(padej, _noun, _isCreature, _sex);
			}
		}
	}

	[TestClass]
	public class RusLangTests : RusLangSimpleTests
	{
		static RusLangTests()
		{
			Constants.WORLD_MAP_SIZE = 1;
			Constants.WORLD_SEED = 1;

			MagickSetting.MagicSettingProvider.Init();

			if (World.TheWorld == null)
			{
				World.LetItBeeee();
			}
		}

		[TestMethod]
		public void Материалы()
		{
			var materails = EssenceHelper.AllEssences.Select(e => e.Material).Distinct().ToArray();
			var strings = new List<string>();
			foreach (var material in materails)
			{
				strings.Clear();
				strings.Add(material.GetType().Name);
				if (string.IsNullOrEmpty(material.Name)) continue;
				try
				{
					foreach (EPadej padej in Enum.GetValues(typeof(EPadej)))
					{
						strings.Add(Sklonenia.NounToPadej(padej, material.Name, false, material.Sex));
					}
				}
				catch (Exception ex)
				{
					strings.Add(ex.Message);
				}
				Debug.WriteLine(string.Join(", ", strings));
			}
		}

		[TestMethod]
		public void Предметы()
		{
			var items = EssenceHelper.GetAllItems<Item>().Select(e=>e.Essence).Where(e=>!(e is StackOfItems)).GroupBy(e=>e.Name).Select(g=>g.First()).ToArray();
			var strings = new List<string>();
			foreach (var item in items)
			{
				strings.Clear();
				strings.Add(item.GetType().Name);
				try
				{
					strings.AddRange(SklonTest(item.Name, item.Sex));
				}
				catch (Exception ex)
				{
					strings.Add(ex.Message);
				}
				Debug.WriteLine(string.Join(", ", strings));
			}
		}


		[TestMethod]
		public void Существа()
		{
			var items = EssenceHelper.GetAllCreatures<Creature>().Select(_e => _e.Essence).Where(_e => !(_e is AbstractDummyCreature)).GroupBy(e => e.Name).Select(_g => _g.First()).ToArray();
			var strings = new List<string>();
			foreach (var item in items)
			{
				strings.Clear();
				strings.Add(item.GetType().Name);
				try
				{
					strings.AddRange(SklonTest(item.Name, item.Sex, true));
				}
				catch (Exception ex)
				{
					strings.Add(ex.Message);
				}
				Debug.WriteLine(string.Join(", ", strings));
			}
		}
	}
}
