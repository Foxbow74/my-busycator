#region

using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace RGL1.UIBlocks
{
	internal class InventoryBlock : UIBlock
	{
		public InventoryBlock(Rectangle _rectangle) : base(_rectangle, Frame.GoldFrame, Color.Green)
		{
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();

			var descriptors = World.TheWorld.Avatar.GetBackPackItems().ToArray();

			DrawLine("СНАРЯЖЕНИЕ", Color, _spriteBatch, 0, 0, EAlignment.CENTER);
			DrawLine("ВЕС:", Color, _spriteBatch, 2, 0, EAlignment.LEFT);
			DrawLine("ДОСТУПНЫЙ ВЕС:", Color, _spriteBatch, 2, 0, EAlignment.RIGHT);

			var c = 'A';
			for (var index = 0; index < descriptors.Length; index++)
			{
				var item = descriptors[index];
				var s = new string((char) (c + index), 1);
				DrawLine(new TextPortion.TextLine("[" + s + "]", 0, new Dictionary<string, Color> {{s, Color.White}}), Color,
				         _spriteBatch, 4 + index, 10, EAlignment.LEFT);
				DrawLine("-", Color, _spriteBatch, 4 + index, 40, EAlignment.LEFT);
				DrawLine(item.Thing.Name, Color.SkyBlue, _spriteBatch, 4 + index, 70, EAlignment.LEFT);
			}

			_spriteBatch.End();
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if (_modifiers != EKeyModifiers.NONE) return;
			if (_key == ConsoleKey.Escape)
			{
				CloseTopBlock();
			}
		}
	}
}