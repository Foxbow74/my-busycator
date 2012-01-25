#region

using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Mapping;
using GameCore.Misc;

#endregion

namespace RGL1.UIBlocks.Map
{
	public class CColor
	{
		public CColor(int _r, int _g, int _b)
		{
			R = _r;
			G = _g;
			B = _b;
		}

		public int R { get; set; }
		public int G { get; set; }
		public int B { get; set; }
	}

	public class LosManager
	{
		private readonly LosCell m_root;

		public LosManager(MapCell[,] _mapCells)
		{
			var screenWidth = _mapCells.GetLength(0);
			var screenHeght = _mapCells.GetLength(1);

			var rnd = new Random(1);

			m_root = new LosCell(new CColor(64, 64, 64));
			var alreadyDone = new Dictionary<Point, LosCell> {{Point.Zero, m_root}};
			var radius = Math.Max(screenWidth, screenHeght)/2;

			const double dRo = Math.PI/1024;

			for (var ro = 0.0; ro < Math.PI*2; ro += dRo)
			{
				var x = Math.Sin(ro)*radius;
				var y = Math.Cos(ro)*radius;
				var end = new Point((int) x, (int) y);

				var ccolor = new CColor(rnd.Next(255), rnd.Next(255), rnd.Next(255));

				var parent = m_root;
				var closedByParent = 1.0;
				foreach (var pnt in Point.Zero.GetLineToPoints(end))
				{
					if (pnt == Point.Zero) continue;

					LosCell cell;
					if (!alreadyDone.TryGetValue(pnt, out cell))
					{
						cell = new LosCell(ccolor);
						alreadyDone.Add(pnt, cell);
					}

					parent.Add(pnt, closedByParent, cell);

					parent = cell;
					closedByParent = 1.0 - pnt.GetDistanceToVector(end);
				}
			}
		}

		public Dictionary<Point, Tuple<double, CColor>> GetVisibleCelss(MapCell[,] _mapCells, int _dx, int _dy)
		{
			var alreadyDone = new Dictionary<Point, Tuple<double, CColor>>();
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

		public LosCell(CColor _ccolor)
		{
			Ccolor = _ccolor;
		}

		public CColor Ccolor { get; set; }

		public void GetVisibleCelss(MapCell[,] _mapCells, int _dx, int _dy,
		                            Dictionary<Point, Tuple<double, CColor>> _alreadyDone, double _visibilityCoeff)
		{
			var maxX = _mapCells.GetLength(0) - 1;
			var maxY = _mapCells.GetLength(1) - 1;

			foreach (var pair in m_cells)
			{
				var pnt = new Point(pair.Key.Item1.X + _dx, pair.Key.Item1.Y + _dy);

				if (pnt.X < 0 || pnt.X >= maxX) continue;
				if (pnt.Y < 0 || pnt.Y >= maxY) continue;

				var attr = _mapCells[pnt.X, pnt.Y].TerrainAttribute;

				var visible = (1.0 - attr.Opaque*pair.Key.Item2)*_visibilityCoeff;
				var ccolor = pair.Value.Ccolor;

				Tuple<double, CColor> tuple;
				if (_alreadyDone.TryGetValue(pnt, out tuple))
				{
					var visibility = tuple.Item1;
					if (visibility >= visible) continue;
				}

				if (attr.Opaque < 0.999f)
				{
					_alreadyDone[pnt] = new Tuple<double, CColor>(visible, ccolor);
				}
				else
				{
					_alreadyDone[pnt] = new Tuple<double, CColor>(_visibilityCoeff, new CColor(0, 0, 0));
					continue;
				}

				if (visible < 0.1) continue;

				pair.Value.GetVisibleCelss(_mapCells, _dx, _dy, _alreadyDone, visible*0.95);
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