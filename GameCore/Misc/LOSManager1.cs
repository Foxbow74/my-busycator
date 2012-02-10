using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Mapping;

namespace GameCore.Misc
{
	public class LosManager1
	{
		private readonly LosCell1 m_root;
		internal const int RADIUS =10;
		const float DIVIDER = 4f;


		private readonly Dictionary<Point, LosCell1> m_alreadyDone;

		public LosManager1()
		{
			m_root = new LosCell1(Point.Zero);
			m_alreadyDone = new Dictionary<Point, LosCell1> { { Point.Zero, m_root } };


			var dVectors = new List<Vector2>();
			for (var di = -DIVIDER / 2; di < DIVIDER / 2; di++)
			{
				for (var dj = -DIVIDER/2; dj < DIVIDER/2; dj++)
				{
					var dv = new Vector2(0.5f + di, 0.5f + dj) / DIVIDER;
					if (dv.Length() > Math.Sqrt(2)) continue;
					dVectors.Add(dv);
				}
			}

			var dividedPart = 1f / (dVectors.Count * dVectors.Count);

			for (var i = -RADIUS; i <= RADIUS; ++i)
			{
				for (var j = -RADIUS; j <= RADIUS; ++j)
				{
					var pnt = new Point(i, j);

					if (pnt.Equals(Point.Zero)) continue;

					LosCell1 cell;
					if (!m_alreadyDone.TryGetValue(pnt, out cell))
					{
						cell = new LosCell1(pnt);
						m_alreadyDone.Add(pnt, cell);
					}

					foreach (var dv in dVectors)
					{
						var v = new Vector2(pnt.X, pnt.Y) + dv;
						var parentPoint = Point.Zero;
						foreach (var lineV in v.GetLineToPoints(Vector2.Zero, 0.1f))
						{
							var point = new Point((int) Math.Round(lineV.X), (int) Math.Round(lineV.Y));
							if (point.Equals(pnt)) continue;
							parentPoint = point;
							break;
						}
						LosCell1 parent;
						if (!m_alreadyDone.TryGetValue(parentPoint, out parent))
						{
							parent = new LosCell1(parentPoint);
							m_alreadyDone.Add(parentPoint, parent);
						}
						parent.Add(pnt, dividedPart, cell);
					}
				}

			}
			foreach (var cell in m_alreadyDone.Values)
			{
				cell.Normalize();
			}
		}

		public Dictionary<Point, float> GetVisibleCelss(MapCell[,] _mapCells, int _dx, int _dy)
		{
			var open = new Dictionary<LosCell1, float>();
			var dPoint = new Point(_dx,_dy);
			m_root.GetVisibleCelss(_mapCells, dPoint, open, 1f);
			return open.ToDictionary(_pair => _pair.Key.Point + dPoint, _pair => _pair.Value);
		}
	}

	internal class LosCell1
	{
		public LosCell1(Point _point)
		{
			Point = _point;
			var r = 1.2f - _point.Lenght / ((float)(LosManager1.RADIUS * Math.Sqrt(2)));
			DistanceCoefficient = Math.Min(r,1f);
		}

		public float DistanceCoefficient { get; private set; }

		public Point Point { get; private set; }

		/// <summary>
		/// 	double хранит величину - насколько та или иная ячейка видна через текущую
		/// </summary>
		public Dictionary<Tuple<Point, float>, LosCell1> Cells
		{
			get { return m_cells; }
		}

		public override string ToString()
		{
			return string.Format("{0}*{2} cells:{1}", Point, Cells.Count, DistanceCoefficient);
		}

		/// <summary>
		/// 	double хранит величину - насколько та или иная ячейка видна через текущую
		/// </summary>
		private readonly Dictionary<Tuple<Point, float>, LosCell1> m_cells = new Dictionary<Tuple<Point, float>, LosCell1>();


		public void GetVisibleCelss(MapCell[,] _mapCells, Point _dPoint, Dictionary<LosCell1, float> _open, float _visibilityCoeff)
		{
			float value;
			if (_open.TryGetValue(this, out value))
			{
				if (value >= DistanceCoefficient) return;
				value /= DistanceCoefficient;
			}

			_visibilityCoeff = Math.Min(1f, _visibilityCoeff + value) * DistanceCoefficient;
			_open[this] = _visibilityCoeff;

			if (_visibilityCoeff < 0.1) return;

			var myPnt = Point + _dPoint;
			var mapCell = _mapCells[myPnt.X, myPnt.Y];
			var childsVisible = (1f - mapCell.Opacity) * _visibilityCoeff;

			if (childsVisible<0.1) return;

			//Debug.WriteLine(childsVisible);

			var maxX = _mapCells.GetLength(0) - 1;
			var maxY = _mapCells.GetLength(1) - 1;

			foreach (var pair in Cells)
			{
				var pnt = pair.Key.Item1 + _dPoint;

				var child = pair.Value;

				if (pnt.X < 0 || pnt.X >= maxX || pnt.Y < 0 || pnt.Y >= maxY) continue;

				var childChildrenVisibles = childsVisible * pair.Key.Item2;

				if(childChildrenVisibles>_visibilityCoeff)
				{
					
				}

				child.GetVisibleCelss(_mapCells, _dPoint, _open, childChildrenVisibles);
			}
		}

		private readonly List<LosCell1> m_visibleFrom = new List<LosCell1>();

		public void Normalize()
		{
			return;
			var dict = m_visibleFrom.ToDictionary(_cell1 => _cell1, _cell1 => _cell1.Cells.Single(_pair => _pair.Value == this));
			var visibles = dict.ToArray();
			if (visibles.Length == 0) return;
			var mul = visibles[0].Value.Key.Item2;
			for (int i = 1; i < visibles.Length; i++)
			{
				mul *= visibles[i].Value.Key.Item2;
			}
			Debug.WriteLine(mul);

			var div = (float)Math.Pow(mul, 1f / visibles.Length);

			foreach (var pair in dict)
			{
				pair.Key.Replace(this, pair.Value.Key.Item2/div);
			}
		}

		public void Add(Point _pnt, float _closedByParent, LosCell1 _cell)
		{
			var have = Cells.Keys.FirstOrDefault(_tuple => _tuple.Item1 == _pnt);
			if (have != null)
			{
				_closedByParent = (float)Math.Min(have.Item2 + _closedByParent, 1f);
				Cells.Remove(have);
			}
			else
			{
				_cell.m_visibleFrom.Add(this);
			}
			Cells.Add(new Tuple<Point, float>(_pnt, _closedByParent), _cell);
		}

		public void Replace(LosCell1 _cell, float _value)
		{
			var have = Cells.Single(_pair => _pair.Value==_cell);
			Cells.Remove(have.Key);
			Cells.Add(new Tuple<Point, float>(_cell.Point, _value), _cell);
		}
	}
}
