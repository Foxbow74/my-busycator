using System;
using System.Drawing;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using GameUi.UIBlocks.Help;
using GameUi.UIBlocks.Items;
using GameUi.UIBlocks.Map;

namespace GameUi.UIBlocks
{
	internal class MainUiBlock : UIBlock
	{
		private readonly UIBlock m_map;
		private readonly TurnMessageUiBlock m_messages;
		private readonly UIBlock m_stats;

		public MainUiBlock(int _width, int _height)
			: base(new Rectangle(0, 0, _width, _height), null, Color.White.ToFColor())
		{
			const int messagesHeight = 3;
			const int statHeight = 2;

			m_messages = new TurnMessageUiBlock(new Rectangle(Rectangle.Left, 0, Rectangle.Width, messagesHeight)) { BackgroundColor = Color.FromArgb(255, 30, 30, 30).ToFColor() };
			m_map = new MapUiBlock(new Rectangle(ContentRectangle.Left, m_messages.Rectangle.Height, Rectangle.Width, Rectangle.Height - m_messages.Rectangle.Height - statHeight));
			m_stats = new StatsBlock(new Rectangle(0, Rectangle.Bottom - statHeight, Rectangle.Width, statHeight)) { BackgroundColor = Color.FromArgb(255, 0, 30, 30).ToFColor() };
		}

		public UIBlock Map
		{
			get { return m_map; }
		}

		public TurnMessageUiBlock Messages
		{
			get { return m_messages; }
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
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new EquipmentUiBlock(Rectangle)));
				return;
			}

			if (_key == ConsoleKey.M)
			{
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new MiniMapUiBlock(Rectangle)));
				return;
			}

			if (_key == ConsoleKey.Oem2 && _modifiers == EKeyModifiers.SHIFT)
			{
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new HelpUiBlock(Rectangle)));
				return;
			}

			World.TheWorld.KeyPressed(_key, _modifiers);
		}

		public override void DrawContent()
		{
			m_map.DrawContent();
			m_messages.DrawContent();
		}

		public override void DrawBackground()
		{
			m_map.DrawBackground();
			m_messages.DrawBackground();
			m_stats.DrawBackground();
		}

		public override void DrawFrame()
		{
			m_stats.DrawFrame();
			m_messages.DrawFrame();
			base.DrawFrame();
		}
	}
}