using GameCore.AbstractLanguage;

namespace LanguagePack
{
	public partial class RusLanguageProcessor
	{
		private static void FillVerbs()
		{
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
			//m_verbs.Add(EALVerbs., new Verb("", ""));
			//m_verbs.Add(EALVerbs., new Verb("", ""));


		}
	}
}