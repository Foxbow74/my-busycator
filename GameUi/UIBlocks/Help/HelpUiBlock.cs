using System;
using System.Drawing;
using GameCore;
using GameCore.Messages;

namespace GameUi.UIBlocks.Help
{
	internal class HelpUiBlock : UiBlockWithText
	{
		public HelpUiBlock(Rectangle _rectangle)
			: base(_rectangle, Frame.SimpleFrame, Color.White, EFonts.COMMON)
		{
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			switch (_key)
			{
				case ConsoleKey.A:
					MessageManager.SendMessage(this, new OpenUIBlockMessage(new HelpKeyBindingUiBlock(Rectangle)));
					break;
				case ConsoleKey.Z:
				case ConsoleKey.Escape:
					CloseTopBlock();
					break;
			}
		}

		public override void DrawContent()
		{
			const int indent = 20;
			var line = 1;
			DrawLine("a. Привязка клавишь", Color, line++, indent, EAlignment.LEFT);
			DrawLine("b. Предметы", Color, line++, indent, EAlignment.LEFT);
			DrawLine("с. Классы", Color, line++, indent, EAlignment.LEFT);
			DrawLine("d. Рассы", Color, line++, indent, EAlignment.LEFT);
			DrawLine("[z|Esc] - выход", Color, TextLinesMax - 2, indent, EAlignment.RIGHT);
		}
	}
}