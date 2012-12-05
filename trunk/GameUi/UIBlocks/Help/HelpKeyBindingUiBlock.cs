using System;
using System.Linq;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Misc;

namespace GameUi.UIBlocks.Help
{
	internal class HelpKeyBindingUiBlock : UiBlockWithText
	{
		public HelpKeyBindingUiBlock(Rct _rct)
			: base(_rct, Frame.Frame1, FColor.DarkGray) { }

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			switch (_key)
			{
				case ConsoleKey.Z:
				case ConsoleKey.Escape:
					CloseTopBlock();
					break;
			}
		}

		public override void DrawContent()
		{
			var line = 0;
			var acts = KeyTranslator.RegisteredActs.OrderBy(_act => _act.Category).ThenBy(_act => _act.Name);
			var currentCategory = string.Empty;
			foreach (var act in acts)
			{
				var category = EALSentence.NONE.GetString(act.Category.AsNoun());
				if (category != currentCategory)
				{
					currentCategory = category;
					line++;
					DrawLine(currentCategory, FColor.Yellow, line++, 20, EAlignment.LEFT);
				}
				DrawLine(act.HelpKeys, FColor.LightBlue, line, 30, EAlignment.LEFT);
				DrawLine(act.Name.GetString(), ForeColor, line++, 100, EAlignment.LEFT);
			}
			DrawLine("[z|Esc] - " + EALConst.EXIT.GetString(), ForeColor, TextLinesMax - 2, 20, EAlignment.RIGHT);
		}
	}
}