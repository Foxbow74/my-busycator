#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

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
	}
}