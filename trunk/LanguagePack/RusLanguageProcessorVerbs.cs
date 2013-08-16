using System.Collections.Generic;
using GameCore;
using GameCore.AbstractLanguage;

namespace LanguagePack
{
	public partial class RusLanguageProcessor
	{
		private static readonly Dictionary<ERLVerbs, Verb> m_rverbs = new Dictionary<ERLVerbs, Verb>();

		private static void FillVerbs()
		{
			m_verbs.Add(EALVerbs.THERE_IS, new Verb("", "����� ���"));
			m_verbs.Add(EALVerbs.HURT, new Verb("�����", "�����"));
			m_verbs.Add(EALVerbs.MANGLE, new Verb("�������", "���������") + new Verb("��������", "���������") + new Verb("���������", "��������"));
			m_verbs.Add(EALVerbs.HACK, new Verb("�����", "��������"));
			m_verbs.Add(EALVerbs.MISS, new Verb("�������������", "�����������"));
			m_verbs.Add(EALVerbs.HIT, new Verb("��������", "�����"));
			m_verbs.Add(EALVerbs.DONT_HIT, new Verb("�� ��������", "�� �����"));
			m_verbs.Add(EALVerbs.FINISH, new Verb("��������", "�����") + new Verb("������������", "���������"));
			m_verbs.Add(EALVerbs.STIKE, new Verb("����", "������"));
			m_verbs.Add(EALVerbs.AMMO_WEAPON_VERB, m_verbs[EALVerbs.DONT_HIT]);
			m_verbs.Add(EALVerbs.AXE_WEAPON_VERB, new Verb("���������", "��������"));
			m_verbs.Add(EALVerbs.CLUB_WEAPON_VERB, m_verbs[EALVerbs.STIKE]);
			m_verbs.Add(EALVerbs.JAWS_WEAPON_VERB, new Verb("������", "���c��"));
			m_verbs.Add(EALVerbs.SWORD_WEAPON_VERB, m_verbs[EALVerbs.HACK]);
			m_verbs.Add(EALVerbs.TAKES, new Verb("�����", "����") + new Verb("���������", "������") + new Verb("�������", "�������"));
			m_verbs.Add(EALVerbs.DROPS, new Verb("�����������", "��������"));
			m_verbs.Add(EALVerbs.CAN, new Verb("�����", "����") + new Verb("�����", "�����"));

			m_rverbs.Add(ERLVerbs.�����, new Verb("��������", "�����"));
		}


		public static string GetString(ERLVerbs _verb, EVerbType _type, params Noun[] _nouns)
		{
			return m_rverbs[_verb].To(_nouns.Length == 1 ? _nouns[0].Sex : ESex.PLURAL, _type);
		}

		public static string GetString(ERLVerbs _verb, EVerbType _type, ESex _sex)
		{
			return m_rverbs[_verb].To(_sex, _type);
		}
	}

	public enum ERLVerbs
	{
		�����,
	}

	public static class RUtil
	{

		public static string GetString(this ERLVerbs _verb, Noun _noun, EVerbType _type)
		{
			return RusLanguageProcessor.GetString(_verb, _type, _noun);
		}

		public static string GetString(this ERLVerbs _verb, ESex _sex, EVerbType _type)
		{
			return RusLanguageProcessor.GetString(_verb, _type, _sex);
		}
	}
}
