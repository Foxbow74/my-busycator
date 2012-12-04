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
			var noun = "�����".AsNoun(ESex.MALE, true);
			noun = noun + "���������".AsTitle(ESex.MALE, true);
			noun = noun + "�����".AsAdv();
		}

		[TestMethod]
		public void NounToString()
		{
			//Parallel.For(1, 200, _l => Debug.WriteLine(_l));

			var noun = "�����".AsNoun(ESex.MALE, true);
			Assert.AreEqual("�����", noun.To(EPadej.IMEN));

			noun = noun + "�����".AsAdv();
			Assert.AreEqual("����� �����", noun.To(EPadej.IMEN));

			noun = noun + "���������".AsTitle(ESex.MALE, true);
			Assert.AreEqual("����� �����-���������", noun.To(EPadej.IMEN));
		}

		[TestMethod]
		public void Sklon()
		{
			var noun = "�����".AsNoun(ESex.MALE, true);
			Assert.AreEqual("������", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdv();
			Assert.AreEqual("������ ������", noun.To(EPadej.DAT));

			noun = noun + "���������".AsTitle(ESex.MALE, true);
			Assert.AreEqual("������ ������-���������", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonFemale()
		{
			var noun = "����".AsNoun(ESex.FEMALE, true);
			Assert.AreEqual("����", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdv();
			Assert.AreEqual("����� ����", noun.To(EPadej.DAT));

			noun = noun + "���������".AsTitle(ESex.MALE, true);
			Assert.AreEqual("����� ����-���������", noun.To(EPadej.DAT));
		}

		[TestMethod]
		public void SklonIt()
		{
			var noun = "����".AsNoun(ESex.IT, true);
			Assert.AreEqual("����", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdv();
			Assert.AreEqual("������ ����", noun.To(EPadej.DAT));

			noun = noun + "���������".AsTitle(ESex.MALE, true);
			Assert.AreEqual("������ ����-���������", noun.To(EPadej.DAT));
		}
		
		[TestMethod]
		public void SklonPlural()
		{
			var noun = "������".AsNoun(ESex.PLURAL, true);
			Assert.AreEqual("�������", noun.To(EPadej.DAT));

			noun = noun + "�����".AsAdv();
			Assert.AreEqual("����� �������", noun.To(EPadej.DAT));

			noun = noun + "���������".AsTitle(ESex.PLURAL, true);
			Assert.AreEqual("����� �������-����������", noun.To(EPadej.DAT));
		}
	}
}
