using System;
using System.Drawing;
using GameCore.Misc;

namespace GameCore
{
	public struct FColor
	{
		#region predefined colors

		public static readonly FColor Empty = new FColor(0, 0, 0, 0);
		public static readonly FColor Transparent = new FColor(0f, 1f, 1f, 1f);
		public static readonly FColor AliceBlue = new FColor(1f, 0.94f, 0.97f, 1f);
		public static readonly FColor AntiqueWhite = new FColor(1f, 0.98f, 0.92f, 0.84f);
		public static readonly FColor Aqua = new FColor(1f, 0f, 1f, 1f);
		public static readonly FColor Aquamarine = new FColor(1f, 0.5f, 1f, 0.83f);
		public static readonly FColor Azure = new FColor(1f, 0.94f, 1f, 1f);
		public static readonly FColor Beige = new FColor(1f, 0.96f, 0.96f, 0.86f);
		public static readonly FColor Bisque = new FColor(1f, 1f, 0.89f, 0.77f);
		public static readonly FColor Black = new FColor(1f, 0f, 0f, 0f);
		public static readonly FColor BlanchedAlmond = new FColor(1f, 1f, 0.92f, 0.8f);
		public static readonly FColor Blue = new FColor(1f, 0f, 0f, 1f);
		public static readonly FColor BlueViolet = new FColor(1f, 0.54f, 0.17f, 0.89f);
		public static readonly FColor Brown = new FColor(1f, 0.65f, 0.16f, 0.16f);
		public static readonly FColor BurlyWood = new FColor(1f, 0.87f, 0.72f, 0.53f);
		public static readonly FColor CadetBlue = new FColor(1f, 0.37f, 0.62f, 0.63f);
		public static readonly FColor Chartreuse = new FColor(1f, 0.5f, 1f, 0f);
		public static readonly FColor Chocolate = new FColor(1f, 0.82f, 0.41f, 0.12f);
		public static readonly FColor Coral = new FColor(1f, 1f, 0.5f, 0.31f);
		public static readonly FColor CornflowerBlue = new FColor(1f, 0.39f, 0.58f, 0.93f);
		public static readonly FColor Cornsilk = new FColor(1f, 1f, 0.97f, 0.86f);
		public static readonly FColor Crimson = new FColor(1f, 0.86f, 0.08f, 0.24f);
		public static readonly FColor Cyan = new FColor(1f, 0f, 1f, 1f);
		public static readonly FColor DarkBlue = new FColor(1f, 0f, 0f, 0.55f);
		public static readonly FColor DarkCyan = new FColor(1f, 0f, 0.55f, 0.55f);
		public static readonly FColor DarkGoldenrod = new FColor(1f, 0.72f, 0.53f, 0.04f);
		public static readonly FColor DarkGray = new FColor(1f, 0.66f, 0.66f, 0.66f);
		public static readonly FColor DarkGreen = new FColor(1f, 0f, 0.39f, 0f);
		public static readonly FColor DarkKhaki = new FColor(1f, 0.74f, 0.72f, 0.42f);
		public static readonly FColor DarkMagenta = new FColor(1f, 0.55f, 0f, 0.55f);
		public static readonly FColor DarkOliveGreen = new FColor(1f, 0.33f, 0.42f, 0.18f);
		public static readonly FColor DarkOrange = new FColor(1f, 1f, 0.55f, 0f);
		public static readonly FColor DarkOrchid = new FColor(1f, 0.6f, 0.2f, 0.8f);
		public static readonly FColor DarkRed = new FColor(1f, 0.55f, 0f, 0f);
		public static readonly FColor DarkSalmon = new FColor(1f, 0.91f, 0.59f, 0.48f);
		public static readonly FColor DarkSeaGreen = new FColor(1f, 0.56f, 0.74f, 0.55f);
		public static readonly FColor DarkSlateBlue = new FColor(1f, 0.28f, 0.24f, 0.55f);
		public static readonly FColor DarkSlateGray = new FColor(1f, 0.18f, 0.31f, 0.31f);
		public static readonly FColor DarkTurquoise = new FColor(1f, 0f, 0.81f, 0.82f);
		public static readonly FColor DarkViolet = new FColor(1f, 0.58f, 0f, 0.83f);
		public static readonly FColor DeepPink = new FColor(1f, 1f, 0.08f, 0.58f);
		public static readonly FColor DeepSkyBlue = new FColor(1f, 0f, 0.75f, 1f);
		public static readonly FColor DimGray = new FColor(1f, 0.41f, 0.41f, 0.41f);
		public static readonly FColor DodgerBlue = new FColor(1f, 0.12f, 0.56f, 1f);
		public static readonly FColor Firebrick = new FColor(1f, 0.7f, 0.13f, 0.13f);
		public static readonly FColor FloralWhite = new FColor(1f, 1f, 0.98f, 0.94f);
		public static readonly FColor ForestGreen = new FColor(1f, 0.13f, 0.55f, 0.13f);
		public static readonly FColor Fuchsia = new FColor(1f, 1f, 0f, 1f);
		public static readonly FColor Gainsboro = new FColor(1f, 0.86f, 0.86f, 0.86f);
		public static readonly FColor GhostWhite = new FColor(1f, 0.97f, 0.97f, 1f);
		public static readonly FColor Gold = new FColor(1f, 1f, 0.84f, 0f);
		public static readonly FColor Goldenrod = new FColor(1f, 0.85f, 0.65f, 0.13f);
		public static readonly FColor Gray = new FColor(1f, 0.5f, 0.5f, 0.5f);
		public static readonly FColor Green = new FColor(1f, 0f, 0.5f, 0f);
		public static readonly FColor GreenYellow = new FColor(1f, 0.68f, 1f, 0.18f);
		public static readonly FColor Honeydew = new FColor(1f, 0.94f, 1f, 0.94f);
		public static readonly FColor HotPink = new FColor(1f, 1f, 0.41f, 0.71f);
		public static readonly FColor IndianRed = new FColor(1f, 0.8f, 0.36f, 0.36f);
		public static readonly FColor Indigo = new FColor(1f, 0.29f, 0f, 0.51f);
		public static readonly FColor Ivory = new FColor(1f, 1f, 1f, 0.94f);
		public static readonly FColor Khaki = new FColor(1f, 0.94f, 0.9f, 0.55f);
		public static readonly FColor Lavender = new FColor(1f, 0.9f, 0.9f, 0.98f);
		public static readonly FColor LavenderBlush = new FColor(1f, 1f, 0.94f, 0.96f);
		public static readonly FColor LawnGreen = new FColor(1f, 0.49f, 0.99f, 0f);
		public static readonly FColor LemonChiffon = new FColor(1f, 1f, 0.98f, 0.8f);
		public static readonly FColor LightBlue = new FColor(1f, 0.68f, 0.85f, 0.9f);
		public static readonly FColor LightCoral = new FColor(1f, 0.94f, 0.5f, 0.5f);
		public static readonly FColor LightCyan = new FColor(1f, 0.88f, 1f, 1f);
		public static readonly FColor LightGoldenrodYellow = new FColor(1f, 0.98f, 0.98f, 0.82f);
		public static readonly FColor LightGreen = new FColor(1f, 0.56f, 0.93f, 0.56f);
		public static readonly FColor LightGray = new FColor(1f, 0.83f, 0.83f, 0.83f);
		public static readonly FColor LightPink = new FColor(1f, 1f, 0.71f, 0.76f);
		public static readonly FColor LightSalmon = new FColor(1f, 1f, 0.63f, 0.48f);
		public static readonly FColor LightSeaGreen = new FColor(1f, 0.13f, 0.7f, 0.67f);
		public static readonly FColor LightSkyBlue = new FColor(1f, 0.53f, 0.81f, 0.98f);
		public static readonly FColor LightSlateGray = new FColor(1f, 0.47f, 0.53f, 0.6f);
		public static readonly FColor LightSteelBlue = new FColor(1f, 0.69f, 0.77f, 0.87f);
		public static readonly FColor LightYellow = new FColor(1f, 1f, 1f, 0.88f);
		public static readonly FColor Lime = new FColor(1f, 0f, 1f, 0f);
		public static readonly FColor LimeGreen = new FColor(1f, 0.2f, 0.8f, 0.2f);
		public static readonly FColor Linen = new FColor(1f, 0.98f, 0.94f, 0.9f);
		public static readonly FColor Magenta = new FColor(1f, 1f, 0f, 1f);
		public static readonly FColor Maroon = new FColor(1f, 0.5f, 0f, 0f);
		public static readonly FColor MediumAquamarine = new FColor(1f, 0.4f, 0.8f, 0.67f);
		public static readonly FColor MediumBlue = new FColor(1f, 0f, 0f, 0.8f);
		public static readonly FColor MediumOrchid = new FColor(1f, 0.73f, 0.33f, 0.83f);
		public static readonly FColor MediumPurple = new FColor(1f, 0.58f, 0.44f, 0.86f);
		public static readonly FColor MediumSeaGreen = new FColor(1f, 0.24f, 0.7f, 0.44f);
		public static readonly FColor MediumSlateBlue = new FColor(1f, 0.48f, 0.41f, 0.93f);
		public static readonly FColor MediumSpringGreen = new FColor(1f, 0f, 0.98f, 0.6f);
		public static readonly FColor MediumTurquoise = new FColor(1f, 0.28f, 0.82f, 0.8f);
		public static readonly FColor MediumVioletRed = new FColor(1f, 0.78f, 0.08f, 0.52f);
		public static readonly FColor MidnightBlue = new FColor(1f, 0.1f, 0.1f, 0.44f);
		public static readonly FColor MintCream = new FColor(1f, 0.96f, 1f, 0.98f);
		public static readonly FColor MistyRose = new FColor(1f, 1f, 0.89f, 0.88f);
		public static readonly FColor Moccasin = new FColor(1f, 1f, 0.89f, 0.71f);
		public static readonly FColor NavajoWhite = new FColor(1f, 1f, 0.87f, 0.68f);
		public static readonly FColor Navy = new FColor(1f, 0f, 0f, 0.5f);
		public static readonly FColor OldLace = new FColor(1f, 0.99f, 0.96f, 0.9f);
		public static readonly FColor Olive = new FColor(1f, 0.5f, 0.5f, 0f);
		public static readonly FColor OliveDrab = new FColor(1f, 0.42f, 0.56f, 0.14f);
		public static readonly FColor Orange = new FColor(1f, 1f, 0.65f, 0f);
		public static readonly FColor OrangeRed = new FColor(1f, 1f, 0.27f, 0f);
		public static readonly FColor Orchid = new FColor(1f, 0.85f, 0.44f, 0.84f);
		public static readonly FColor PaleGoldenrod = new FColor(1f, 0.93f, 0.91f, 0.67f);
		public static readonly FColor PaleGreen = new FColor(1f, 0.6f, 0.98f, 0.6f);
		public static readonly FColor PaleTurquoise = new FColor(1f, 0.69f, 0.93f, 0.93f);
		public static readonly FColor PaleVioletRed = new FColor(1f, 0.86f, 0.44f, 0.58f);
		public static readonly FColor PapayaWhip = new FColor(1f, 1f, 0.94f, 0.84f);
		public static readonly FColor PeachPuff = new FColor(1f, 1f, 0.85f, 0.73f);
		public static readonly FColor Peru = new FColor(1f, 0.8f, 0.52f, 0.25f);
		public static readonly FColor Pink = new FColor(1f, 1f, 0.75f, 0.8f);
		public static readonly FColor Plum = new FColor(1f, 0.87f, 0.63f, 0.87f);
		public static readonly FColor PowderBlue = new FColor(1f, 0.69f, 0.88f, 0.9f);
		public static readonly FColor Purple = new FColor(1f, 0.5f, 0f, 0.5f);
		public static readonly FColor Red = new FColor(1f, 1f, 0f, 0f);
		public static readonly FColor RosyBrown = new FColor(1f, 0.74f, 0.56f, 0.56f);
		public static readonly FColor RoyalBlue = new FColor(1f, 0.25f, 0.41f, 0.88f);
		public static readonly FColor SaddleBrown = new FColor(1f, 0.55f, 0.27f, 0.07f);
		public static readonly FColor Salmon = new FColor(1f, 0.98f, 0.5f, 0.45f);
		public static readonly FColor SandyBrown = new FColor(1f, 0.96f, 0.64f, 0.38f);
		public static readonly FColor SeaGreen = new FColor(1f, 0.18f, 0.55f, 0.34f);
		public static readonly FColor SeaShell = new FColor(1f, 1f, 0.96f, 0.93f);
		public static readonly FColor Sienna = new FColor(1f, 0.63f, 0.32f, 0.18f);
		public static readonly FColor Silver = new FColor(1f, 0.75f, 0.75f, 0.75f);
		public static readonly FColor SkyBlue = new FColor(1f, 0.53f, 0.81f, 0.92f);
		public static readonly FColor SlateBlue = new FColor(1f, 0.42f, 0.35f, 0.8f);
		public static readonly FColor SlateGray = new FColor(1f, 0.44f, 0.5f, 0.56f);
		public static readonly FColor Snow = new FColor(1f, 1f, 0.98f, 0.98f);
		public static readonly FColor SpringGreen = new FColor(1f, 0f, 1f, 0.5f);
		public static readonly FColor SteelBlue = new FColor(1f, 0.27f, 0.51f, 0.71f);
		public static readonly FColor Tan = new FColor(1f, 0.82f, 0.71f, 0.55f);
		public static readonly FColor Teal = new FColor(1f, 0f, 0.5f, 0.5f);
		public static readonly FColor Thistle = new FColor(1f, 0.85f, 0.75f, 0.85f);
		public static readonly FColor Tomato = new FColor(1f, 1f, 0.39f, 0.28f);
		public static readonly FColor Turquoise = new FColor(1f, 0.25f, 0.88f, 0.82f);
		public static readonly FColor Violet = new FColor(1f, 0.93f, 0.51f, 0.93f);
		public static readonly FColor Wheat = new FColor(1f, 0.96f, 0.87f, 0.7f);
		public static readonly FColor White = new FColor(1f, 1f, 1f, 1f);
		public static readonly FColor WhiteSmoke = new FColor(1f, 0.96f, 0.96f, 0.96f);
		public static readonly FColor Yellow = new FColor(1f, 1f, 1f, 0f);
		public static readonly FColor YellowGreen = new FColor(1f, 0.6f, 0.8f, 0.2f);

