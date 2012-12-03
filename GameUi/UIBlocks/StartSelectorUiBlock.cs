using System;
using GameCore;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	class StartSelectorUiBlock: UiBlockWithText
	{
		private readonly TheGame m_game;

		public StartSelectorUiBlock(Rct _rct, TheGame _game)
			: base(_rct, Frame.Frame3, FColor.DarkGray)
		{
			m_game = _game;
		}


		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			switch (_key)
			{
				case ConsoleKey.A:
					Constants.WORLD_MAP_SIZE = 32;
					Constants.WORLD_SEED = 2;
					m_game.Run();
					break;
				case ConsoleKey.B:
					Constants.WORLD_MAP_SIZE = 100;
					Constants.WORLD_SEED = new Random().Next(10000);
					m_game.Run();
					break;
				case ConsoleKey.C:
					Constants.WORLD_MAP_SIZE = 1;
					Constants.WORLD_SEED = 1;
					m_game.Run();
					break;
				case ConsoleKey.D:
					Constants.WORLD_MAP_SIZE = 1;
					Constants.WORLD_SEED = 2;
					m_game.Run();
					break;
				case ConsoleKey.E:
					Constants.WORLD_MAP_SIZE = 1;
					Constants.WORLD_SEED = 0;
					m_game.Run();
					break;
			}
		}

		public override void DrawContent()
		{
			const int indent = 20;
			var line = 1;
			DrawLine("ВЫБОР РЕЖИМА", FColor.Yellow, line++, indent, EAlignment.CENTER);
			line += 2;
			DrawLine("a. Карта 1024х1024, полный режим, фиксированный random seed.", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("b. Карта 3200х3200, полный режим.", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("c. 1х1 тест объектов.", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("d. 1х1 тест боя.", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("e. 1х1 тест освещения.", ForeColor, line++, indent, EAlignment.LEFT);
		}
	}
}
