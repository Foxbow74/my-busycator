using System;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Messages;
using GameCore.Misc;

namespace GameUi.UIBlocks.Help
{
	internal class HelpUiBlock : UiBlockWithText
	{
		public HelpUiBlock(Rct _rct)
			: base(_rct, Frame.Frame1, FColor.White) { }

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			switch (_key)
			{
				case ConsoleKey.A:
					MessageManager.SendMessage(this, new OpenUIBlockMessage(new HelpKeyBindingUiBlock(Rct)));
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
			DrawLine("a. Привязка клавиш", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("b. Предметы", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("с. Классы", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("d. Рассы", ForeColor, line++, indent, EAlignment.LEFT);
			DrawLine("[z|Esc] - " + EALConst.EXIT.GetString(), ForeColor, TextLinesMax - 2, indent, EAlignment.RIGHT);
		}
	}
}