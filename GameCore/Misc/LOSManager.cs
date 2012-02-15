using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore.Mapping;

namespace GameCore.Misc
{
	public class LosManager
	{
		const float DIVIDER = 6f;

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

			foreach (var cell in alreadyDone.Values)
			{
				cell.UpdateByDistance();
			}

			m_inOrder = alreadyDone.Values.OrderByDescending(_cell2 => _cell2.DistanceCoefficient).ToArray();
			foreach (var losCell in m_inOrder)
			{
				losCell.BuildCellIndexes(m_inOrder);
			}
		}

		public void GetVisibleCelss(MapCell[,] _mapCells, Point _dPoint, FColor _startFrom)
		{
			var maxX = _mapCells.GetLength(0) - 1;
			var maxY = _mapCells.GetLength(1) - 1;
			var cvisibles = new FColor[m_inOrder.Length];
			var visibles = new float[m_inOrder.Length];

			cvisibles[0] = _startFrom;
			visibles[0] = 1;

			for (var index = 0; index < m_inOrder.Length; index++)
			{
				var cell = m_inOrder[index];
				var visibilityCoeff = visibles[index];
				var color = cvisibles[index];

				if (visibilityCoeff < 0.01) continue;

				var myPnt = cell.Point + _dPoint;

				if (myPnt.X < 0 || myPnt.X >= maxX || myPnt.Y < 0 || myPnt.Y >= maxY) continue;

				var mapCell = _mapCells[myPnt.X, myPnt.Y];
				mapCell.Visibility = new FColor(visibilityCoeff, color);

				var transColor = index == 0 ? FColor.White : mapCell.TransparentColor;

				visibilityCoeff = transColor.A * visibilityCoeff;
				var childsColor = color.Multiply(transColor);

				if (visibilityCoeff < 0.01) continue;

				foreach (var pair in cell.CellIndexes)
				{
					var pnt = m_inOrder[pair.Key].Point + _dPoint;
					if (pnt.X < 0 || pnt.X >= maxX || pnt.Y < 0 || pnt.Y >= maxY) continue;

					visibles[pair.Key] += pair.Value * visibilityCoeff;
					cvisibles[pair.Key] = cvisibles[pair.Key].ScreenColorsOnly(childsColor);
				}
			}
		}

		public void LightCells(MapCell[,] _mapCells, Point _dPoint, FColor _fColor)
		{
			var minX = 0;
			var minY = 0;

			if (_dPoint.X< 0) return;
			if (_dPoint.Y< 0) return;

			var maxX = _mapCells.GetLength(0) - 1;
			var maxY = _mapCells.GetLength(1) - 1;

			if (_dPoint.X> maxX) return;
			if (_dPoint.Y> maxY) return;

			var cvisibles = new FColor[m_inOrder.Length];
			var visibles = new float[m_inOrder.Length];

			cvisibles[0] = _fColor;
			visibles[0] = 2f;

			if(_mapCells[_dPoint.X,_dPoint.Y].Visibility.A==0) return;

			//if (_mapCells[_dPoint.X - 1, _dPoint.Y].TransparentColor.A==0)
			//{
				
			//}

			for (var index = 0; index < m_inOrder.Length; index++)
			{
				var losCell = m_inOrder[index];
				var visibilityCoeff = visibles[index];

				if (visibilityCoeff < 0.01) continue;


				var myPnt = losCell.Point + _dPoint;

				if (myPnt.X < 0 || myPnt.X >= maxX || myPnt.Y < 0 || myPnt.Y >= maxY) continue;

				var mapCell = _mapCells[myPnt.X, myPnt.Y];

				var color = cvisibles[index];

				var transColor = index == 0 ? FColor.White : mapCell.TransparentColor;
				if (mapCell.Visibility.A > 0)
				{
					mapCell.Lighted = mapCell.Lighted.Screen(color.Multiply(visibilityCoeff));
					visibilityCoeff *= transColor.A;
					if (visibilityCoeff < 0.01) continue;
				}
				else
				{
					visibilityCoeff *= transColor.A;
					if (visibilityCoeff < 0.01) continue;
					mapCell.Lighted = mapCell.Lighted.Screen(color.Multiply(visibilityCoeff));
				}

				var childsColor = color.Multiply(transColor);


				foreach (var pair in losCell.CellIndexes)
				{
					var pnt = m_inOrder[pair.Key].Point + _dPoint;
					if (pnt.X < 0 || pnt.X >= maxX || pnt.Y < 0 || pnt.Y >= maxY) continue;

					visibles[pair.Key] += pair.Value * visibilityCoeff;
					cvisibles[pair.Key] = childsColor.Screen(cvisibles[pair.Key]);
				}
			}
		}

		public void GetVisibleCelss(LiveMap _liveMap, Point _dPoint, FColor _startFrom)
		{
			var cvisibles = new FColor[m_inOrder.Length];
			var visibles = new float[m_inOrder.Length];

			cvisibles[0] = _startFrom;
			visibles[0] = 1;

			for (var index = 0; index < m_inOrder.Length; index++)
			{
				var cell = m_inOrder[index];
				var visibilityCoeff = visibles[index];
				var color = cvisibles[index];

				if (visibilityCoeff < 0.01) continue;

				var myPnt = (cell.Point + _dPoint).Wrap(_liveMap.SizeInCells, _liveMap.SizeInCells);

				var mapCell = _liveMap.Cells[myPnt.X, myPnt.Y];
				mapCell.Visibility = new FColor(visibilityCoeff, color);

				var transColor = index == 0 ? FColor.White : mapCell.MapCell.TransparentColor;

				visibilityCoeff = transColor.A * visibilityCoeff;
				var childsColor = color.Multiply(transColor);

				if (visibilityCoeff < 0.01) continue;

				foreach (var pair in cell.CellIndexes)
				{
					visibles[pair.Key] += pair.Value * visibilityCoeff;
					cvisibles[pair.Key] = cvisibles[pair.Key].ScreenColorsOnly(childsColor);
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
			//dc += (1f - dc)*0.5f;

			DistanceCoefficient = Math.Min(dc, 1f);
		}

		public float DistanceCoefficient { get; private set; }

		public Point Point { get; private set; }

		public Dictionary<LosCell, float> Cells { get; private set; }
		public Dictionary<int, float> CellIndexes { get; private set; }

		public void BuildCellIndexes(LosCell[] _inOrder)
		{
			CellIndexes = new Dictionary<int, float>();
			foreach (var cell in Cells)
			{
				CellIndexes.Add(Array.IndexOf(_inOrder, cell.Key), cell.Value);
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
