using System;
using System.Drawing;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameUi
{
	public static class VectorHelpers
	{
		public static Color Lerp(this Color _color1, Color _color2, float _f)
		{
			var color = Color.FromArgb(
				(int) MathHelper.Lerp(_color1.A, _color2.A, _f),
				(int) MathHelper.Lerp(_color1.R, _color2.R, _f),
				(int) MathHelper.Lerp(_color1.G, _color2.G, _f),
				(int) MathHelper.Lerp(_color1.B, _color2.B, _f));
			return color;
		}

		public static Color Multiply(this Color _color, float _f)
		{
			Func<int, int> mult = _i => (int)Math.Min(_i * _f, 255);
			return Color.FromArgb(mult(_color.A), mult(_color.R), mult(_color.G), mult(_color.B));
		}

		public static Point MeasureString(this EFonts _font, string _s)
		{
			return new Point(15, 15);
		}
	}


}