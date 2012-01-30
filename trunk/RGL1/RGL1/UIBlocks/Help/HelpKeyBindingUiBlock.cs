using System;
using System.Collections.Generic;
using GameCore;
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
			var line = 1;
			foreach (var act in KeyTranslator.RegisteredActs)
			{
				var ind = DrawLine("[", Color, _spriteBatch, line, 20, EAlignment.LEFT) - Tile.Size;
				ind = DrawLine(act.HelpKeys, Color.LightBlue, _spriteBatch, line, ind, EAlignment.LEFT) - Tile.Size + 2;
				ind = DrawLine("]", Color, _spriteBatch, line, ind, EAlignment.LEFT) - Tile.Size;
				DrawLine(act.Name, Color, _spriteBatch, line++, ind + 10, EAlignment.LEFT);
			}
			_spriteBatch.End();
		}
	}
}