using System;
using System.Collections.Generic;

namespace GameCore
{
	public class Point
	{
		public static Point Zero = new Point();

		public int X { get; set; }
		public int Y { get; set; }

		public Point(int _x, int _y)
		{
			X = _x;
			Y = _y;
		}

		public Point()
		{
			X = 0;
			Y = 0;
		}

		public double Lenght
		{
			get { return Math.Sqrt(X * X + Y * Y); }
		}

		public double QLenght
		{
			get { return Math.Max(Math.Abs(X), Math.Abs(Y)); }
		}

		public double GetDistTill(Point _point)
		{
			return Math.Sqrt((X - _point.X) * (X - _point.X) + (Y - _point.Y)*(Y - _point.Y));
		}

		public override string ToString()
		{
			return string.Format("P({0};{1})", X,Y);
		}

		public override bool Equals(object _obj)
		{
			//var pnt = (Point) _obj;

			//return pnt.X == X && pnt.Y == Y;
			return GetHashCode() == _obj.GetHashCode();
		}

		public override int GetHashCode()
		{
			return X^(Y<<16);
		}

		public IEnumerable<Point> GetLineToPoints(Point _point)
		{
			var lx = Math.Abs(_point.X - X);
			var ly = Math.Abs(_point.Y - Y);
			var onX = lx >= ly;
			var max = onX ? lx : ly;
			var min = onX ? ly : lx;

			if (lx == 0) lx = 1;
			if (ly == 0) ly = 1;


			var dC = Math.Round(onX ? ((double)lx / ly) : ((double)ly / lx));
			var dD = onX ? lx % ly : ly % lx;

			var sx = Math.Sign(_point.X - X);
			var sy = Math.Sign(_point.Y - Y);

			//var s = onX?sx:sy;

			var a = 0;

			if (onX)
			{
				var j = Y;
				for (int i = X; i != _point.X; i += sx)
				{
					yield return new Point(i, j);
					a += min;
					if (a >= max)
					{
						j += sy;
						a = a % max;
					}
				}
			}
			else
			{
				var i = X;
				for (int j = Y; j != _point.Y; j += sy)
				{
					yield return new Point(i, j);
					a += min;
					if (a >= max)
					{
						i += sx;
						a = a % max;
					}
				}
			}

			yield return _point;
		}
	}
}