using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using GameCore;
using GameCore.Acts;
using GameCore.Mapping;


using Point = GameCore.Misc.Point;

namespace GameUi.UIBlocks
{
	class SelectTargetUiBlock:UIBlock
	{
		private readonly Point m_center;
		private int m_currentTarget;
		private readonly int m_maxDistance;
		private readonly Act m_act;
		private readonly MapCell[,] m_mapCells;
		private Point m_targetPoint;
		private readonly Point m_addPoint;
		readonly List<Point> m_targets =new List<Point>();
		private readonly TurnMessageUiBlock m_messages;
		private Point m_realTarget;

		public SelectTargetUiBlock(TurnMessageUiBlock _messages, Rectangle _mapRectangle, int _maxDistance, Act _act)
			: base(_mapRectangle, null, Color.Gray)
		{
			m_messages = _messages;
			m_maxDistance = _maxDistance;
			m_act = _act;
			m_targetPoint = Point.Zero;
			m_addPoint = new Point(ContentRectangle.Left + ContentRectangle.Width/2, ContentRectangle.Top + ContentRectangle.Height/2);
			m_center = new Point(m_maxDistance, m_maxDistance);

			m_mapCells = new MapCell[m_maxDistance * 2 + 1, m_maxDistance * 2 + 1];
			World.TheWorld.Avatar.Layer.SetData(m_mapCells, World.TheWorld.Avatar.Coords);

			var points = new List<Point>();
			for (var x = 0; x < m_mapCells.GetLength(0); ++x)
			{
				for (var y = 0; y < m_mapCells.GetLength(1); ++y)
				{
					if(m_mapCells[x, y].Creature!=null)
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
					if(m_targetPoint.Lenght>m_maxDistance)
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
					if(_modifiers!=EKeyModifiers.SHIFT) return;
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
		}

		private void SelectTargetFromList()
		{
			if (m_targets.Count==0) return;
			m_currentTarget = (m_currentTarget + m_targets.Count) % m_targets.Count;
			m_targetPoint = m_targets[m_currentTarget];
		}

		public override void DrawBackground()
		{
		}

		public override void DrawContent()
		{
			var strings = new List<string> { "[Enter|T] цель", "[z|Esc] - выход" };
			if (m_targets.Count > 0)
			{
				strings.Insert(1, "[-] - предыдущая цель");
				strings.Insert(1, "[+] - следующая цель");
			}

			m_messages.DrawLine(JoinCommandCaptions(strings), Color.White,0, 0, EAlignment.LEFT);

			var pnt = Point.Zero;
			var done = false;
			var color = Color.Gold;
			var lineToPoints = Point.Zero.GetLineToPoints(m_targetPoint).ToArray();
			for (var index = 1; index < lineToPoints.Length; index++)
			{
				var point = lineToPoints[index];
				var mapCell = m_mapCells[m_center.X + point.X, m_center.Y + point.Y];
				if (point.Lenght >= m_maxDistance || (!mapCell.IsCanShootThrough && mapCell.Creature == null))
				{
					color = Color.Red;
					done = true;
				}
				if (!done) pnt = point;
				if (mapCell.Creature != null)
				{
					color = Color.Red;
					done = true;
				}
				if (point.Lenght < 1) continue;
				ETiles.TARGET_DOT.DrawAtCell(point + m_addPoint, color);
			}
			ETiles.TARGET_CROSS.DrawAtCell(pnt + m_addPoint, Color.Gold);
			m_realTarget = pnt;
			
		}
	}
}
