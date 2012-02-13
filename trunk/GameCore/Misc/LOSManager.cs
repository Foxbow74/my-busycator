using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Mapping;

namespace GameCore.Misc
{
	public class LosManager
	{
		internal const int RADIUS = 20;
		const float DIVIDER = 6f;

		private readonly List<LosCell> m_inOrder;
		private readonly Dictionary<LosCell, float> m_visibles;
		private readonly Dictionary<LosCell, FColor> m_cvisibles;

		private readonly LosCell m_root;

		public LosManager()
		{
			m_root = new LosCell(Point.Zero);
			var alreadyDone= new Dictionary<Point, LosCell> { { Point.Zero, m_root } };


			var dVectors = new List<Vector2>();
			for (var di = -DIVIDER / 2 + 1; di < DIVIDER / 2; di++)
			{
				for (var dj = -DIVIDER / 2 + 1; dj < DIVIDER / 2; dj++)
				{
					var dv = new Vector2(di, dj) / DIVIDER;
					if (dv.Length() > Math.Sqrt(2)) continue;
					dVectors.Add(dv);
				}
			}

			var dividedPart = 1f / dVectors.Count;

			for (var i = RADIUS; i >= -RADIUS; --i)
			{
				for (var j = -RADIUS; j <= RADIUS; ++j)
				{
					var pnt = new Point(i, j);

					if (pnt.Equals(Point.Zero)) continue;

					LosCell cell;
					if (!alreadyDone.TryGetValue(pnt, out cell))
					{
						cell = new LosCell(pnt);
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
						LosCell parent;
						if (!alreadyDone.TryGetValue(parentPoint, out parent))
						{
							parent = new LosCell(parentPoint);
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
			m_cvisibles = m_inOrder.ToDictionary(_cell2 => _cell2, _cell2 => FColor.Empty);
		}

		const int LIGHTNESS_MIN = 20;
		const float TRANSPARENCE_MIN = 20f/255;

		public IEnumerable<Tuple<Point, FColor>> GetVisibleCelss(MapCell[,] _mapCells, Point _dPoint, FColor _startFrom)
		{
			var maxX = _mapCells.GetLength(0) - 1;
			var maxY = _mapCells.GetLength(1) - 1;

			m_cvisibles[m_root] = _startFrom;
			m_visibles[m_root] = 1;

			for (var index = 1; index < m_inOrder.Count; index++)
			{
				m_cvisibles[m_inOrder[index]] = FColor.Empty;
				m_visibles[m_inOrder[index]] = 0;
			}

			foreach (var cell in m_inOrder)
			{
				var visibilityCoeff = m_visibles[cell];
				var color = m_cvisibles[cell];

				if (visibilityCoeff <= 0) continue;

				var myPnt = cell.Point + _dPoint;

				if (myPnt.X < 0 || myPnt.X >= maxX || myPnt.Y < 0 || myPnt.Y >= maxY) continue;
				
				var mapCell = _mapCells[myPnt.X, myPnt.Y];

				yield return new Tuple<Point, FColor>(myPnt, new FColor(visibilityCoeff, color));

				var transColor = mapCell.TransparentColor;

				//var childsVisible = (1f - mapCell.Opacity) * visibilityCoeff;
				var childsVisible = transColor.A * visibilityCoeff;
				var childsColor = color.Multiply(transColor);

				//if (childsColor.A <= TRANSPARENCE_MIN) continue;
				if (childsVisible <= 0) continue;

				foreach (var pair in cell.Cells)
				{
					var pnt = pair.Key.Point + _dPoint;
					if (pnt.X < 0 || pnt.X >= maxX || pnt.Y < 0 || pnt.Y >= maxY) continue;

					m_visibles[pair.Key] += pair.Value * childsVisible;
					m_cvisibles[pair.Key] = m_cvisibles[pair.Key].ScreenColorsOnly(childsColor);
				}
			}
		}

		public IEnumerable<Tuple<Point, float>> GetVisibleCelss(MapCell[,] _mapCells, Point _dPoint)
		{
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

				if(visibilityCoeff<=0) continue;

				var myPnt = cell.Point + _dPoint;
				
				var mapCell = _mapCells[myPnt.X, myPnt.Y];
				var childsVisible = (1f - mapCell.Opacity) * visibilityCoeff;

				if (childsVisible <= 0) continue;
				foreach (var pair in cell.Cells)
				{
					var pnt = pair.Key.Point + _dPoint;
					if (pnt.X < 0 || pnt.X >= maxX || pnt.Y < 0 || pnt.Y >= maxY) continue;

					m_visibles[pair.Key] += pair.Value*childsVisible;
				}
			}
			return from pair in m_visibles select new Tuple<Point, float>(pair.Key.Point + _dPoint, pair.Value);
		}
	}

	class LosCell
	{
		public LosCell(Point _point)
		{
			Cells = new Dictionary<LosCell, float>();
			Point = _point;
			var r = _point.Lenght / LosManager.RADIUS;
			var fi = Math.Asin(r);
			var dc = (float)Math.Cos(fi);
			DistanceCoefficient = Math.Min(dc, 1f);
		}

		public float DistanceCoefficient { get; private set; }

		public Point Point { get; private set; }

		public Dictionary<LosCell, float> Cells { get; private set; }

		public override string ToString()
		{
			return string.Format("{0}*{2} cells:{1}", Point, Cells.Count, DistanceCoefficient);
		}

		public void Add(Point _pnt, float _closedByParent, LosCell _cell)
		{
			float value;
			if(Cells.TryGetValue(_cell, out value))
			{
				_closedByParent += value;
			}
			Cells[_cell] = _closedByParent;
		}

		public void UpdateByDistance()
		{
			var cells = Cells.Keys.ToArray();
			foreach (var cell in cells)
			{
				Cells[cell] *= cell.DistanceCoefficient;
			}
		}
	}
}
