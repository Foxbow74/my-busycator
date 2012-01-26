#region

using System;
using GameCore;
using GameCore.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.UIBlocks.Map;

#endregion

namespace RGL1.UIBlocks
{
	internal class MainBlock : UIBlock
	{
		private readonly GraphicsDevice m_device;

		private readonly UIBlock m_map;
		private readonly UIBlock m_messages;
		private readonly UIBlock m_stats;

		public MainBlock(GraphicsDevice _device)
			: base(new Rectangle(0, 0, _device.Viewport.Width/Tile.Size, _device.Viewport.Height/Tile.Size), null, Color.White)
		{
			m_device = _device;
			var width = ContentRectangle.Width;
			var height = ContentRectangle.Height;

			const int messagesHeight = 3;
			const int statHeight = 2;

			m_messages = new MessageBlock(new Rectangle(Rectangle.Left, 0, Rectangle.Width, messagesHeight));
			m_stats = new StatsBlock(new Rectangle(0, Rectangle.Bottom - statHeight, Rectangle.Width, statHeight));

			m_map =
				new MapBlock(new Rectangle(ContentRectangle.Left, m_messages.Rectangle.Height, Rectangle.Width,
				                           Rectangle.Height - m_messages.Rectangle.Height - m_stats.Rectangle.Height));
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if (_key == ConsoleKey.Q && _modifiers == EKeyModifiers.CTRL)
			{
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new ConfirmQuitBlock()));
				return;
			}

			if (_key == ConsoleKey.I)
			{
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new InventoryBlock(Rectangle)));
				return;
			}

			var dx = (_key == ConsoleKey.LeftArrow ? -1 : 0) + (_key == ConsoleKey.RightArrow ? 1 : 0);
			var dy = (_key == ConsoleKey.UpArrow ? -1 : 0) + (_key == ConsoleKey.DownArrow ? 1 : 0);

			dx += (_key == ConsoleKey.NumPad4 ? -1 : 0) + (_key == ConsoleKey.NumPad6 ? 1 : 0);

			dx += (_key == ConsoleKey.NumPad7 ? -1 : 0) + (_key == ConsoleKey.NumPad9 ? 1 : 0);
			dx += (_key == ConsoleKey.NumPad1 ? -1 : 0) + (_key == ConsoleKey.NumPad3 ? 1 : 0);

			dx += (_key == ConsoleKey.Home ? -1 : 0) + (_key == ConsoleKey.PageUp ? 1 : 0);
			dx += (_key == ConsoleKey.End ? -1 : 0) + (_key == ConsoleKey.PageDown ? 1 : 0);

			dy += (_key == ConsoleKey.NumPad8 ? -1 : 0) + (_key == ConsoleKey.NumPad2 ? 1 : 0);

			dy += (_key == ConsoleKey.NumPad7 ? -1 : 0) + (_key == ConsoleKey.NumPad1 ? 1 : 0);
			dy += (_key == ConsoleKey.NumPad9 ? -1 : 0) + (_key == ConsoleKey.NumPad3 ? 1 : 0);

			dy += (_key == ConsoleKey.Home ? -1 : 0) + (_key == ConsoleKey.End ? 1 : 0);
			dy += (_key == ConsoleKey.PageUp ? -1 : 0) + (_key == ConsoleKey.PageDown ? 1 : 0);

			if (dx != 0 || dy != 0)
			{
				World.TheWorld.Avatar.MoveCommandReceived(dx, dy);
				return;
			}
			var command = KeyTranslator.TranslateKey(_key, _modifiers);
			if (command != ECommands.NONE)
			{
				World.TheWorld.Avatar.CommandReceived(command);
				return;
			}
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			m_map.DrawContent(_spriteBatch);
			m_messages.DrawContent(_spriteBatch);
		}

		public override void DrawBackground(SpriteBatch _spriteBatch)
		{
			Frame.SimpleFrame.Draw(_spriteBatch, 0, 0, Rectangle.Width, Rectangle.Height);
			m_map.Clear(_spriteBatch, new Color(10, 5, 0));
			m_messages.Clear(_spriteBatch, new Color(30, 30, 30));
			m_stats.Clear(_spriteBatch, new Color(0, 30, 30));
			base.DrawBackground(_spriteBatch);
		}

		public override void DrawFrame(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			m_stats.DrawFrame(_spriteBatch);
			m_messages.DrawFrame(_spriteBatch);

			_spriteBatch.End();
			base.DrawFrame(_spriteBatch);
		}
	}
}