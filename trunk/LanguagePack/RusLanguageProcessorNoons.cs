using GameCore;
using GameCore.AbstractLanguage;

namespace LanguagePack
{
	public partial class RusLanguageProcessor
	{

		private static void FillNouns()
		{
			m_nouns.Add(EALNouns.StackOf, "�����".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Grave, "������".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Chair, "����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Cabinet, "����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.ArmorRack, "������".AsNoun(ESex.FEMALE, false) + "��� �����".AsIm());
			m_nouns.Add(EALNouns.WeaponRack, "������".AsNoun(ESex.FEMALE, false) + "��� ������".AsIm());
			m_nouns.Add(EALNouns.Barrel, "�����".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Stair, "��������".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.StairUp, "��������".AsNoun(ESex.FEMALE, false) + "�����".AsIm());
			m_nouns.Add(EALNouns.StairDown, "��������".AsNoun(ESex.FEMALE, false) + "����".AsIm());
			m_nouns.Add(EALNouns.Table, "����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Bed, "�������".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Door, "�����".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.ClosedDoor, "�����".AsNoun(ESex.FEMALE, false) + "��������".AsAdj());
			m_nouns.Add(EALNouns.BackPack, "������".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Chest, "������".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Button, "������".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Lever, "�����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Ring, "������".AsNoun(ESex.IT, false));
			m_nouns.Add(EALNouns.Potion, "�����".AsNoun(ESex.IT, false));
			m_nouns.Add(EALNouns.Corpse, "����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.IndoorLight, "����������".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Torch, "�����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Shrub, "����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Mushrum, "����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.MagicPlate, "��������".AsNoun(ESex.FEMALE, false));

			m_nouns.Add(EALNouns.Avatar, "������".AsNoun(ESex.MALE, true) + "�����".AsNoun(ESex.MALE, true) + new Adjective("��������"));

			m_nouns.Add(EALNouns.Ctitzen, "���������".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.NONE, "".AsNoun(ESex.IT, false));
			m_nouns.Add(EALNouns.Tree, "������".AsNoun(ESex.IT, false));
			m_nouns.Add(EALNouns.Sign, "����".AsNoun(ESex.MALE, false));


			m_nouns.Add(EALNouns.Maple, "����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Willow, "���".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Walnut, "����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Spruce, "���".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Pine, "�����".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Oak, "���".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Ash, "�����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Mushrum0, "����0".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Shrub0, "����0".AsNoun(ESex.MALE, false));

			m_nouns.Add(EALNouns.Crossbow, "�������".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Jaws, "����".AsNoun(ESex.PLURAL, false));
			m_nouns.Add(EALNouns.Bolt, "����".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Sword, "���".AsNoun(ESex.MALE, false));

			m_nouns.Add(EALNouns.Rat, "�����".AsNoun(ESex.FEMALE, true) + "�������".AsAdj() + "��������������".AsAdj());
			m_nouns.Add(EALNouns.Spider, "����".AsNoun(ESex.MALE, true) + "��������".AsAdj() + "��������".AsAdj());
			m_nouns.Add(EALNouns.Wolf, "����".AsNoun(ESex.MALE, true) + "��������".AsAdj() + "��������".AsAdj());

			m_nouns.Add(EALNouns.YOU, "��".AsNoun(ESex.MALE, true));
		}
	}
}