using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Mapping;

namespace GameCore.Misc
{
	public class LosManager
	{
		private readonly LosCell m_root;

		public LosManager()
		{
			m_root = new LosCell();
			var alreadyDone = new Dictionary<Point, LosCell> {{Point.Zero, m_root}};
			const int radius = 20;

			const double dRo = Math.PI/2048;

			for (var ro = 0.0; ro < Math.PI*2; ro += dRo)
			{
				var x = Math.Sin(ro)*radius;
				var y = Math.Cos(ro)*radius;
				var end = new Point((int) x, (int) y);

				var parent = m_root;
				var closedByParent = 1.0;
				foreach (var pnt in Point.Zero.GetLineToPoints(end).Where(_pnt => _pnt != Point.Zero))
				{
					LosCell cell;
					if (!alreadyDone.TryGetValue(pnt, out cell))
					{
						cell = new LosCell();
						alreadyDone.Add(pnt, cell);
					}

					parent.Add(pnt, closedByParent, cell);

					parent = cell;
					closedByParent = 1.0 - pnt.GetDistanceToVector(end);
				}
			}
		}

		public Dictionary<Point, double> GetVisibleCelss(MapCell[,] _mapCells, int _dx, int _dy)
		{
			var alreadyDone = new Dictionary<Point, double>();
			m_root.GetVisibleCelss(_mapCells, _dx, _dy, alreadyDone, 1.0);
			return alreadyDone;
		}
	}

	internal class LosCell
	{
		/// <summary>
		/// 	double хранит величину - насколько та или иная ячейка видна через текущую
		/// </summary>
		private readonly Dictionary<Tuple<Point, double>, LosCell> m_cells = new Dictionary<Tuple<Point, double>, LosCell>();

		public void GetVisibleCelss(MapCell[,] _mapCells, int _dx, int _dy,
		                            Dictionary<Point, double> _alreadyDone, double _visibilityCoeff)
		{
			var maxX = _mapCells.GetLength(0) - 1;
			var maxY = _mapCells.GetLength(1) - 1;

			foreach (var pair in m_cells)
			{
				var pnt = new Point(pair.Key.Item1.X + _dx, pair.Key.Item1.Y + _dy);

				if (pnt.X < 0 || pnt.X >= maxX) continue;
				if (pnt.Y < 0 || pnt.Y >= maxY) continue;

				var mapCell = _mapCells[pnt.X, pnt.Y];

				var opaque = mapCell.Opaque;

				var visible = 1.0*_visibilityCoeff;
				var nextVisible = (1.0 - opaque*pair.Key.Item2)*_visibilityCoeff;

				double tuple;
				if (_alreadyDone.TryGetValue(pnt, out tuple))
				{
					var visibility = tuple;
					if (visibility >= visible) continue;
				}

				if (opaque < 0.999f)
				{
					_alreadyDone[pnt] = visible;
				}
				else
				{
					_alreadyDone[pnt] = _visibilityCoeff;
					continue;
				}

				if (visible < 0.1) continue;

				pair.Value.GetVisibleCelss(_mapCells, _dx, _dy, _alreadyDone, nextVisible*0.99);
			}
		}

		public void Add(Point _pnt, double _closedByParent, LosCell _cell)
		{
			var have = m_cells.Keys.FirstOrDefault(_tuple => _tuple.Item1 == _pnt);
			if (have != null)
			{
				if (have.Item2 < _closedByParent)
				{
					m_cells.Remove(have);
				}
				else
				{
					return;
				}
			}
			m_cells.Add(new Tuple<Point, double>(_pnt, _closedByParent), _cell);
		}
	}
}