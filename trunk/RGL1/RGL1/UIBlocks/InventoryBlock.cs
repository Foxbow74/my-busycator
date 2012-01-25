#region

using System;
using System.Collections.Generic;
using GameCore;
using GameCore.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace RGL1.UIBlocks
{
	internal class InventoryBlock : UIBlock
	{
		private readonly World m_world;

		public InventoryBlock(World _world, Rectangle _rectangle) : base(_rectangle, Frame.GoldFrame, Color.Green)
		{
			m_world = _world;
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();

			var inventory = m_world.Avatar.Inventory;

			DrawLine("СНАРЯЖЕНИЕ", Color, _spriteBatch, 0, 0, EAlignment.CENTER);
			DrawLine("ВЕС:", Color, _spriteBatch, 2, 0, EAlignment.LEFT);
			DrawLine("ДОСТУПНЫЙ ВЕС:", Color, _spriteBatch, 2, 0, EAlignment.RIGHT);

			var c = 'A';
			for (var index = 0; index < inventory.Items.Count; index++)
			{
				var item = inventory.Items[index];
				var s = new string((char) (c + index), 1);
				DrawLine(new TextPortion.TextLine("[" + s + "]", 0, new Dictionary<string, Color> {{s, Color.White}}), Color,
				         _spriteBatch, 4 + index, 10, EAlignment.LEFT);
				DrawLine("-", Color, _spriteBatch, 4 + index, 40, EAlignment.LEFT);
				DrawLine(item.Name, Color.SkyBlue, _spriteBatch, 4 + index, 70, EAlignment.LEFT);
			}

			_spriteBatch.End();
		}

		public override void DrawFrame(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();
			base.DrawFrame(_spriteBatch);
			_spriteBatch.End();
		}


		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if (_modifiers != EKeyModifiers.NONE) return;
			if (_key == ConsoleKey.Escape)
			{
				MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK));
			}
		}
	}
}