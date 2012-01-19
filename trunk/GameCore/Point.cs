using System;

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
	}
}