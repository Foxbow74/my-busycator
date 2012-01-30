using System;
using GameCore;
using GameCore.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.Help
{
	class HelpUiBlock:UIBlock
	{
		public HelpUiBlock(Rectangle _rectangle) : base(_rectangle, Frame.SimpleFrame, Color.White, Fonts.Font)
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

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();
			const int indent = 20;
			var line = 1;
			DrawLine("a. Привязка клавишь", Color, _spriteBatch, line++, indent, EAlignment.LEFT);
			DrawLine("b. Предметы", Color, _spriteBatch, line++, indent, EAlignment.LEFT);
			DrawLine("с. Классы", Color, _spriteBatch, line++, indent, EAlignment.LEFT);
			DrawLine("d. Рассы", Color, _spriteBatch, line++, indent, EAlignment.LEFT);
			DrawLine("[z|Esc] - выход", Color, _spriteBatch, TextLinesMax - 2, indent, EAlignment.RIGHT);
			_spriteBatch.End();
		}
	}
}
