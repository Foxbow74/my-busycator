﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Acts;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameUi.UIBlocks
{
	internal class SelectTargetUiBlock : UiBlockWithText
	{
		private readonly Act m_act;
		private readonly Point m_addPoint;
		private readonly Point m_center;
		private readonly int m_maxDistance;
		private readonly TurnMessageUiBlock m_messages;
		private readonly List<Point> m_targets = new List<Point>();
		private int m_currentTarget;
		private Point m_realTarget;
		private Point m_targetPoint;

		public SelectTargetUiBlock(TurnMessageUiBlock _messages, Rectangle _mapRectangle, int _maxDistance, Act _act)
			: base(_mapRectangle, null, Color.Gray.ToFColor())
		{
			m_messages = _messages;
			m_maxDistance = _maxDistance;
			m_act = _act;
			m_targetPoint = Point.Zero;
			m_addPoint = new Point(ContentRectangle.Left + ContentRectangle.Width/2,
			                       ContentRectangle.Top + ContentRectangle.Height/2);
			m_center = new Point(m_maxDistance, m_maxDistance);

			var dPoint = World.TheWorld.LiveMap.GetData();
			var points = new List<Point>();


			var width = ContentRectangle.Width;
			var height = ContentRectangle.Height;

			for (var x = 0; x < width; ++x)
			{
				for (var y = 0; y < height; ++y)
				{
					var pnt = LiveMap.WrapCellCoords(new Point(x + dPoint.X, y + dPoint.Y));
					var liveCell = World.TheWorld.LiveMap.Cells[pnt.X, pnt.Y];
					if (liveCell.Creature != null)
					{
						points.Add(new Point(x - m_center.X, y - m_center.Y));
					}
				}
			}

			m_targets.AddRange(points.Where(_point => _point.Lenght < m_maxDistance).OrderBy(_point => _point.Lenght));
			SelectTargetFromList();
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var dPoint = KeyTranslator.GetDirection(_key);
			if (dPoint != null)
			{
				var newPoint = m_targetPoint + dPoint + m_addPoint;
				if (ContentRectangle.Contains(newPoint.X, newPoint.Y))
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
					if (_modifiers != EKeyModifiers.SHIFT) return;
					m_currentTarget++;
					SelectTargetFromList();
					break;
				case ConsoleKey.OemMinus:
					if (_modifiers != EKeyModifiers.SHIFT) return;
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
			MessageManager.SendMessage(this, WorldMessage.Turn);
		}

		private void SelectTargetFromList()
		{
			if (m_targets.Count == 0) return;
			m_currentTarget = (m_currentTarget + m_targets.Count)%m_targets.Count;
			m_targetPoint = m_targets[m_currentTarget];
		}

		public override void DrawBackground()
		{
		}

		public override void DrawContent()
		{
			var strings = new List<string> {"[Enter|T] цель", "[z|Esc] - выход"};
			if (m_targets.Count > 0)
			{
				strings.Insert(1, "[-] - предыдущая цель");
				strings.Insert(1, "[+] - следующая цель");
			}

			m_messages.DrawLine(JoinCommandCaptions(strings), Color.White.ToFColor(), 0, 0, EAlignment.LEFT);

			var pnt = Point.Zero;
			var done = false;
			var color = new FColor(Color.Gold);
			var lineToPoints = Point.Zero.GetLineToPoints(m_targetPoint).ToArray();

			var dPoint = World.TheWorld.LiveMap.GetData();

			for (var index = 1; index < lineToPoints.Length; index++)
			{
				var point = lineToPoints[index];

				var liveCell = World.TheWorld.LiveMap.GetCell(point + dPoint);

				if (point.Lenght >= m_maxDistance || (!liveCell.IsCanShootThrough && liveCell.Creature == null))
				{
					color =new FColor(Color.Red);
					done = true;
				}
				if (!done) pnt = point;
				if (liveCell.Creature != null)
				{
					color = new FColor(Color.Red);
					done = true;
				}
				if (point.Lenght < 1) continue;
				ETiles.TARGET_DOT.GetTile().Draw(point + m_addPoint, color,BackgroundColor);
			}
			ETiles.TARGET_CROSS.GetTile().Draw(pnt + m_addPoint, new FColor(Color.Gold), BackgroundColor);
			m_realTarget = pnt;
		}
	}
}