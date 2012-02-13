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
	}
}