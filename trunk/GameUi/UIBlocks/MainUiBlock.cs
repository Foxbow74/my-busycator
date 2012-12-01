using System;
using System.Linq;
using GameCore;
using GameCore.Acts.System;
using GameCore.Messages;
using GameCore.Misc;
using UnsafeUtils;

namespace GameUi.UIBlocks
{
	internal class MainUiBlock : UIBlock
	{
		private const int MESSAGES_HEIGHT = 3;
		private const int STAT_HEIGHT = 2;
		private readonly UIBlock m_map;
		private readonly TurnMessageUiBlock m_messages;
		private readonly UIBlock m_stats;

		public MainUiBlock(int _width, int _height)
			: base(new Rct(0, 0, _width, _height), null, FColor.White)
		{
			m_messages = new TurnMessageUiBlock(new Rct(Rct.Left, 0, Rct.Width, MESSAGES_HEIGHT)) {BackgroundColor = FColor.FromArgb(255, 30, 30, 30)};
			m_map = new MapUiBlock(new Rct(ContentRct.Left, m_messages.Rct.Height, Rct.Width, Rct.Height - m_messages.Rct.Height - STAT_HEIGHT));
			m_stats = new StatsBlock(new Rct(0, Rct.Bottom - STAT_HEIGHT + 1, Rct.Width, STAT_HEIGHT)) {BackgroundColor = FColor.FromArgb(255, 0, 30, 30)};
		}

		public UIBlock Map { get { return m_map; } }

		public TurnMessageUiBlock Messages { get { return m_messages; } }

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

		public bool NeedWait { get { return m_messages.NeedWait && World.TheWorld.Avatar.NextAct==null; } }

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var tuple = new Tuple<ConsoleKey, EKeyModifiers>(_key, _modifiers);
			if (new QuitAct().ConsoleKeys.Contains(tuple))
			{
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new ConfirmQuitBlock()));
				return;
			}

			if(m_messages.NeedWait)
			{
				m_messages.KeysPressed(_key, _modifiers);
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

		public override void MouseMove(Point _pnt)
		{
			base.MouseMove(_pnt);

			foreach (var uiBlock in new[] {m_stats, m_messages, m_map})
			{
				if (uiBlock.ContentRct.Contains(_pnt))
				{
					uiBlock.MouseMove(_pnt - uiBlock.ContentRct.LeftTop);
					break;
				}
			}
		}

		public override void MouseButtonDown(Point _pnt, EMouseButton _button)
		{
			base.MouseButtonDown(_pnt, _button);

			foreach (var uiBlock in new[] {m_stats, m_messages, m_map})
			{
				if (uiBlock.ContentRct.Contains(_pnt))
				{
					uiBlock.MouseButtonDown(_pnt - uiBlock.ContentRct.LeftTop, _button);
					break;
				}
			}
		}

		public override void MouseButtonUp(Point _pnt, EMouseButton _button)
		{
			base.MouseButtonUp(_pnt, _button);

			foreach (var uiBlock in new[] {m_stats, m_messages, m_map})
			{
				if (uiBlock.ContentRct.Contains(_pnt))
				{
					uiBlock.MouseButtonUp(_pnt - uiBlock.ContentRct.LeftTop, _button);
					break;
				}
			}
		}
	}
}