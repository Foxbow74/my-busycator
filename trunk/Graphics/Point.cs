using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Graphics
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
				for (int i = -1; i <= 1; ++i)
				{
					for(int j=-1;j<=1;++j)
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

		public float GetDistanceToVector(Point _point)
		{
			var lineVector = new Vector2(_point.X,_point.Y);
			lineVector.Normalize();

			var myVector = new Vector2(X, Y);

			float distanceAlongLine = Vector2.Dot(myVector, lineVector) - Vector2.Dot(Vector2.Zero, lineVector);
			Vector2 nearestPoint;
			if (distanceAlongLine < 0)
			{
				nearestPoint = Vector2.Zero;
			}
			else
			{
				nearestPoint = distanceAlongLine * lineVector;
			}

			return Vector2.Distance(nearestPoint, myVector);
		}
	}
}