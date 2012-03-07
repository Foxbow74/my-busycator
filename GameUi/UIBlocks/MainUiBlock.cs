using System;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using GameUi.UIBlocks.Help;
using GameUi.UIBlocks.Items;

namespace GameUi.UIBlocks
{
	internal class MainUiBlock : UIBlock
	{
		private readonly UIBlock m_map;
		private readonly TurnMessageUiBlock m_messages;
		private readonly UIBlock m_stats;
		const int MESSAGES_HEIGHT = 3;
		const int STAT_HEIGHT = 2;

		public MainUiBlock(int _width, int _height)
			: base(new Rct(0, 0, _width, _height), null, FColor.White)
		{
			m_messages = new TurnMessageUiBlock(new Rct(Rct.Left, 0, Rct.Width, MESSAGES_HEIGHT)) { BackgroundColor = FColor.FromArgb(255, 30, 30, 30) };
			m_map = new MapUiBlock(new Rct(ContentRct.Left, m_messages.Rct.Height, Rct.Width, Rct.Height - m_messages.Rct.Height - STAT_HEIGHT));
			m_stats = new StatsBlock(new Rct(0, Rct.Bottom - STAT_HEIGHT + 1, Rct.Width, STAT_HEIGHT)) { BackgroundColor = FColor.FromArgb(255, 0, 30, 30) };
		}

		public override void Resize(Rct _newRct)
		{
			base.Resize(_newRct);
			m_messages.Resize(new Rct(Rct.Left, 0, Rct.Width, MESSAGES_HEIGHT));
			m_map.Resize(new Rct(ContentRct.Left, m_messages.Rct.Height, Rct.Width, Rct.Height - m_messages.Rct.Height - STAT_HEIGHT));
			m_stats.Resize(new Rct(0, Rct.Bottom - STAT_HEIGHT + 1, Rct.Width, STAT_HEIGHT));
		}

		public override void Dispose()
		{
			m_messages.Dispose();
			m_map.Dispose();
			m_stats.Dispose();
			base.Dispose();
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
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new EquipmentUiBlock(Rct)));
				return;
			}

			if (_key == ConsoleKey.M)
			{
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new MiniMapUiBlock(Rct)));
				return;
			}

			if (_key == ConsoleKey.Oem2 && _modifiers == EKeyModifiers.SHIFT)
			{
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new HelpUiBlock(Rct)));
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