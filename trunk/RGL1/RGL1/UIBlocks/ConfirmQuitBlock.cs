using System;
using GameCore;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.Messages;

namespace RGL1.UIBlocks
{
	class ConfirmQuitBlock:UIBlock
	{
		public ConfirmQuitBlock() : base(new Rectangle(2,2,30,6), Frame.SimpleFrame, Color.Black)
		{
		}

		public override void Draw(GameTime _gameTime, SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();
			this.Fill(_spriteBatch, Tiles.SolidTile, Color.Black);
			base.Draw(_gameTime, _spriteBatch);
			DrawText(new TextPortion("Are you sure?"), _spriteBatch, Tile.Font, Color.Red);
			_spriteBatch.End();
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if(_modifiers!=EKeyModifiers.NONE) return;
			if (_key == ConsoleKey.Y)
			{
				MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.EXIT_GAME));
			}
			if (_key == ConsoleKey.N || _key==ConsoleKey.Escape)
			{
				MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK));
			}
		}
	}
}
