using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Community.CsharpSqlite.SQLiteClient;
using GameCore;
using GameCore.Misc;
using UnsafeUtils;

namespace ConsoleTests
{
	class Program
	{
		const int SIZE = 64;
		static void Main(string[] _args)
		{

			var lst = new List<Tuple<int, int>>();
			var ll = 10;
			for (var i = -ll; i < ll; ++i)
			{
				for (var j = -ll; j < ll; ++j)
				{
					lst.Add(new Tuple<int, int>(i, j));
				}
			}


			var faPoints = new List<Point>();
			var points = new List<OldPoint>();
			{
				for (var k = 0; k <20; ++k)
				{
					Debug.WriteLine(k);
					using (new Profiler("Point"))
					{
						int ee = 0;
						foreach (var tuple in lst)
						{
							faPoints.Add(new Point(tuple.Item1, tuple.Item2));
						}
						foreach (var point in faPoints)
						{
							foreach (var pnt in faPoints)
							{
								if (pnt== point)
								{
									ee++;
								}
							}
						}
					}

					using (new Profiler("OldPoint"))
					{
						int ee = 0;
						foreach (var tuple in lst)
						{
							points.Add(new OldPoint(tuple.Item1, tuple.Item2));
						}
						foreach (var point in points)
						{
							foreach (var pnt in points)
							{
								if (pnt==point)
								{
									ee++;
								}
							}
						}
					}

				}
			}

			Profiler.Report();
			
		}

		public static int Method<T>(List<Tuple<int, int>> _lst, List<T> _points, Func<int, int, T> _func, Func<T, T, bool> _eq) where T : struct
		{
			int ee = 0;
			foreach (var tuple in _lst)
			{
				_points.Add(_func(tuple.Item1, tuple.Item2));
			}
			foreach (var point in _points)
			{
				foreach (var pnt in _points)
				{
					if (_eq(pnt, point))
					{
						ee++;
					}
				}
			}
			return ee;
		}

		public static int MethodFor<T>(List<Tuple<int, int>> _lst, List<T> _points, Func<int, int, T> _func, Func<T, T, bool> _eq) where T : struct
		{
			int ee = 0;
			for (int index = 0; index < _lst.Count; index++)
			{
				var tuple = _lst[index];
				_points.Add(_func(tuple.Item1, tuple.Item2));
			}
			for (int index = 0; index < _points.Count; index++)
			{
				var point = _points[index];
				for (int i = 0; i < _points.Count; i++)
				{
					var pnt = _points[i];
					if (_eq(pnt, point))
					{
						ee++;
					}
				}
			}
			return ee;
		}

		public static int MethodAr<T>(List<Tuple<int, int>> _lst, List<T> _points, Func<int, int, T> _func, Func<T, T, bool> _eq) where T : struct
		{
			int ee = 0;
			foreach (var tuple in _lst)
			{
				_points.Add(_func(tuple.Item1, tuple.Item2));
			}
			var arr = _points.ToArray();

			foreach (var point in arr)
			{
				foreach (var pnt in arr)
				{
					if (_eq(pnt, point))
					{
						ee++;
					}
				}
			}
			return ee;
		}

		public static int MethodArFor<T>(List<Tuple<int, int>> _lst, List<T> _points, Func<int, int, T> _func, Func<T, T, bool> _eq) where T : struct
		{
			int ee = 0;
			for (int index = 0; index < _lst.Count; index++)
			{
				var tuple = _lst[index];
				_points.Add(_func(tuple.Item1, tuple.Item2));
			}
			var arr = _points.ToArray();

			for (int i = 0; i < arr.Length; i++)
			{
				var point = arr[i];
				for (int index = 0; index < arr.Length; index++)
				{
					var pnt = arr[index];
					if (_eq(pnt, point))
					{
						ee++;
					}
				}
			}
			return ee;
		}
		public static int MethodLq<T>(List<Tuple<int, int>> _lst, List<T> _points, Func<int, int, T> _func, Func<T, T, bool> _eq) where T : struct
		{
			_points.AddRange(_lst.Select(_tuple => _func(_tuple.Item1, _tuple.Item2)));
			var arr = _points.ToArray();
			return (from point in arr from pnt in arr where _eq(pnt, point) select point).Count();
		}
	}


}

