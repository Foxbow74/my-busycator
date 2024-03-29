﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore.Misc
{
	public class Point
	{
		private static Point[] m_nearestDPoints;
		public readonly int X;
		public readonly int Y;
		static Point()
		{
			Zero = new Point(0,0);

			var blockPoints = new List<Point>();
			for (var i = 0; i < Constants.MAP_BLOCK_SIZE; i++)
			{
				for (var j = 0; j < Constants.MAP_BLOCK_SIZE; j++)
				{
					blockPoints.Add(new Point(i, j));
				}
			}
			AllBlockPoints = blockPoints.ToArray();
		}

		public Point(int _x, int _y)
		{
			X = _x;
			Y = _y;
		}

		public static Point Zero { get; private set; }

		public float Lenght { get { return (float) Math.Sqrt((float) X*X + (float) Y*Y); } }

		public float QLenght { get { return Math.Max(Math.Abs(X), Math.Abs(Y)); } }

		public IEnumerable<Point> NearestPoints
		{
			get
			{
				return NearestDPoints.Select(_point => _point + this);
			}
		}

		public static Point[] AllBlockPoints { get; private set; }

		public IEnumerable<Point> GetAllBlockPoints()
		{
			foreach (var t in AllBlockPoints)
			{
				yield return t + this;
			}
		}


		public static Point[] NearestDPoints
		{
			get
			{
				if (m_nearestDPoints==null)
				{
					var list = new List<Point>();

					for (var i = -1; i <= 1; ++i)
					{
						for (var j = -1; j <= 1; ++j)
						{
							list.Add(new Point(i, j));
						}
					}
					m_nearestDPoints = list.ToArray();
				}
				return m_nearestDPoints;
			}
		}

		public IEnumerable<Point> AllNeighbours
		{
			get
			{
				yield return new Point(X - 1, Y - 1);
				yield return new Point(X, Y - 1);
				yield return new Point(X + 1, Y - 1);
				yield return new Point(X - 1, Y);
				yield return new Point(X + 1, Y - 1);
				yield return new Point(X - 1, Y + 1);
				yield return new Point(X, Y + 1);
				yield return new Point(X + 1, Y + 1);
			}
		}

		public float GetDistTill(Point _point) { return (float) Math.Sqrt((X - _point.X)*(X - _point.X) + (Y - _point.Y)*(Y - _point.Y)); }

		public override string ToString() { return string.Format("P({0};{1})", X, Y); }

		public override bool Equals(object _obj) { return GetHashCode() == _obj.GetHashCode(); }

		public override int GetHashCode() { return X ^ (Y << 16); }

		public IEnumerable<Point> GetLineToPoints(Point _point)
		{
			double lx = Math.Abs(_point.X - X);
			double ly = Math.Abs(_point.Y - Y);

			var max = Math.Max(lx, ly);

			double dx = Math.Sign(_point.X - X);
			double dy = Math.Sign(_point.Y - Y);
			
			double x = X;
			double y = Y;

			if (lx > ly)
			{
				dy *= ly/lx;
			}
			else if ((lx < ly))
			{
				dx *= lx/ly;
			}

			yield return this;
			for (var i = 0; i < max; ++i)
			{
				x += dx;
				y += dy;
				yield return new Point((int) Math.Round(x), (int) Math.Round(y));
			}
		}

		public float GetDistanceToVector(Point _point)
		{
			var lineVector = new Vector2(_point.X, _point.Y);
			lineVector.Normalize();

			var myVector = new Vector2(X, Y);

			var distanceAlongLine = Vector2.Dot(myVector, lineVector) - Vector2.Dot(Vector2.Zero, lineVector);
			Vector2 nearestPoint;
			if (distanceAlongLine < 0)
			{
				nearestPoint = Vector2.Zero;
			}
			else
			{
				nearestPoint = distanceAlongLine*lineVector;
			}

			return Vector2.Distance(nearestPoint, myVector);
		}

		public Point Wrap(int _width, int _height) { return new Point((X + 100*_width)%_width, (Y + 100*_height)%_height); }

		public Point Clone() { return new Point(X, Y); }

		public static Point Parse(string _s)
		{
			var ss = _s.Split(new[] {','}, StringSplitOptions.None);
			return new Point(int.Parse(ss[0]), int.Parse(ss[1]));
		}

		public IEnumerable<Point> GetSpiral(int _size)
		{
			yield return this;
			for (var i = 1; i < _size; i++)
			{
				foreach (var point in new Rct(-i,-i,i*2,i*2).BorderPoints)
				{
					yield return point;
				}
			}
		}

		#region overrides

		public static Point operator +(Point _a, Point _b) { return new Point(_a.X + _b.X, _a.Y + _b.Y); }

		public static Point operator -(Point _a, Point _b) { return new Point(_a.X - _b.X, _a.Y - _b.Y); }

		public static Point operator *(Point _a, float _c) { return new Point((int) Math.Round(_a.X*_c), (int) Math.Round(_a.Y*_c)); }

		public static Point operator *(Point _a, Point _b) { return new Point(_a.X*_b.X, _a.Y*_b.Y); }

		public static Point operator /(Point _a, int _c) { return new Point(_a.X/_c, _a.Y/_c); }

		public static bool operator ==(Point _a, Point _b)
		{
			if (ReferenceEquals(_a, _b))
			{
				return true;
			}

			//if (((object) _a == null) && ((object) _b == null))
			//{
			//	return true;
			//}

			if (((object) _a == null) || ((object) _b == null))
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

			//if (((object) _a == null) && ((object) _b == null))
			//{
			//	return false;
			//}

			if (((object) _a == null) || ((object) _b == null))
			{
				return true;
			}

			return _a.X != _b.X || _a.Y != _b.Y;
		}

		#endregion
	}
}