using System;
using System.Drawing;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;

namespace GameUi.UIBlocks.Help
{
	internal class HelpUiBlock : UiBlockWithText
	{
		public HelpUiBlock(Rectangle _rectangle)
			: base(_rectangle, Frame.SimpleFrame, Color.White.ToFColor())
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
			DrawLine("a. Привязка клавишь", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("b. Предметы", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("с. Классы", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("d. Рассы", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("[z|Esc] - выход", ForeColor, TextLinesMax - 2, indent, EAlignment.RIGHT);
		}
	}
}