		#endregion

		public float A;
		public float R;
		public float G;
		public float B;

		public FColor(float _a, float _r, float _g, float _b)
		{
			if(_a<0 || _r<0 || _b<0 || _g<0)
			{
				
			}
			A = _a;
			R = _r;
			G = _g;
			B = _b;
		}

		public FColor(float _a, FColor _color)
		{
			A = _a;
			R = _color.R;
			G = _color.G;
			B = _color.B;
		}

		public FColor NormalColorOnly
		{
			get
			{
				return new FColor(1f, R / A, G / A, B / A);
			}
		}

		public FColor ScreenColorsOnly(FColor _color)
		{
			Func<float, float, float> func = (_i, _i1) => 1f - ((1f - _i) * (1f - _i1));
			return new FColor(A, func(R, _color.R), func(G, _color.G), func(B, _color.B));
		}

		public FColor Screen(FColor _color)
		{
			var f = Math.Max(A, Math.Max(R, Math.Max(G, B)));
			f = Math.Max(f, Math.Max(_color.A, Math.Max(_color.R, Math.Max(_color.G, _color.B))));

			Func<float, float, float> func = (_i, _i1) => f - ((1f - _i / f) * (1f - _i1 / f)) * f;


			return new FColor(func(A, _color.A), func(R, _color.R), func(G, _color.G), func(B, _color.B));
		}

