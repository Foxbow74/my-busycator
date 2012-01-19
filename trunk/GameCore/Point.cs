namespace GameCore
{
	public class Point
	{
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