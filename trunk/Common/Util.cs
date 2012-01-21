using System;
using System.Linq;
using System.Reflection;

namespace Common
{
	public static class Util
	{
		public static TAttribute GetAttribute<TEnum, TAttribute>(this TEnum _enum) where TAttribute:Attribute
		{
			foreach (var field in typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
			{
				if(field.GetValue(null).Equals(_enum))return field.GetCustomAttributes(true).OfType<TAttribute>().FirstOrDefault();
			}
			throw new NotImplementedException();
		}
	}

}
