using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Mapping;

namespace GameCore.Misc
{
	public class LosManager
	{
		private const float VISIBILITY_THRESHOLD = 1f / 255;

		const float DIVIDER = 10f;

		private readonly LosCell[] m_inOrder;
		private readonly LosCell m_root;

		public LosManager(int _radius)
		{
			m_root = new LosCell(Point.Zero, _radius);
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

			for (var i = _radius; i >= -_radius; --i)
			{
				for (var j = -_radius; j <= _radius; ++j)
				{
					var pnt = new Point(i, j);

					if (pnt.Equals(Point.Zero)) continue;

					LosCell cell;
					if (!alreadyDone.TryGetValue(pnt, out cell))
					{
						cell = new LosCell(pnt, _radius);
						if (cell.Point.Lenght > _radius)
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
							if (point.Equals(pnt) || point.Lenght > _radius) continue;
							parentPoint = point;
							break;
						}
						LosCell parent;
						if (!alreadyDone.TryGetValue(parentPoint, out parent))
						{
							parent = new LosCell(parentPoint, _radius);
							alreadyDone.Add(parentPoint, parent);
						}
						parent.Add(pnt, dividedPart, cell);
					}
				}
			}

			m_inOrder = alreadyDone.Values.OrderByDescending(_cell => _cell.DistanceCoefficient).ToArray();

			Action<Point, Point, Point> act = (_p1, _p2, _p3) =>
			{
				alreadyDone[_p1].DistanceCoefficient = (alreadyDone[_p2].DistanceCoefficient +
														alreadyDone[_p3].DistanceCoefficient) / 2;
			};

			for (var i = 1; i < _radius; i++)
			{
				act(new Point(0, i), new Point(-1, i), new Point(1, i));
				act(new Point(0, -i), new Point(-1, -i), new Point(1, -i));
				act(new Point(i, 0), new Point(i, -1), new Point(i, 1));
				act(new Point(-i, 0), new Point(-i, -1), new Point(-i, 1));
			}

			foreach (var losCell in m_inOrder)
			{
				losCell.BuildCellIndexes(m_inOrder);
			}


			//var min = 1f;
			//var max = 0f;

			//for (var index = 0; index < m_inOrder.Length; index++)
			//{
			//    var visibleBy = m_inOrder.Where(_cell => _cell.CellIndexes.ContainsKey(index)).Select(_cell => _cell.CellIndexes[index]).Sum();
			//    max = Math.Max(visibleBy, max);
			//    min = Math.Min(visibleBy, min);
			//}

			//foreach (var losCell in m_inOrder)
			//{
			//    losCell.Expand(min, max);
			//}

			////for (var index = 0; index < m_inOrder.Length; index++)
			////{
			////    var visibleBy = m_inOrder.Where(_cell => _cell.CellIndexes.ContainsKey(index)).Select(_cell => _cell.CellIndexes[index]).Sum();
			////    Debug.WriteLine(visibleBy);
			////}
			////foreach (var i in distAndVisibility.Keys.OrderBy(_i => _i))
			////{
			//	System.Diagnostics.Debug.WriteLine(i + "\t" + distAndVisibility[i]);
			//}

			foreach (var losCell in m_inOrder)
			{
				losCell.BuildCellIndexes(m_inOrder);
			}

		}

		public void SetVisibleCelss(LiveMap _liveMap, Point _dPoint, FColor _startFrom)
		{
			var cvisibles = new FColor[m_inOrder.Length];
			var visibles = new float[m_inOrder.Length];

			cvisibles[0] = _startFrom;
			visibles[0] = 1;

			for (var index = 0; index < m_inOrder.Length; index++)
			{
				var cap = float.MaxValue;

				var visibilityCoeff = visibles[index];
				var color = cvisibles[index];

				if (visibilityCoeff < VISIBILITY_THRESHOLD) continue;

				var losCell = m_inOrder[index]; 
				var myPnt = (losCell.Point + _dPoint).Wrap(_liveMap.SizeInCells, _liveMap.SizeInCells);

				var liveCell = _liveMap.Cells[myPnt.X, myPnt.Y];
				liveCell.Visibility = new FColor(visibilityCoeff, color);
				var transColor = index == 0 ? FColor.White : liveCell.MapCell.TransparentColor;

				visibilityCoeff = transColor.A * visibilityCoeff;
				var childsColor = color.Multiply(transColor);

				if (visibilityCoeff < VISIBILITY_THRESHOLD) continue;

				foreach (var pair in losCell.CellIndexes)
				{
					visibles[pair.Key] += pair.Value * visibilityCoeff;
					cvisibles[pair.Key] = cvisibles[pair.Key].ScreenColorsOnly(childsColor);
				}
			}
		}

		public void LightCells(LiveMap _liveMap, Point _dPoint, FColor _fColor)
		{
			var cvisibles = new FColor[m_inOrder.Length];
			var power = new float[m_inOrder.Length];
			var childVisibility = new float[m_inOrder.Length];

			cvisibles[0] = new FColor(1f, _fColor);
			power[0] = _fColor.A;
			childVisibility[0] = 1;

			for (var index = 0; index < m_inOrder.Length; index++)
			{
				var losCell = m_inOrder[index];
	
				var powerCoeff = power[index];

				if (powerCoeff < 0.01) continue;

				var myPnt = (losCell.Point + _dPoint).Wrap(_liveMap.SizeInCells, _liveMap.SizeInCells);

				var liveCell = _liveMap.Cells[myPnt.X, myPnt.Y];

				var color = cvisibles[index];

				var transColor = index == 0 ? FColor.White : liveCell.MapCell.TransparentColor;
				if (childVisibility[index] > 0)
				{
					if (transColor.A==0)
					{
						
					}
					liveCell.Lighted = liveCell.Lighted.Screen(color.Multiply(powerCoeff));
					powerCoeff *= transColor.A;
					if (powerCoeff < 0.01) continue;
				}
				else
				{
					powerCoeff *= transColor.A;
					if (powerCoeff < 0.01) continue;
					liveCell.Lighted = liveCell.Lighted.Screen(color.Multiply(powerCoeff));
				}

				var childsColor = color.Multiply(transColor);


				foreach (var pair in losCell.CellIndexes)
				{
					power[pair.Key] += pair.Value * powerCoeff;
					cvisibles[pair.Key] = childsColor.Screen(cvisibles[pair.Key]);
					childVisibility[pair.Key] = pair.Value * liveCell.Visibility.A;
				}
			}
		}
	}

	class LosCell
	{
		public LosCell(Point _point, int _radius)
		{
			Cells = new Dictionary<LosCell, float>();
			Point = _point;
			var r = _point.Lenght / _radius;
			var fi = Math.Asin(r);
			var dc = (float)Math.Cos(fi);
			DistanceCoefficient = Math.Min(dc, 1f);
		}

		public float DistanceCoefficient { get; set; }

		public Point Point { get; private set; }

		public Dictionary<LosCell, float> Cells { get; private set; }
		public Dictionary<int, float> CellIndexes { get; private set; }

		public void BuildCellIndexes(LosCell[] _inOrder)
		{
			CellIndexes = new Dictionary<int, float>();
			foreach (var cell in Cells)
			{
				CellIndexes.Add(Array.IndexOf(_inOrder, cell.Key), cell.Value*cell.Key.DistanceCoefficient);
			}
		}

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

		public void Expand(float _min, float _max)
		{
			var v = 1f / (_max - _min);
			DistanceCoefficient = (DistanceCoefficient - _min)*v;
		}
	}
}
