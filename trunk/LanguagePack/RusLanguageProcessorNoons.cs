using GameCore;
using GameCore.AbstractLanguage;

namespace LanguagePack
{
	public partial class RusLanguageProcessor
	{

		private static void FillNouns()
		{
			m_nouns.Add(EALNouns.StackOf, "кучка".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Grave, "могила".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Chair, "стул".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Cabinet, "шкаф".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.ArmorRack, "стойка".AsNoun(ESex.FEMALE, false) + "для брони".AsIm());
			m_nouns.Add(EALNouns.WeaponRack, "стойка".AsNoun(ESex.FEMALE, false) + "для оружия".AsIm());
			m_nouns.Add(EALNouns.Barrel, "бочка".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Stair, "лестница".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.StairUp, "лестница".AsNoun(ESex.FEMALE, false) + "вверх".AsIm());
			m_nouns.Add(EALNouns.StairDown, "лестница".AsNoun(ESex.FEMALE, false) + "вниз".AsIm());
			m_nouns.Add(EALNouns.Table, "стол".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Bed, "кровать".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Door, "дверь".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.ClosedDoor, "дверь".AsNoun(ESex.FEMALE, false) + "закрытый".AsAdj());
			m_nouns.Add(EALNouns.BackPack, "рюкзак".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Chest, "сундук".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Button, "кнопка".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Lever, "рычаг".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Ring, "кольцо".AsNoun(ESex.IT, false));
			m_nouns.Add(EALNouns.Potion, "зелье".AsNoun(ESex.IT, false));
			m_nouns.Add(EALNouns.Corpse, "труп".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.IndoorLight, "светильник".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Torch, "факел".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Shrub, "куст".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Mushrum, "гриб".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.MagicPlate, "пластина".AsNoun(ESex.FEMALE, false));

			m_nouns.Add(EALNouns.Avatar, "Аватар".AsNoun(ESex.MALE, true) + "Герой".AsNoun(ESex.MALE, true) + new Adjective("отважный"));

			m_nouns.Add(EALNouns.Ctitzen, "горожанин".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.NONE, "".AsNoun(ESex.IT, false));
			m_nouns.Add(EALNouns.Tree, "дерево".AsNoun(ESex.IT, false));
			m_nouns.Add(EALNouns.Sign, "знак".AsNoun(ESex.MALE, false));


			m_nouns.Add(EALNouns.Maple, "клен".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Willow, "ива".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Walnut, "орех".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Spruce, "ель".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Pine, "сосна".AsNoun(ESex.FEMALE, false));
			m_nouns.Add(EALNouns.Oak, "дуб".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Ash, "ясень".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Mushrum0, "гриб0".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Shrub0, "куст0".AsNoun(ESex.MALE, false));

			m_nouns.Add(EALNouns.Crossbow, "арбалет".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Jaws, "зубы".AsNoun(ESex.PLURAL, false));
			m_nouns.Add(EALNouns.Bolt, "болт".AsNoun(ESex.MALE, false));
			m_nouns.Add(EALNouns.Sword, "меч".AsNoun(ESex.MALE, false));

			m_nouns.Add(EALNouns.Rat, "крыса".AsNoun(ESex.FEMALE, true) + "грязный".AsAdj() + "отвратительный".AsAdj());
			m_nouns.Add(EALNouns.Spider, "паук".AsNoun(ESex.MALE, true) + "огромный".AsAdj() + "страшный".AsAdj());
			m_nouns.Add(EALNouns.Wolf, "волк".AsNoun(ESex.MALE, true) + "лохматый".AsAdj() + "свирепый".AsAdj());

			m_nouns.Add(EALNouns.YOU, "ты".AsNoun(ESex.MALE, true));
		}
	}
}