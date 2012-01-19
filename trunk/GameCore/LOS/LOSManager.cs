using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameCore.LOS
{
	public class LosManager
	{
		private readonly LosCell m_root;

		public LosManager(MapCell[,] _mapCells)
		{
			var screenWidth = _mapCells.GetLength(0);
			var screenHeght = _mapCells.GetLength(1);

			m_root = new LosCell(Point.Zero);
			var alreadyDone = new Dictionary<Point, LosCell> { { Point.Zero, m_root } };
			var radius = Math.Max(screenWidth, screenHeght) / 2;

			var dRo = 1.0/(2 * Math.PI * radius);
			for (var ro = 0.0; ro < Math.PI*2; ro += dRo)
			{
				var x = Math.Sin(ro);
				var y = Math.Cos(ro);

				if (x<0&&y<0)
				{
					
				}

				m_root.Process(1, x, y, alreadyDone, radius, 0.999);
			}

			foreach (var pair in alreadyDone)
			{
				pair.Value.Clear(screenWidth / 2, screenHeght / 2, pair.Key);
			}
		}

		public Dictionary<Point, double> GetVisibleCelss(MapCell[,] _mapCells, int _dx, int _dy)
		{
			var alreadyDone = new Dictionary<Point, double>();
			m_root.GetVisibleCelss(_mapCells, _dx, _dy, alreadyDone);
			return alreadyDone;
		}
	}

	class LosCell
	{
		private readonly Point m_point;
		private static int N = 0;
		private int m_n = 0;

		/// <summary>
		/// double хранит величину - насколько та или иная ячейка видна через текущую
		/// </summary>
		readonly Dictionary<Tuple<Point, double>, LosCell> m_cells = new Dictionary<Tuple<Point, double>, LosCell>();

		public LosCell(Point _point)
		{
			m_point = _point;
			N++;
			m_n = N;
		}

		//public Dictionary<Point, LosCell>  Cells{get{return m_cells;}}

		public void Process(double _fromR, double _x, double _y, Dictionary<Point, LosCell> _alreadyDone, double _radius, double _visibility)
		{
			if (_fromR >= _radius) return;

			//Debug.WriteLine(m_n + " / " + _fromR);

			var rx = _x * _fromR;
			var ry = _y * _fromR;

			var fromCenter = m_point.QLenght;

			var dists = new Dictionary<double, Point>();
			for (var vx = Math.Round(rx - 2); vx < rx + 2; ++vx)
			{
				for (var vy = Math.Round(ry - 2); vy < ry + 2; ++vy)
				{

					var dist = new Point((int)vx, (int)vy).QLenght;// Math.Sqrt(vx*vx + vy*vy);
					if(fromCenter >= dist)
					{
						//Если выбранная точка ближе к центру чем текущая
						continue;
					}
					//varr d = Math.Sqrt((vx - rx) * (vx - rx) + (vy - ry) * (vy - ry));
					var d = Math.Max(Math.Abs(vx - rx), Math.Abs(vy - ry))/2;
					if (d < 1)
					{
						dists[d] = new Point((int)vx, (int)vy);
					}
				}
			}

			foreach (var pair in dists)
			{
				var pnt = pair.Value;
				var visibility = (1 - pair.Key) * _visibility;
				//var visibility = _visibility;
				if (visibility < 0.1) continue;
				LosCell losCell;
				if (!_alreadyDone.TryGetValue(pnt, out losCell))
				{
					losCell = new LosCell(pnt);
					_alreadyDone.Add(pnt, losCell);
				}

				var have = m_cells.Keys.FirstOrDefault(_tuple => _tuple.Item1.Equals(pnt));
				var key = new Tuple<Point, double>(pnt, visibility);
				if (losCell != this)
				{
					if (have != null)
					{
						if (have.Item2 < visibility)
						{
							m_cells.Remove(have);
							m_cells.Add(key, losCell);
						}
					}
					else
					{
						m_cells.Add(key, losCell);
					}
				}
				//if(pnt.X!=0 && pnt.Y!=0 && visibility>0.5)
				//{
				//    Debug.WriteLine(pnt);
				//}
				losCell.Process(_fromR + 1, pnt.X / pnt.Lenght, pnt.Y / pnt.Lenght, _alreadyDone, _radius, visibility);
			}
		}

		public void GetVisibleCelss(MapCell[,] _mapCells, int _dx, int _dy, Dictionary<Point, double> _alreadyDone)
		{
			foreach (var pair in m_cells)
			{
				var pnt = new Point(pair.Key.Item1.X + _dx, pair.Key.Item1.Y + _dy);

				double visibility;
				if (_alreadyDone.TryGetValue(pnt, out visibility))
				{
					if(visibility>=pair.Key.Item2) continue;
				}
				_alreadyDone[pnt] = pair.Key.Item2;

				if (!_mapCells[pnt.X, pnt.Y].Terrain.IsPassable())
				{
					continue;
				}
				
				pair.Value.GetVisibleCelss(_mapCells, _dx, _dy, _alreadyDone);
			}
		}

		public void Clear(int _halfWidth, int _halfHeght, Point _key)
		{
			var array = m_cells.Keys.ToArray();
			foreach (var tuple in array)
			{
				var p = tuple.Item1;
				if (Math.Abs(p.X) >= _halfWidth || Math.Abs(p.Y) >= _halfHeght)
				{
					m_cells.Remove(tuple);
				}
			}
		}
	}
}
