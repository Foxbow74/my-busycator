using System;
using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Acts;

namespace GameUi.UIBlocks.Help
{
	internal class HelpKeyBindingUiBlock : UIBlock
	{
		public HelpKeyBindingUiBlock(Rectangle _rectangle)
			: base(_rectangle, Frame.SimpleFrame, Color.DarkGray, EFonts.COMMON)
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
					DrawLine(currentCategory, Color.Yellow, line++, 20, EAlignment.LEFT);
				}
				var ind = DrawLine("[", Color, line, 30, EAlignment.LEFT) - ATile.Size;
				ind = DrawLine(act.HelpKeys, Color.LightBlue, line, ind, EAlignment.LEFT) - ATile.Size + 2;
				ind = DrawLine("]", Color, line, ind, EAlignment.LEFT) - ATile.Size;
				DrawLine(act.Name, Color, line++, ind + 10, EAlignment.LEFT);
			}
			DrawLine("[z|Esc] - выход", Color, TextLinesMax - 2, 20, EAlignment.RIGHT);
		}
	}
}