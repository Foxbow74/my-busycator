using System;
using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Acts;
using GameCore.Misc;

namespace GameUi.UIBlocks.Help
{
	internal class HelpKeyBindingUiBlock : UiBlockWithText
	{
		public HelpKeyBindingUiBlock(Rectangle _rectangle)
			: base(_rectangle, Frame.SimpleFrame, Color.DarkGray.ToFColor())
		{
		}

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
				var category = ActionCategoryAttribute.GetAttribute(act.Category).DisplayName;
				if (category != currentCategory)
				{
					currentCategory = category;
					line++;
					DrawLine(currentCategory, Color.Yellow.ToFColor(), line++, 20, EAlignment.LEFT);
				}
				var ind = DrawLine("[", ForeColor, line, 30, EAlignment.LEFT) - ATile.Size;
				ind = DrawLine(act.HelpKeys, Color.LightBlue.ToFColor(), line, ind, EAlignment.LEFT) - ATile.Size + 2;
				ind = DrawLine("]", ForeColor, line, ind, EAlignment.LEFT) - ATile.Size;
				DrawLine(act.Name, ForeColor, line++, ind + 10, EAlignment.LEFT);
			}
			DrawLine("[z|Esc] - выход", ForeColor, TextLinesMax - 2, 20, EAlignment.RIGHT);
		}
	}
}