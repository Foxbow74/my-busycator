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
			var noun = "�����".AsNoun(ESex.MALE, true);
			noun = noun + "���������".AsNoun(ESex.MALE, true);
			noun = noun + "�����".AsAdj();
		}

		[TestMethod]
		public void NounToString()
		{
			//Parallel.For(1, 200, _l => Debug.WriteLine(_l));

			var noun = "�����".AsNoun(ESex.MALE, true);
			Assert.AreEqual("�����", noun.To(EPadej.IMEN));

			noun = noun + "�����".AsAdj();
			Assert.AreEqual("����� �����", noun.To(EPadej.IMEN));

			noun = noun + "���������".AsNoun(ESex.MALE, true);
			Assert.AreEqual("����� ���������", noun.To(EPadej.IMEN));
		}

		[TestMethod]
		public void Sklon()
		{
			var noun = "�����".AsNoun(ESex.MALE, true);
			Assert.AreEqual("������", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdj();
			Assert.AreEqual("������ ������", noun.To(EPadej.DAT));

			noun = noun + "���������".AsNoun(ESex.MALE, true);
			Assert.AreEqual("������ ���������", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonFemale()
		{
			var noun = "����".AsNoun(ESex.FEMALE, true);
			Assert.AreEqual("����", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdj();
			Assert.AreEqual("����� ����", noun.To(EPadej.DAT));

			noun = noun + "���������".AsNoun(ESex.MALE, true);
			Assert.AreEqual("����� ���������", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonIt()
		{
			var noun = "����".AsNoun(ESex.IT, true);
			Assert.AreEqual("����", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdj();
			Assert.AreEqual("������ ����", noun.To(EPadej.DAT));

			noun = noun + "���������".AsNoun(ESex.MALE, true);
			Assert.AreEqual("������ ���������", noun.To(EPadej.DAT));
		}
		
		[TestMethod]
		public void SklonPlural()
		{
			var noun = "������".AsNoun(ESex.PLURAL, true);
			Assert.AreEqual("�������", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdj();
			Assert.AreEqual("����� �������", noun.To(EPadej.DAT));

			noun = noun + "���������".AsNoun(ESex.PLURAL, true);
			Assert.AreEqual("����� ����������", noun.To(EPadej.DAT));
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

			var noun = "�����".AsNoun(ESex.MALE, true);
			Assert.AreEqual("�����", noun.To(EPadej.IMEN));

			noun = noun + "�����".AsAdj();
			Assert.AreEqual("����� �����", noun.To(EPadej.IMEN));

			noun = noun + "���������".AsNoun(ESex.MALE, true);
			Assert.AreEqual("����� �����", noun.To(EPadej.IMEN));
		}

		[TestMethod]
		public void Sklon()
		{
			var noun = "�����".AsNoun(ESex.MALE, true);
			Assert.AreEqual("������", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdj();
			Assert.AreEqual("������ ������", noun.To(EPadej.DAT));

			noun = noun + "���������".AsNoun(ESex.MALE, true);
			Assert.AreEqual("������ ������", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonFemale()
		{
			var noun = "����".AsNoun(ESex.FEMALE, true);
			Assert.AreEqual("����", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdj();
			Assert.AreEqual("����� ����", noun.To(EPadej.DAT));

			noun = noun + "���������".AsNoun(ESex.MALE, true);
			Assert.AreEqual("����� ����", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonIt()
		{
			var noun = "����".AsNoun(ESex.IT, true);
			Assert.AreEqual("����", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdj();
			Assert.AreEqual("������ ����", noun.To(EPadej.DAT));

			noun = noun + "���������".AsNoun(ESex.MALE, true);
			Assert.AreEqual("������ ����", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonPlural()
		{
			var noun = "������".AsNoun(ESex.PLURAL, true);
			Assert.AreEqual("�������", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdj();
			Assert.AreEqual("����� �������", noun.To(EPadej.DAT));

			noun = noun + "���������".AsNoun(ESex.PLURAL, true);
			Assert.AreEqual("����� �������", noun.To(EPadej.DAT));
		}
	}
}
