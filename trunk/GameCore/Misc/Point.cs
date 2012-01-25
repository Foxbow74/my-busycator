using System;
using System.Collections.Generic;

namespace GameCore.Misc
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

		public IEnumerable<Point> NearestPoints
		{
			get
			{
				for (var i = -1; i <= 1; ++i)
				{
					for(var j=-1;j<=1;++j)
					{
						if(i==0 && j==0) continue;
						yield return new Point(X+i,Y+j);
					}
				}
			}
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
			return GetHashCode() == _obj.GetHashCode();
		}

		#region overrides

		public static Point operator +(Point _a, Point _b)
		{
			return new Point(_a.X + _b.X, _a.Y + _b.Y);
		}

		public static Point operator -(Point _a, Point _b)
		{
			return new Point(_a.X - _b.X, _a.Y - _b.Y);
		}

		public static bool operator ==(Point _a, Point _b)
		{
			if (ReferenceEquals(_a, _b))
			{
				return true;
			}

			if (((object)_a == null) && ((object)_b == null))
			{
				return true;
			}

			if (((object)_a == null) || ((object)_b == null))
			{
				return false;
			}

			return _a.X == _b.X && _a.Y == _b.Y;
		}

		public static bool operator !=(Point _a, Point _b)
		{
			if (ReferenceEquals(_a, _b))
			{
				return false;
			}

			if (((object)_a == null) && ((object)_b == null))
			{
				return false;
			} 
			
			if (((object)_a == null) || ((object)_b == null))
			{
				return true;
			}

			return _a.X != _b.X || _a.Y != _b.Y;
		}

		#endregion

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
			var sx = Math.Sign(_point.X - X);
			var sy = Math.Sign(_point.Y - Y);

			var a = 0;

			if (onX)
			{
				var j = Y;
				for (var i = X; i != _point.X; i += sx)
				{
					yield return new Point(i, j);
					a += min;
					if (a < max) continue;
					j += sy;
					a = a % max;
				}
			}
			else
			{
				var i = X;
				for (var j = Y; j != _point.Y; j += sy)
				{
					yield return new Point(i, j);
					a += min;
					if (a < max) continue;
					i += sx;
					a = a % max;
				}
			}

			yield return _point;
		}
	}
}