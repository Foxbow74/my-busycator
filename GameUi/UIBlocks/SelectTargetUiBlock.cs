using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Messages;
using GameCore.Misc;
using UnsafeUtils;

namespace GameUi.UIBlocks
{
	internal class SelectTargetUiBlock : UiBlockWithText
	{
		private readonly Act m_act;
		private readonly int m_maxDistance;
		private readonly TurnMessageUiBlock m_messages;
		private readonly List<Point> m_targets = new List<Point>();
		private Point m_addPoint;
		private Point m_center;
		private int m_currentTarget;
		private Point m_realTarget;
		private Point m_targetPoint;

		public SelectTargetUiBlock(TurnMessageUiBlock _messages, Rct _mapRct, AskMessage _message)
			: base(_mapRct, null, FColor.Gray)
		{
			m_messages = _messages;
			m_act = _message.Act;
			m_maxDistance = _message.GetFirstParameter<int>();
			var points = new List<Point>();

			for (var x = -m_maxDistance; x < m_maxDistance; ++x)
			{
				for (var y = -m_maxDistance; y < m_maxDistance; ++y)
				{
					var point = new Point(x, y);
					if (point.Lenght > m_maxDistance) continue;

					var liveCell = World.TheWorld.Avatar[point];
					if (liveCell.Creature != null && !liveCell.Creature.IsAvatar)
					{
						points.Add(point);
					}
				}
			}

			m_targets.AddRange(points.Where(_point => _point.Lenght < m_maxDistance).OrderBy(_point => _point.Lenght));

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
			m_center = new Point(ContentRct.Width/2, ContentRct.Height/2);
			m_addPoint = new Point(ContentRct.Left, ContentRct.Top) + m_center;

			SelectTargetFromList();
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var dPoint = KeyTranslator.GetDirection(_key);
			if (dPoint != null)
			{
				var newPoint = m_targetPoint + dPoint + m_addPoint;
				if (ContentRct.Contains(newPoint))
				{
					m_targetPoint += dPoint;
					if (m_targetPoint.Lenght > m_maxDistance)
					{
						m_targetPoint *= m_maxDistance/m_targetPoint.Lenght;
					}
				}
			}
			switch (_key)
			{
				case ConsoleKey.Escape:
				case ConsoleKey.Z:
					m_act.IsCancelled = true;
					CloseTopBlock();
					break;
				case ConsoleKey.OemPlus:
					if (_modifiers != EKeyModifiers.NONE) return;
					m_currentTarget++;
					SelectTargetFromList();
					break;
				case ConsoleKey.OemMinus:
					if (_modifiers != EKeyModifiers.NONE) return;
					m_currentTarget--;
					SelectTargetFromList();
					break;
				case ConsoleKey.Add:
					m_currentTarget++;
					SelectTargetFromList();
					break;
				case ConsoleKey.Subtract:
					m_currentTarget--;
					SelectTargetFromList();
					break;
				case ConsoleKey.Enter:
				case ConsoleKey.T:
					m_act.AddParameter(m_realTarget);
					CloseTopBlock();
					return;
			}
			//MessageManager.SendMessage(this, WorldMessage.JustRedraw);
		}

		private void SelectTargetFromList()
		{
			if (m_targets.Count == 0) return;
			m_currentTarget = (m_currentTarget + m_targets.Count)%m_targets.Count;
			m_targetPoint = m_targets[m_currentTarget];
		}

		public override void DrawBackground() { }

		public override void DrawContent()
		{
			var strings = new List<string> { "[Enter|T] цель", "[z|Esc] - " + EALConst.EXIT.GetString() };
			if (m_targets.Count > 0)
			{
				strings.Insert(1, "[-] - предыдущая цель");
				strings.Insert(1, "[+] - следующая цель");
			}

			m_messages.DrawLine(JoinCommandCaptions(strings), FColor.White, 0, 0, EAlignment.LEFT);

			var pnt = Point.Zero;
			var done = false;
			var color = FColor.Gold;
			var lineToPoints = Point.Zero.GetLineToPoints(m_targetPoint).ToArray();

			for (var index = 1; index < lineToPoints.Length; index++)
			{
				var point = lineToPoints[index];
				var liveCell = World.TheWorld.Avatar[point];

				if (point.Lenght >= m_maxDistance || (!liveCell.IsCanShootThrough && liveCell.Creature == null))
				{
					color = FColor.Red;
					done = true;
				}
				if (!done) pnt = point;
				if (liveCell.Creature != null)
				{
					color = FColor.Red;
					done = true;
				}
				if (point.Lenght < 1) continue;
                ETileset.TARGETING.GetTile(1).Draw(point + m_addPoint, color);
			}
            ETileset.TARGETING.GetTile(0).Draw(pnt + m_addPoint, FColor.Gold);
			m_realTarget = pnt;
		}

		public override void MouseMove(Point _pnt) { SetPoint(_pnt); }

		private void SetPoint(Point _pnt)
		{
			m_targetPoint = _pnt - m_addPoint + ContentRct.LeftTop;
			if (m_targetPoint.Lenght > m_maxDistance)
			{
				m_targetPoint *= m_maxDistance/m_targetPoint.Lenght;
			}
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