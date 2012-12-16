using GameCore.AbstractLanguage;

namespace LanguagePack
{
	public partial class RusLanguageProcessor
	{

		private static void FillConstants()
		{
			m_consts.Add(EALConst.EXIT, "выход");
			m_consts.Add(EALConst.PLEASE_CHOOSE_DIRECTION, "Выбери направление:");

			m_consts.Add(EALConst.AN_HELP, "показать экран помощи");
			m_consts.Add(EALConst.AN_INTERRACT_WITH_ESSENCE, "взаимодействовать с объектом");
			m_consts.Add(EALConst.AN_TACTIC_NORMAL, "соблюдать разумный баланс атаки и защиты");
			m_consts.Add(EALConst.AN_TACTIC_PEACEFULL, "мирный режим, нельзя атаковать мирные существа");
			m_consts.Add(EALConst.AN_TACTIC_COWARD, "приоритет в бою - защита");
			m_consts.Add(EALConst.AN_TACTIC_BERSERK, "приоритет в бою - атака");
			m_consts.Add(EALConst.AN_DESCEND, "спуститься по лестнице");
			m_consts.Add(EALConst.AN_WORLD_MAP, "посмотреть карту мира");
			m_consts.Add(EALConst.AN_OPEN, "открыть сундук/дверь");
			m_consts.Add(EALConst.AN_WAIT, "ждать");
			m_consts.Add(EALConst.AN_SHOOT, "выстрелить/метнуть");
			m_consts.Add(EALConst.AN_QUIT, "выйти из игры");
			m_consts.Add(EALConst.AN_INVENTORY, "показать инвентарь");
			m_consts.Add(EALConst.AN_USE, "задействовать инструмент");
			m_consts.Add(EALConst.AN_MOVE_TO, "движение к точке");
			m_consts.Add(EALConst.AN_ASCEND, "подняться по лестнице");
			m_consts.Add(EALConst.AN_LOOK_AT, "осмотреться");
			m_consts.Add(EALConst.AN_ATACK, "атаковать");
			m_consts.Add(EALConst.AN_LEAVE_BUILDING, "покинуть помещение");
			m_consts.Add(EALConst.AN_CLOSE, "закрыть сундук/дверь");
			m_consts.Add(EALConst.AN_TAKE, "подобрать предмет");
			m_consts.Add(EALConst.AN_DROP, "выбросить предмет");
			m_consts.Add(EALConst.AN_DRINK, "выпить");
			m_consts.Add(EALConst.AN_MOVE, "движение (стороны света)");
			m_consts.Add(EALConst.AN_MOVE_ARROWS, "стрелки");
			//Consts.Add(EALConst., );
			//Consts.Add(EALConst., );
			//Consts.Add(EALConst., );
			//Consts.Add(EALConst., );
		}
	}
}