		public FColor Multiply(FColor _color)
		{
			return new FColor(A * _color.A, R * _color.R, G * _color.G, B * _color.B);
		}

		public FColor Multiply(float _f)
		{
			return new FColor(A * _f, R * _f, G * _f, B * _f);
		}

		public override string ToString()
		{
			//return string.Format("FC(A={0:N2};R={1:N2};G={2:N2};B={3:N2})", A, R, G, B);
			return string.Format("f ARGB=({0:N0},{1:N0},{2:N0},{3:N0})", A*255, R*255, G*255, B*255);
		}

		public Color ToColor()
		{
			return Color.FromArgb((int) (A*255), (int) (R*255), (int) (G*255), (int) (B*255));
		}


		public FColor Lerp(FColor _color2, float _f)
		{
			var color = new FColor(
				MathHelper.Lerp(A, _color2.A, _f),
				MathHelper.Lerp(R, _color2.R, _f),
				MathHelper.Lerp(G, _color2.G, _f),
				MathHelper.Lerp(B, _color2.B, _f));
			return color;
		}

		public FColor LerpColorsOnly(FColor _color2, float _f)
		{
			if(_f==0)
			{
				return this;
			}
			var color = new FColor(
				A,
				MathHelper.Lerp(R, _color2.R, _f),
				MathHelper.Lerp(G, _color2.G, _f),
				MathHelper.Lerp(B, _color2.B, _f));
			return color;
		}

