using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Mapping;

namespace GameCore.Misc
{
	public class LosManager
	{
		internal const int RADIUS = 15;
		const float DIVIDER = 6f;
		const float MIN_VISIBILITY = 0.05f;

		private readonly List<LosCell2> m_inOrder;
		private readonly Dictionary<LosCell2, float> m_visibles;

		private readonly LosCell2 m_root;

		public LosManager()
		{
			m_root = new LosCell2(Point.Zero);
			var alreadyDone= new Dictionary<Point, LosCell2> { { Point.Zero, m_root } };


			var dVectors = new List<Vector2>();
			for (var di = -DIVIDER / 2 + 1; di < DIVIDER / 2; di++)
			{
				for (var dj = -DIVIDER / 2 + 1; dj < DIVIDER / 2; dj++)
				{
					Debug.WriteLine(di + ", " + dj);
					var dv = new Vector2(di, dj) / DIVIDER;
					if (dv.Length() > Math.Sqrt(2)) continue;
					dVectors.Add(dv);
				}
			}

			var dividedPart = 1f / dVectors.Count;

			for (var i = -RADIUS; i <= RADIUS; ++i)
			{
				for (var j = -RADIUS; j <= RADIUS; ++j)
				{
					var pnt = new Point(i, j);

					if (pnt.Equals(Point.Zero)) continue;

					LosCell2 cell;
					if (!alreadyDone.TryGetValue(pnt, out cell))
					{
						cell = new LosCell2(pnt);
						if (cell.Point.Lenght > RADIUS)
						{
							continue;
						}
						alreadyDone.Add(pnt, cell);
					}

					foreach (var dv in dVectors)
					{
						var v = new Vector2(pnt.X, pnt.Y) + dv;
						v -= new Vector2(Math.Sign(v.X) / 2f, Math.Sign(v.Y) / 2f);

						var parentPoint = Point.Zero;
						foreach (var lineV in v.GetLineToPoints(Vector2.Zero, 0.02f))
						{
							var point = new Point((int)Math.Round(lineV.X), (int)Math.Round(lineV.Y));
							if (point.Equals(pnt) || point.Lenght > RADIUS) continue;
							parentPoint = point;
							break;
						}
						LosCell2 parent;
						if (!alreadyDone.TryGetValue(parentPoint, out parent))
						{
							parent = new LosCell2(parentPoint);
							alreadyDone.Add(parentPoint, parent);
						}
						parent.Add(pnt, dividedPart, cell);
					}
				}

			}

			foreach (var cell in alreadyDone.Values)
			{
				cell.UpdateByDistance();
			}
			m_inOrder = alreadyDone.Values.OrderByDescending(_cell2 => _cell2.DistanceCoefficient).ToList();
			m_visibles = m_inOrder.ToDictionary(_cell2 => _cell2, _cell2 => 0f);
		}

		public IEnumerable<Tuple<Point, float>> GetVisibleCelss(MapCell[,] _mapCells, int _dx, int _dy)
		{
			var dPoint = new Point(_dx, _dy);
			
			var maxX = _mapCells.GetLength(0) - 1;
			var maxY = _mapCells.GetLength(1) - 1;
			
			m_visibles[m_root] = 1;
			for (var index = 1; index < m_inOrder.Count; index++)
			{
				m_visibles[m_inOrder[index]] = 0;
			}

			foreach (var cell in m_inOrder)
			{
				var visibilityCoeff = m_visibles[cell];

				if(visibilityCoeff<MIN_VISIBILITY) continue;

				var myPnt = cell.Point + dPoint;
				
				var mapCell = _mapCells[myPnt.X, myPnt.Y];
				 var childsVisible = (1f - mapCell.Opacity) * visibilityCoeff;

				foreach (var pair in cell.Cells)
				{
					var pnt = pair.Key.Point + dPoint;
					if (pnt.X < 0 || pnt.X >= maxX || pnt.Y < 0 || pnt.Y >= maxY) continue;

					m_visibles[pair.Key] += pair.Value*childsVisible;
				}
			}
			return from pair in m_visibles where pair.Value > MIN_VISIBILITY select new Tuple<Point, float>(pair.Key.Point + dPoint, pair.Value);
		}
	}

	internal class LosCell2
	{
		public LosCell2(Point _point)
		{
			Point = _point;
			var r = 1f - _point.Lenght / ((float)(LosManager.RADIUS * Math.Sqrt(2)));
			DistanceCoefficient = Math.Min(r, 1f);
		}

		public float DistanceCoefficient { get; private set; }

		public Point Point { get; private set; }
		public Dictionary<LosCell2, float> Cells
		{
			get { return m_cells; }
		}

		public override string ToString()
		{
			return string.Format("{0}*{2} cells:{1}", Point, Cells.Count, DistanceCoefficient);
		}

		private readonly Dictionary<LosCell2, float> m_cells = new Dictionary<LosCell2, float>();

		public void Add(Point _pnt, float _closedByParent, LosCell2 _cell)
		{
			float value;
			if(m_cells.TryGetValue(_cell, out value))
			{
				_closedByParent += value;
			}
			m_cells[_cell] = _closedByParent;
		}

		public void UpdateByDistance()
		{
			var cells = m_cells.Keys.ToArray();
			foreach (var cell in cells)
			{
				m_cells[cell] *= cell.DistanceCoefficient;
			}
		}
	}
}
