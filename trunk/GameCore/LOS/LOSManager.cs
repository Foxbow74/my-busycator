using System;
using System.Collections.Generic;
using System.Linq;
using Graphics;

namespace GameCore.LOS
{
	public class LosManager
	{
		private readonly LosCell m_root;

		public LosManager(MapCell[,] _mapCells)
		{
			var screenWidth = _mapCells.GetLength(0);
			var screenHeght = _mapCells.GetLength(1);

			m_root = new LosCell();
			var alreadyDone = new Dictionary<Point, LosCell> { { Point.Zero, m_root } };
			var radius = Math.Max(screenWidth, screenHeght) / 2;

			const double dRo = Math.PI / 1024;
			
			for (var ro = 0.0; ro < Math.PI*2; ro += dRo)
			{
				var x = Math.Sin(ro) * radius;
				var y = Math.Cos(ro) * radius;
				var end = new Point((int)x,(int)y);

				var parent = m_root;
				var closedByParent = 1.0;
				foreach (var pnt in Point.Zero.GetLineToPoints(end))
				{
					if(pnt.Equals(Point.Zero)) continue;

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

	class LosCell
	{
		/// <summary>
		/// double хранит величину - насколько та или иная ячейка видна через текущую
		/// </summary>
		readonly Dictionary<Tuple<Point, double>, LosCell> m_cells = new Dictionary<Tuple<Point, double>, LosCell>();

		public void GetVisibleCelss(MapCell[,] _mapCells, int _dx, int _dy, Dictionary<Point, double> _alreadyDone, double _visibilityCoeff)
		{
			var maxX = _mapCells.GetLength(0)-1;
			var maxY = _mapCells.GetLength(1)-1;

			foreach (var pair in m_cells)
			{
				var pnt = new Point(pair.Key.Item1.X + _dx, pair.Key.Item1.Y + _dy);

				if (pnt.X < 0 || pnt.X >= maxX) continue;
				if (pnt.Y < 0 || pnt.Y >= maxY) continue;

				var visible = (1.0 - (_mapCells[pnt.X, pnt.Y].Terrain.IsPassable() ? 0 : pair.Key.Item2)) * _visibilityCoeff;

				double visibility;
				if (_alreadyDone.TryGetValue(pnt, out visibility))
				{
					if(visibility>=visible) continue;
				}

				_alreadyDone[pnt] = _mapCells[pnt.X, pnt.Y].Terrain.IsPassable() ? visible : _visibilityCoeff;
				if (visible < 0.1) continue;

				pair.Value.GetVisibleCelss(_mapCells, _dx, _dy, _alreadyDone, visible * 0.99);
			}
		}

		public void Add(Point _pnt, double _closedByParent, LosCell _cell)
		{
			var have = m_cells.Keys.FirstOrDefault(_tuple => _tuple.Item1.Equals(_pnt));
			if(have!=null)
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