		public float Lightness()
		{
			var max = Math.Max(R, Math.Max(G, B));
			var min = Math.Min(R, Math.Min(G, B));
			return (max + min) / 2 * A;
		}

		public FColor ToGrayScale()
		{
			var mid = (R + G + B)/4;
			return new FColor(1f, mid, mid, mid);
		}

		public FColor Clamp()
		{
			var k = Math.Max(R, Math.Max(G, B));
			if(k>1)
			{
				return new FColor(1f, R/k, G/k, B/k);
			}
			return this;
			//return new FColor(Math.Min(1f, A), Math.Min(1f, R), Math.Min(1f, G), Math.Min(1f, B));
		}

		public void AddColorOnly(FColor _fColor)
		{
			A += 1f;
			R += _fColor.R;
			G += _fColor.G;
			B += _fColor.B;
		}

		public void Add(FColor _fColor)
		{
			A += _fColor.A;
			R += _fColor.R;
			G += _fColor.G;
			B += _fColor.B;
		}

		public static FColor FromArgb(int _a, int _r, int _g, int _b)
		{
			return new FColor(_a / 255f, _r / 255f, _g / 255f, _b / 255f);
		}

		public static FColor FromArgb(int _r, int _g, int _b)
		{
			return new FColor(1f, _r / 255f, _g / 255f, _b / 255f);
		}
	}
}