public class OldPoint
{
	static OldPoint()
	{
		Zero = new OldPoint(0, 0);
		Null = new OldPoint(Int32.MaxValue, Int32.MaxValue);
	}

	public OldPoint(int _x, int _y)
	{
		X = _x;
		Y = _y;
	}

	public static OldPoint Zero { get; private set; }

	public static OldPoint Null { get; private set; }

	public bool IsNull
	{
		get { return X == Int32.MaxValue; }

	}
	public bool IsNotNull
	{
		get { return X != Int32.MaxValue; }

	}

	public int X;
	public int Y;

	public float Lenght { get { return (float)Math.Sqrt((float)X * X + (float)Y * Y); } }

	public float QLenght { get { return Math.Max(Math.Abs(X), Math.Abs(Y)); } }

	public IEnumerable<OldPoint> NearestPoints
	{
		get
		{
			var p = this;
			return NearestDPoints.Select(_point => _point + p);
		}
	}


	private static OldPoint[] m_nearestDPoints = null;

	public static OldPoint[] NearestDPoints
	{
		get
		{
			if (m_nearestDPoints == null)
			{
				var list = new List<OldPoint>();

				for (var i = -1; i <= 1; ++i)
				{
					for (var j = -1; j <= 1; ++j)
					{
						list.Add(new OldPoint(i, j));
					}
				}
				m_nearestDPoints = list.ToArray();
			}
			return m_nearestDPoints;
		}
	}

	public IEnumerable<OldPoint> AllNeighbours
	{
		get
		{
			yield return new OldPoint(X - 1, Y - 1);
			yield return new OldPoint(X, Y - 1);
			yield return new OldPoint(X + 1, Y - 1);
			yield return new OldPoint(X - 1, Y);
			yield return new OldPoint(X + 1, Y - 1);
			yield return new OldPoint(X - 1, Y + 1);
			yield return new OldPoint(X, Y + 1);
			yield return new OldPoint(X + 1, Y + 1);
		}
	}

	public float GetDistTill(OldPoint _point) { return (float)Math.Sqrt((X - _point.X) * (X - _point.X) + (Y - _point.Y) * (Y - _point.Y)); }

	public override string ToString() { return string.Format("P({0};{1})", X, Y); }

	public override bool Equals(object _obj) { return GetHashCode() == _obj.GetHashCode(); }

	public override int GetHashCode() { return X ^ (Y << 16); }

	public IEnumerable<OldPoint> GetLineToPoints(OldPoint _point)
	{
		double lx = Math.Abs(_point.X - X);
		double ly = Math.Abs(_point.Y - Y);

		var max = Math.Max(lx, ly);

		double dx = Math.Sign(_point.X - X);
		double dy = Math.Sign(_point.Y - Y);
		;

		double x = X;
		double y = Y;

		if (lx > ly)
		{
			dy *= ly / lx;
		}
		else if ((lx < ly))
		{
			dx *= lx / ly;
		}

		yield return this;
		for (var i = 0; i < max; ++i)
		{
			x += dx;
			y += dy;
			yield return new OldPoint((int)Math.Round(x), (int)Math.Round(y));
		}
	}


	public OldPoint Wrap(int _width, int _height) { return new OldPoint((X + 100 * _width) % _width, (Y + 100 * _height) % _height); }

	#region overrides

	public static OldPoint operator +(OldPoint _a, OldPoint _b) { return new OldPoint(_a.X + _b.X, _a.Y + _b.Y); }

	public static OldPoint operator -(OldPoint _a, OldPoint _b) { return new OldPoint(_a.X - _b.X, _a.Y - _b.Y); }

	public static OldPoint operator *(OldPoint _a, float _c) { return new OldPoint((int)Math.Round(_a.X * _c), (int)Math.Round(_a.Y * _c)); }

	public static OldPoint operator *(OldPoint _a, OldPoint _b) { return new OldPoint(_a.X * _b.X, _a.Y * _b.Y); }

	public static OldPoint operator /(OldPoint _a, int _c) { return new OldPoint(_a.X / _c, _a.Y / _c); }

	public static bool operator ==(OldPoint _a, OldPoint _b)
	{
		return _a.X == _b.X && _a.Y == _b.Y;
	}

	public static bool operator !=(OldPoint _a, OldPoint _b)
	{
		return _a.X != _b.X || _a.Y != _b.Y;
	}

	#endregion
}
