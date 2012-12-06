using GameCore.AbstractLanguage;

namespace LanguagePack
{
	public partial class RusLanguageProcessor
	{
		private static void FillVerbs()
		{
			m_verbs.Add(EALVerbs.HURT, new Verb("ранит", "ранил"));
			m_verbs.Add(EALVerbs.MANGLE, new Verb("калечит", "покалечил") + new Verb("кромсает", "искромсал") + new Verb("раздирает", "разодрал"));
			m_verbs.Add(EALVerbs.HACK, new Verb("рубит", "разрубил"));
			m_verbs.Add(EALVerbs.MISS, new Verb("промахивается", "промахнулся"));
			m_verbs.Add(EALVerbs.HIT, new Verb("попадает", "попал"));
			m_verbs.Add(EALVerbs.DONT_HIT, new Verb("не попадает", "не попал"));
			m_verbs.Add(EALVerbs.FINISH, new Verb("добивает", "добил") + new Verb("приканчивает", "прикончил"));
			m_verbs.Add(EALVerbs.STIKE, new Verb("бьет", "ударил"));
			m_verbs.Add(EALVerbs.AMMO_WEAPON_VERB, m_verbs[EALVerbs.DONT_HIT]);
			m_verbs.Add(EALVerbs.AXE_WEAPON_VERB, new Verb("разрубает", "разрубил"));
			m_verbs.Add(EALVerbs.CLUB_WEAPON_VERB, m_verbs[EALVerbs.STIKE]);
			m_verbs.Add(EALVerbs.JAWS_WEAPON_VERB, new Verb("кусает", "укуcил"));
			m_verbs.Add(EALVerbs.SWORD_WEAPON_VERB, m_verbs[EALVerbs.HACK]);
			//m_verbs.Add(EALVerbs., new Verb("", ""));
			//m_verbs.Add(EALVerbs., new Verb("", ""));


		}
	}
}