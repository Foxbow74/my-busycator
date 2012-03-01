using System;
using System.Collections.Generic;

namespace GameCore.Misc
{
	public class Rct
	{
		public int Left { get { return LeftTop.X; } }
		public int Right { get { return RightBottom.X; } }

		public int Top { get { return LeftTop.Y; } }
		public int Bottom { get { return RightBottom.Y; } }

		public Point LeftTop { get; private set; }
		public Point RightBottom { get; private set; }

		public int Width { get { return RightBottom.X - LeftTop.X + 1; } }
		public int Height { get { return RightBottom.Y - LeftTop.Y + 1; } }

		public Rct(Point _leftTop, Point _rightBottom)
		{
			if(_leftTop.X>_rightBottom.X || _leftTop.Y>_rightBottom.Y)
			{
				throw new ArgumentException();
			}

			LeftTop = _leftTop;
			RightBottom = _rightBottom;
		}

		public Rct(Point _leftTop, int _width, int _height)
			: this(_leftTop, _leftTop + new Point(_width - 1, _height - 1))
		{
		}
		
		public Rct(int _left, int _top, int _width, int _height)
			: this(new Point(_left, _top), new Point(_left, _top) + new Point(_width - 1, _height - 1))
		{
		}

		public Rct Inflate(int _x, int _y)
		{
			return new Rct(LeftTop - new Point(_x, _y), RightBottom + new Point(_x, _y));
		}

		public Rct Offset(int _x, int _y)
		{
			return new Rct(LeftTop + new Point(_x, _y), RightBottom + new Point(_x, _y));
		}

		public bool ContainsEx(Point _point)
		{
			var result = Left <= _point.X && Top <= _point.Y && Bottom >= _point.Y && Right >= _point.X;
			return result;
		}

		public IEnumerable<Point> AllPoints
		{
			get
			{
				for (var i = Left; i < Right; ++i)
				{
					for (var j = Top; j < Bottom; ++j)
					{
						yield return new Point(i, j);
					}
				}
			}
		}

		public IEnumerable<Point> BorderPoints
		{
			get
			{
				for (var i = Left; i < Right; ++i)
				{
					yield return new Point(i, Top);
					yield return new Point(i, Bottom);
				}
				for (var j = Top + 1; j < Bottom - 1; ++j)
				{
					yield return new Point(Left, j);
					yield return new Point(Right, j);
				}
			}
		}

		#region overrides

		public static Rct operator *(Rct _a, int _c)
		{
			return new Rct(_a.LeftTop * _c, _a.RightBottom * _c);
		}

		#endregion
	}
}
