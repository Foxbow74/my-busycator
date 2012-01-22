using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common
{
	public static class Util
	{
		static readonly Dictionary<Tuple<Type,Type, object>,Attribute> m_attrs = new Dictionary<Tuple<Type, Type, object>, Attribute>();

		public static Dictionary<TEnum, TAttribute> Fill<TEnum, TAttribute>() where TAttribute : Attribute
		{
			var result = new Dictionary<TEnum, TAttribute>();
			foreach (var field in typeof (TEnum).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
			{
				result[(TEnum) field.GetValue(null)] = field.GetCustomAttributes(true).OfType<TAttribute>().Single();
			}
			return result;
		}
	}

}
