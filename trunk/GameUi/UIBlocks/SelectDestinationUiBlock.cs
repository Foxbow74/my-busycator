using System;
using System.Collections.Generic;
using GameCore;
using GameCore.Acts;
using GameCore.Acts.Movement;
using GameCore.Messages;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class SelectDestinationUiBlock : UiBlockWithText
	{
		private readonly Act m_act;
		private readonly TurnMessageUiBlock m_messages;
		private Point m_avatarScreenPoint;
		private Point m_halfScreen;
		private IEnumerable<Point> m_path;
		private Point m_targetPoint;

		public SelectDestinationUiBlock(TurnMessageUiBlock _messages, Rct _mapRct, Act _act)
			: base(_mapRct, null, FColor.Gray)
		{
			m_messages = _messages;
			m_act = _act;
			Rebuild();
		}

		public override void Resize(Rct _newRct)
		{
			base.Resize(_newRct);
			Rebuild();
		}

		private void Rebuild()
		{
			m_targetPoint = Point.Zero;
			m_halfScreen = new Point(ContentRct.Width/2, ContentRct.Height/2);
			m_avatarScreenPoint = m_halfScreen + ContentRct.LeftTop;
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var dPoint = KeyTranslator.GetDirection(_key);
			if (dPoint != null)
			{
				var newPoint = m_targetPoint + dPoint + m_avatarScreenPoint;
				if (ContentRct.Contains(newPoint))
				{
					m_targetPoint += dPoint;
				}
			}
			switch (_key)
			{
				case ConsoleKey.Escape:
				case ConsoleKey.Z:
					m_act.IsCancelled = true;
					CloseTopBlock();
					break;
				case ConsoleKey.Enter:
				case ConsoleKey.M:
					if (m_path != null)
					{
						m_act.AddParameter(m_path);
						CloseTopBlock();
					}
					return;
			}
			MessageManager.SendMessage(this, WorldMessage.JustRedraw);
		}

		public override void DrawBackground() { }

		public override void DrawContent()
		{
			var strings = new List<string> {"[Enter|M] идти", "[z|Esc] - выход"};

			m_messages.DrawLine(JoinCommandCaptions(strings), FColor.White, 0, 0, EAlignment.LEFT);

			var color = FColor.Gold;
			var avatarPathMapCoords = World.TheWorld.Avatar[0, 0].PathMapCoords;
			var targetPathMapCoords = avatarPathMapCoords + m_targetPoint;
			var path = World.TheWorld.LiveMap.PathFinder.FindPath(World.TheWorld.Avatar, targetPathMapCoords);
			if (path != null)
			{
				m_path = MoveToAct.GetMoveToPath(World.TheWorld.Avatar, path);
				foreach (var point in path)
				{
					var pnt = point - avatarPathMapCoords;
					if (pnt.Lenght < 1) continue;

					if (ContentRct.Contains(pnt + m_avatarScreenPoint))
					{
						ETiles.TARGET_DOT.GetTile(0).Draw(pnt + m_avatarScreenPoint, color);
					}
				}
				ETiles.TARGET_CROSS.GetTile(0).Draw(m_targetPoint + m_avatarScreenPoint, FColor.Gold);
			}
			else
			{
				m_path = null;
				ETiles.TARGET_CROSS.GetTile(0).Draw(m_targetPoint + m_avatarScreenPoint, FColor.Red);
			}
		}

		public override void MouseMove(Point _pnt) { SetPoint(_pnt); }

		private void SetPoint(Point _pnt)
		{
			m_targetPoint = _pnt - m_avatarScreenPoint + ContentRct.LeftTop;
			MessageManager.SendMessage(this, WorldMessage.JustRedraw);
		}

		public override void MouseButtonUp(Point _pnt, EMouseButton _button)
		{
			if (_button != EMouseButton.LEFT) return;

			SetPoint(_pnt);
			KeysPressed(ConsoleKey.M, EKeyModifiers.NONE);
		}
	}
}