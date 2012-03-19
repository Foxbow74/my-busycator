using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore;
using GameCore.Acts;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.PathFinding;

namespace GameUi.UIBlocks
{
	class SelectDestinationUiBlock : UiBlockWithText
	{
		private readonly Act m_act;
		private Point m_avatarScreenPoint;
		private Point m_halfScreen;
		private readonly TurnMessageUiBlock m_messages;
		private Point m_realTarget;
		private Point m_targetPoint;

		public override void Resize(Rct _newRct)
		{
			base.Resize(_newRct);
			Rebuild();
		}

		public SelectDestinationUiBlock(TurnMessageUiBlock _messages, Rct _mapRct, Act _act)
			: base(_mapRct, null, FColor.Gray)
		{
			m_messages = _messages;
			m_act = _act;
			Rebuild();
		}

		private void Rebuild()
		{
			m_targetPoint = Point.Zero;
			m_halfScreen = new Point(ContentRct.Width / 2, ContentRct.Height / 2);
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
					m_act.AddParameter(m_realTarget);
					CloseTopBlock();
					return;
			}
			MessageManager.SendMessage(this, WorldMessage.JustRedraw);
		}

		public override void DrawBackground()
		{
		}

		public override void DrawContent()
		{
			var strings = new List<string> {"[Enter|M] идти", "[z|Esc] - выход"};

			m_messages.DrawLine(JoinCommandCaptions(strings), FColor.White, 0, 0, EAlignment.LEFT);

			var pnt = Point.Zero;
			var color = FColor.Gold;
			var avatarPathMapCoords = World.TheWorld.Avatar[0, 0].PathMapCoords;
			var targetPathMapCoords = avatarPathMapCoords + m_targetPoint;
			var path = World.TheWorld.LiveMap.PathFinder.FindPath(World.TheWorld.Avatar, targetPathMapCoords, PathFinder.HeuristicFormula.EUCLIDEAN_NO_SQR);
			if (path != null)
			{
				foreach (var point in path)
				{
					pnt = point - avatarPathMapCoords;
					if (pnt.Lenght < 1) continue;
					ETiles.TARGET_DOT.GetTile().Draw(pnt + m_avatarScreenPoint, color);
				}
				m_realTarget = World.TheWorld.Avatar[m_targetPoint].WorldCoords;
			}
			ETiles.TARGET_CROSS.GetTile().Draw(m_targetPoint + m_avatarScreenPoint, FColor.Gold);
		}

		public override void MouseMove(Point _pnt)
		{
			SetPoint(_pnt);
		}

		private void SetPoint(Point _pnt)
		{
			m_targetPoint = _pnt - m_avatarScreenPoint + ContentRct.LeftTop;
			MessageManager.SendMessage(this, WorldMessage.JustRedraw);
		}

		public override void MouseButtonUp(Point _pnt, EMouseButton _button)
		{
			if (_button != EMouseButton.LEFT) return;

			SetPoint(_pnt);
			KeysPressed(ConsoleKey.T, EKeyModifiers.NONE);
		}
	}
}