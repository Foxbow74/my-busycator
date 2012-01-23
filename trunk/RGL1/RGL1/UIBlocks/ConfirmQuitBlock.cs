using System;
using Common.Messages;
using GameCore;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks
{
	class ConfirmQuitBlock:UIBlock
	{
		public ConfirmQuitBlock() : base(new Rectangle(2,2,10,5), Frame.SimpleFrame, Color.Black)
		{
			ContentRectangle = new Rectangle(ContentRectangle.Left + 1, ContentRectangle.Top, ContentRectangle.Width - 1*2, ContentRectangle.Height);
		}

		public override void Draw(GameTime _gameTime, SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();
			this.Clear(_spriteBatch, Color.DimGray);
			base.Draw(_gameTime, _spriteBatch);
			DrawText(new TextPortion("Уверен?"), _spriteBatch, Tile.Font, Color.White, Tile.Size * 2);
			_spriteBatch.End();
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if(_modifiers!=EKeyModifiers.NONE) return;
			if (_key == ConsoleKey.Y || _key == ConsoleKey.L)
			{
				MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.EXIT_GAME));
			}
			if (_key == ConsoleKey.N || _key == ConsoleKey.Y || _key == ConsoleKey.Escape)
			{
				MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK));
			}
		}
	}
}
