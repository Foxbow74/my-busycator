using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace GameCore.Misc
{
	public static class Util
	{
		public static Dictionary<TEnum, TAttribute> Fill<TEnum, TAttribute>() where TAttribute : Attribute
		{
			var result = new Dictionary<TEnum, TAttribute>();
			foreach (
				var field in typeof (TEnum).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
			{
				result[(TEnum) field.GetValue(null)] = field.GetCustomAttributes(true).OfType<TAttribute>().Single();
			}
			return result;
		}

		public static T As<TSource, T>(this TSource _d) where T : TSource
		{
			return (T) _d;
		}

		public static Color Lerp(this Color _color1, Color _color2, float _f)
		{
			var color = Color.FromArgb(
				(int)MathHelper.Lerp(_color1.A, _color2.A, _f),
				(int)MathHelper.Lerp(_color1.R, _color2.R, _f),
				(int)MathHelper.Lerp(_color1.G, _color2.G, _f),
				(int)MathHelper.Lerp(_color1.B, _color2.B, _f));
			return color;
		}

		public static Color LerpColorsOnly(this Color _color1, Color _color2, float _f)
		{
			var color = Color.FromArgb(
				_color1.A,
				(int)MathHelper.Lerp(_color1.R, _color2.R, _f),
				(int)MathHelper.Lerp(_color1.G, _color2.G, _f),
				(int)MathHelper.Lerp(_color1.B, _color2.B, _f));
			return color;
		}

		public static Color Multiply(this Color _color, float _f)
		{
			Func<int, int> func = _i => (int)Math.Min(_i * _f, 255);
			return Color.FromArgb(func(_color.A), func(_color.R), func(_color.G), func(_color.B));
		}

		public static Color MultiplyColorsOnly(this Color _color, float _f)
		{
			Func<int, int> func = _i => (int)Math.Min(_i * _f, 255);
			return Color.FromArgb(_color.A, func(_color.R), func(_color.G), func(_color.B));
		}

		public static Color Multiply(this Color _color, Color _color1)
		{
			return Color.FromArgb(_color.A * _color1.A / 255, _color.R * _color1.R / 255, _color.G * _color1.G / 255, _color.B * _color1.B / 255);
		}

		public static Color Multiply(this Color _color, FColor _color1)
		{
			return Color.FromArgb((int)(_color.A * _color1.A), (int)(_color.R * _color1.R), (int)(_color.G * _color1.G), (int)(_color.B * _color1.B));
		}

		public static Color ScreenColorsOnly(this Color _color, Color _color1)
		{
			Func<int, int, int> func = (_i, _i1) => 255 - (((255 - _i) * (255 - _i1)) / 255);
			return Color.FromArgb(_color.A, func(_color.R, _color1.R), func(_color.G, _color1.G), func(_color.B, _color1.B));
		}

		public static float Lightness(this Color _color)
		{
			var max = Math.Max(_color.R, Math.Max(_color.G, _color.B));
			var min = Math.Min(_color.R, Math.Min(_color.G, _color.B));
			return (max + min) / 2 * _color.A / 255f;
		}

		public static FColor ToFColor(this Color _color)
		{
			return new FColor(_color);
		}

		public static bool ContainsEx(this Rectangle _rectangle, Point _point)
		{
			var result = _rectangle.Left <= _point.X && _rectangle.Top <= _point.Y && _rectangle.Bottom > _point.Y && _rectangle.Right > _point.X;
			return result;
		}

		public static IEnumerable<Point> AllPoints(this Rectangle _rectangle)
		{
			for (var i = _rectangle.Left; i < _rectangle.Right; ++i)
			{
				for (var j = _rectangle.Top; j < _rectangle.Bottom; ++j)
				{
					yield return new Point(i, j);
				}
			}
		}

		public static IEnumerable<Point> AllPointsExceptCorners(this Rectangle _rectangle)
		{
			var r = _rectangle;
			r.Inflate(0,-1);
			foreach (var point in r.AllPoints())
			{
				yield return point;
			}
			for (var i = _rectangle.Left + 1; i < _rectangle.Right - 1; ++i)
			{
				yield return new Point(i,_rectangle.Top);
				yield return new Point(i,_rectangle.Bottom - 1);
			}
		}

		public static EDirections Opposite(this EDirections _direction)
		{
			switch (_direction)
			{
				case EDirections.UP:
					return EDirections.DOWN;
				case EDirections.DOWN:
					return EDirections.UP;
				case EDirections.LEFT:
					return EDirections.RIGHT;
				case EDirections.RIGHT:
					return EDirections.LEFT;
				default:
					throw new ArgumentOutOfRangeException("_direction");
			}
		}

		public static ETerrains GetTerrain(this EDirections _direction)
		{
			switch (_direction)
			{
				case EDirections.UP:
					return ETerrains.UP;
				case EDirections.DOWN:
					return ETerrains.DOWN;
				case EDirections.LEFT:
					return ETerrains.LEFT;
				case EDirections.RIGHT:
					return ETerrains.RIGHT;
				default:
					throw new ArgumentOutOfRangeException("_direction");
			}
		}


		public static Point GetDelta(this EDirections _direction)
		{
			switch (_direction)
			{
				case EDirections.UP:
					return new Point(0, -1);
				case EDirections.DOWN:
					return new Point(0, 1);
				case EDirections.LEFT:
					return new Point(-1, 0);
				case EDirections.RIGHT:
					return new Point(1, 0);
				default:
					throw new ArgumentOutOfRangeException("_direction");
			}
		}

		public static EDirections GetDirection(Point _point1, Point _point2)
		{
			if (_point1.X < _point2.X) return EDirections.RIGHT;
			if (_point1.X > _point2.X) return EDirections.LEFT;
			if (_point1.Y < _point2.Y) return EDirections.DOWN;
			if (_point1.Y> _point2.Y) return EDirections.UP;
			return EDirections.NONE;
		}

		public static IEnumerable<Point> GetBorders(this EDirections _direction)
		{
			switch (_direction)
			{
				case EDirections.UP:
				case EDirections.DOWN:
					yield return EDirections.LEFT.GetDelta();
					yield return EDirections.RIGHT.GetDelta();
					break;
				case EDirections.LEFT:
				case EDirections.RIGHT:
					yield return EDirections.UP.GetDelta();
					yield return EDirections.DOWN.GetDelta();
					break;
				default:
					yield break;
			}
		}
	}
}