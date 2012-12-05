using GameCore.AbstractLanguage;

namespace LanguagePack
{
	public partial class RusLanguageProcessor
	{

		private static void FillConstants()
		{
			Consts.Add(EALConst.EXIT, "выход");
			Consts.Add(EALConst.PLEASE_CHOOSE_DIRECTION, "Выбери направление:");

			Consts.Add(EALConst.AN_HELP, "показать экран помощи");
			Consts.Add(EALConst.AN_INTERRACT_WITH_ESSENCE, "взаимодействовать с объектом");
			Consts.Add(EALConst.AN_TACTIC_NORMAL, "соблюдать разумный баланс атаки и защиты");
			Consts.Add(EALConst.AN_TACTIC_COWARD, "приоритет в бою - защита");
			Consts.Add(EALConst.AN_TACTIC_BERSERK, "приоритет в бою - атака");
			Consts.Add(EALConst.AN_DESCEND, "спуститься по лестнице");
			Consts.Add(EALConst.AN_WORLD_MAP, "посмотреть карту мира");
			Consts.Add(EALConst.AN_OPEN, "открыть сундук/дверь");
			Consts.Add(EALConst.AN_WAIT, "ждать");
			Consts.Add(EALConst.AN_SHOOT, "выстрелить/метнуть");
			Consts.Add(EALConst.AN_QUIT, "выйти из игры");
			Consts.Add(EALConst.AN_INVENTORY, "показать инвентарь");
			Consts.Add(EALConst.AN_USE, "задействовать инструмент");
			Consts.Add(EALConst.AN_MOVE_TO, "движение к точке");
			Consts.Add(EALConst.AN_ASCEND, "подняться по лестнице");
			Consts.Add(EALConst.AN_LOOK_AT, "осмотреться");
			Consts.Add(EALConst.AN_ATACK, "атаковать");
			Consts.Add(EALConst.AN_LEAVE_BUILDING, "покинуть помещение");
			Consts.Add(EALConst.AN_CLOSE, "закрыть сундук/дверь");
			Consts.Add(EALConst.AN_TAKE, "подобрать предмет");
			Consts.Add(EALConst.AN_DROP, "выбросить предмет");
			Consts.Add(EALConst.AN_DRINK, "выпить");
			Consts.Add(EALConst.AN_MOVE, "движение (стороны света)");
			Consts.Add(EALConst.AN_MOVE_ARROWS, "стрелки");
			//Consts.Add(EALConst., );
			//Consts.Add(EALConst., );
			//Consts.Add(EALConst., );
			//Consts.Add(EALConst., );
		}
	}
}