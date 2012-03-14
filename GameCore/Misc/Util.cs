using System;
using System.Collections.Generic;
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
				var attribute = field.GetCustomAttributes(true).OfType<TAttribute>().SingleOrDefault();
				if(attribute==null)
				{
					attribute = (TAttribute)Activator.CreateInstance(typeof (TAttribute));
				}
				result[(TEnum) field.GetValue(null)] = attribute;
			}
			return result;
		}

		public static IEnumerable<Type> GetAllTypesOf<T>()
		{
			return from assembly in AppDomain.CurrentDomain.GetAssemblies()
				   from type in assembly.GetTypes()
				   where typeof(T).IsAssignableFrom(type) && !type.IsAbstract
				   select type;
		}

		public static T As<TSource, T>(this TSource _d) where T : TSource
		{
			return (T) _d;
		}

		public static bool ContainsEx(this Rct _rct, Point _point)
		{
			var result = _rct.Left <= _point.X && _rct.Top <= _point.Y && _rct.Bottom > _point.Y && _rct.Right > _point.X;
			return result;
		}

		public static IEnumerable<Point> AllPoints(this Rct _rct)
		{
			for (var i = _rct.Left; i < _rct.Right; ++i)
			{
				for (var j = _rct.Top; j < _rct.Bottom; ++j)
				{
					yield return new Point(i, j);
				}
			}
		}

		public static IEnumerable<KeyValuePair<Point, EDirections>> AllForbidBorders(this Rct _rct)
		{
			var rect = new Rct(_rct.Left - 1, _rct.Top - 1, _rct.Width + 2, _rct.Height + 2);
			for (var i = rect.Left; i < rect.Right; ++i)
			{
				yield return new KeyValuePair<Point, EDirections>(new Point(i, rect.Top), EDirections.DOWN | EDirections.UP);
				yield return new KeyValuePair<Point, EDirections>(new Point(i, rect.Bottom - 1), EDirections.DOWN | EDirections.UP);
			}

			for (var j = _rct.Top; j < _rct.Bottom; ++j)
			{
				yield return new KeyValuePair<Point, EDirections>(new Point(rect.Left, j), EDirections.UP | EDirections.DOWN);
				yield return new KeyValuePair<Point, EDirections>(new Point(rect.Right - 1, j), EDirections.UP | EDirections.DOWN);
			}
		}

		public static IEnumerable<Point> AllPointsExceptCorners(this Rct _rct)
		{
			var r = _rct.Inflate(0, -1);
			foreach (var point in r.AllPoints)
			{
				yield return point;
			}
			for (var i = _rct.Left + 1; i <= _rct.Right - 1; ++i)
			{
				yield return new Point(i,_rct.Top);
				yield return new Point(i,_rct.Bottom);
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

		public static IEnumerable<KeyValuePair<Point, EDirections>> GetBorders(this EDirections _direction)
		{
			switch (_direction)
			{
				case EDirections.UP:
				case EDirections.DOWN:
					yield return new KeyValuePair<Point, EDirections>(EDirections.LEFT.GetDelta(), EDirections.LEFT | EDirections.RIGHT);
					yield return new KeyValuePair<Point, EDirections>(EDirections.RIGHT.GetDelta(), EDirections.LEFT | EDirections.RIGHT);
					break;
				case EDirections.LEFT:
				case EDirections.RIGHT:
					yield return new KeyValuePair<Point, EDirections>(EDirections.UP.GetDelta(), EDirections.UP | EDirections.DOWN);
					yield return new KeyValuePair<Point, EDirections>(EDirections.DOWN.GetDelta(), EDirections.UP | EDirections.DOWN);
					break;
				default:
					yield break;
			}
		}

		private static EDirections[] m_allDirections;

		public static EDirections[] AllDirections
		{
			get 
			{
				if (m_allDirections==null)
				{
					m_allDirections = Enum.GetValues(typeof (EDirections)).Cast<EDirections>().Where(_dir => _dir != EDirections.NONE && _dir != EDirections.ALL).ToArray();
				}
				return m_allDirections;
			}
		}

		public static IEnumerable<EDirections> AllDirectionsIn(this EDirections _allowed)
		{
			return Enum.GetValues(typeof(EDirections)).Cast<EDirections>().Where(_dir => _dir != EDirections.NONE && _dir != EDirections.ALL && _allowed.HasFlag(_dir));
		}

		public static EDirections GetRandomDirections(this Random _random)
		{
			var dirs = 1 + (EDirections)_random.Next((int)EDirections.ALL);
			return dirs;
		}

		public static EDirections GetRandomDirection(this Random _random)
		{
			var dir = AllDirections[_random.Next(AllDirections.Length)];
			return dir;
		}

		public static T RandomItem<T>(this T[] _array, Random _rnd)
		{
			return _array[_rnd.Next(_array.Length)];
		}
	}
}