using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Misc;

namespace GameCore
{
	public static class Util
	{
		private static EDirections[] m_allDirections;
		private static Point[] m_allDeltas;

		public static EDirections[] AllDirections
		{
			get { return m_allDirections ?? (m_allDirections = Enum.GetValues(typeof(EDirections)).Cast<EDirections>().Where(_dir => _dir != EDirections.NONE && _dir != EDirections.ALL).ToArray()); }
		}

		public static Point[] AllDeltas
		{
			get { return m_allDeltas ?? (m_allDeltas = AllDirections.Select(_directions => _directions.GetDelta()).ToArray()); }
		}

		public static Dictionary<TEnum, TAttribute> Fill<TEnum, TAttribute>() where TAttribute : Attribute
		{
			var result = new Dictionary<TEnum, TAttribute>();
			foreach (
				var field in typeof (TEnum).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
			{
				var attribute = field.GetCustomAttributes(true).OfType<TAttribute>().SingleOrDefault();
				if (attribute == null)
				{
					attribute = (TAttribute) Activator.CreateInstance(typeof (TAttribute));
				}
				result[(TEnum) field.GetValue(null)] = attribute;
			}
			return result;
		}

		public static IEnumerable<Type> GetAllTypesOf<T>()
		{
			return from assembly in AppDomain.CurrentDomain.GetAssemblies()
			       from type in assembly.GetTypes()
			       where typeof (T).IsAssignableFrom(type) && !type.IsAbstract
			       select type;
		}

		public static T As<TSource, T>(this TSource _d) where T : TSource { return (T) _d; }


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
			if (_point1.Y > _point2.Y) return EDirections.UP;
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

		public static IEnumerable<EDirections> AllDirectionsIn(this EDirections _allowed)
		{
			return Enum.GetValues(typeof (EDirections)).Cast<EDirections>().Where(_dir => _dir != EDirections.NONE && _dir != EDirections.ALL && _allowed.HasFlag(_dir));
		}

		public static EDirections GetRandomDirections(this Random _random)
		{
			var dirs = 1 + (EDirections) _random.Next((int) EDirections.ALL);
			return dirs;
		}

		public static EDirections GetRandomDirection(this Random _rnd)
		{
			return AllDirections.RandomItem(_rnd);
		}

		public static T RandomItem<T>(this T[] _array, Random _rnd)
		{
			if(_array.Length==0)
			{
				return default(T);
			}
			return _array[_rnd.Next(_array.Length)];
		}

		public static string GetString(this EALConst _const)
		{
			return World.AL.GetString(_const);
		}

		public static string GetString(this EALSentence _sentence, params Noun[] _nouns)
		{
			return World.AL.GetString(_sentence, _nouns);
		}

		public static Noun AsNoun(this ETerrains _terrain)
		{
			return World.AL.AsNoun(_terrain);
		}

		public static Noun AsNoun(this EItemCategory _category)
		{
			return World.AL.AsNoun(_category);
		}

		public static Noun AsNoun(this EActionCategory _category)
		{
			return World.AL.AsNoun(_category);
		}

		public static Noun AsNoun(this EEquipmentPlaces _place)
		{
			return World.AL.AsNoun(_place);
		}

		public static Noun AsNoun(this EALNouns _enoun)
		{
			return World.AL.AsNoun(_enoun);
		}
	}
}