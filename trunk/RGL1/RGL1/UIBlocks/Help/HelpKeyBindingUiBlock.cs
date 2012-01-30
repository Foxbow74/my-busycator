using System;
using System.Linq;
using GameCore;
using GameCore.Acts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.Help
{
	internal class HelpKeyBindingUiBlock : UIBlock
	{
		public HelpKeyBindingUiBlock(Rectangle _rectangle) : base(_rectangle, Frame.SimpleFrame, Color.DarkGray, Fonts.Font)
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

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();
			var line = 0;
			var acts = KeyTranslator.RegisteredActs.OrderBy(_act => _act.Category).ThenBy(_act => _act.Name);
			var currentCategory = string.Empty;
			foreach (var act in acts)
			{
				var category = ActionCategoryAttribute.GetAttribute(act.Category).DisplayName;
				if(category!=currentCategory)
				{
					currentCategory = category;
					line++;
					DrawLine(currentCategory, Color.Yellow, _spriteBatch, line++, 20, EAlignment.LEFT);
				}
				var ind = DrawLine("[", Color, _spriteBatch, line, 30, EAlignment.LEFT) - Tile.Size;
				ind = DrawLine(act.HelpKeys, Color.LightBlue, _spriteBatch, line, ind, EAlignment.LEFT) - Tile.Size + 2;
				ind = DrawLine("]", Color, _spriteBatch, line, ind, EAlignment.LEFT) - Tile.Size;
				DrawLine(act.Name, Color, _spriteBatch, line++, ind + 10, EAlignment.LEFT);
			}
			DrawLine("[z|Esc] - выход", Color, _spriteBatch, TextLinesMax - 2, 20, EAlignment.RIGHT);
			_spriteBatch.End();
		}
	}
}