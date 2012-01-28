using System;
using GameCore;
using GameCore.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.UIBlocks.Map;

namespace RGL1.UIBlocks
{
	internal class MainBlock : UIBlock
	{
		private readonly UIBlock m_map;
		private readonly UIBlock m_messages;
		private readonly UIBlock m_stats;

		public MainBlock(GraphicsDevice _device)
			: base(new Rectangle(0, 0, _device.Viewport.Width/Tile.Size, _device.Viewport.Height/Tile.Size), null, Color.White)
		{

			const int messagesHeight = 3;
			const int statHeight = 2;

			m_messages = new MessageBlock(new Rectangle(Rectangle.Left, 0, Rectangle.Width, messagesHeight)) { BackgroundColor = new Color(30, 30, 30) };
			m_map = new MapBlock(new Rectangle(ContentRectangle.Left, m_messages.Rectangle.Height, Rectangle.Width, Rectangle.Height - m_messages.Rectangle.Height - statHeight)) { BackgroundColor = new Color(0, 15, 0) };
			m_stats = new StatsBlock(new Rectangle(0, Rectangle.Bottom - statHeight, Rectangle.Width, statHeight + 2)) { BackgroundColor = new Color(0, 30, 30) };
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

			var dPoint = KeyHelper.GetDirection(_key);

			if (dPoint != null)
			{
				World.TheWorld.Avatar.MoveCommandReceived(dPoint);
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
			m_map.Clear(_spriteBatch);
			m_messages.Clear(_spriteBatch);
			m_stats.Clear(_spriteBatch);
		}

		public override void DrawFrame(SpriteBatch _spriteBatch)
		{
			m_stats.DrawFrame(_spriteBatch);
			m_messages.DrawFrame(_spriteBatch);
			base.DrawFrame(_spriteBatch);
		}
	}
}