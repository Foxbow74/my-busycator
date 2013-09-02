using System;
using System.Drawing;

namespace Shader
{
	public struct Vector
	{
		PointF _val;

		public Vector(float x, float y)
		{
			_val = new PointF(x, y);
		}

		public Vector(PointF p1, PointF p2)
		{
			_val = new PointF(p2.X - p1.X, p2.Y - p1.Y);
		}

		public static Vector Empty
		{
			get
			{
				return new Vector(0,0);
			}
		}

		public float Length
		{
			get
			{
				return (float)Math.Sqrt(_val.X * _val.X + _val.Y * _val.Y);
			}
		}

		public Vector Single
		{
			get
			{
				return this/Length;
			}
		}

		public float X
		{
			get
			{
				return _val.X;
			}
		}

		public float Y
		{
			get
			{
				return _val.Y;
			}
		}

		public static Vector operator +(Vector a, Vector b)
		{
			return new Vector(a.X + b.X, a.Y + b.Y);
		}

		public static float operator *(Vector a, Vector b)
		{
			return a.X * b.X + a.Y * b.Y;
		}

		public static Vector operator *(Vector a, float c)
		{
			return new Vector(a.X * c, a.Y * c);
		}

		public static Vector operator /(Vector a, float c)
		{
			return new Vector(a.X / c, a.Y / c);
		}

		public static implicit operator PointF (Vector a)
		{
			return a._val;
		}
	}
}
