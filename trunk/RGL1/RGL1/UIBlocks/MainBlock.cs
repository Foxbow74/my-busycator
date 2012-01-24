using System;
using Common.Messages;
using GameCore;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks
{
	class MainBlock:UIBlock
	{
		public World World { get; private set; }
		private readonly GraphicsDevice m_device;

		private readonly UIBlock m_map;
		private readonly UIBlock m_messages;
		private readonly UIBlock m_stats;

		public MainBlock(GraphicsDevice _device, World _world)
			: base(new Rectangle(0, 0, _device.Viewport.Width / Tile.Size, _device.Viewport.Height / Tile.Size), null, Color.White)
		{
			World = _world;
			m_device = _device;
			var width = ContentRectangle.Width;
			var height = ContentRectangle.Height;

			const int messagesHeight = 10;
			const int statWidth = 15;

			m_stats = new StatsBlock(new Rectangle(width - statWidth, ContentRectangle.Top, statWidth, height - messagesHeight + 1));
			m_messages = new MessageBlock(new Rectangle(ContentRectangle.Left, height - messagesHeight, width, messagesHeight));

			m_map = new MapBlock(new Rectangle(ContentRectangle.Left, ContentRectangle.Top, m_stats.ContentRectangle.Left + 1, m_messages.ContentRectangle.Top + 1), _world);

		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if (_key == ConsoleKey.Q && _modifiers == EKeyModifiers.CTRL)
			{
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new ConfirmQuitBlock()));
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
				World.Avatar.MoveCommandReceived(dx, dy);
				//MessageManager.SendMessage(this, new TurnMessage());
				return;
			}
			var command = KeyTranslator.TranslateKey(_key, _modifiers);
			if (command != ECommands.NONE)
			{
				World.Avatar.CommandReceived(command);
				//MessageManager.SendMessage(this, new TurnMessage());
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
