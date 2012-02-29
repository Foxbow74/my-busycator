﻿using System;
using System.Drawing;
using GameCore.Misc;

namespace GameCore
{
	public struct FColor
	{
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

		public FColor(Color _color)
		{
			A = _color.A / 255f;
			R = _color.R / 255f;
			G = _color.G / 255f;
			B = _color.B / 255f;
		}

		public FColor(float _a, FColor _color)
		{
			A = _a;
			R = _color.R;
			G = _color.G;
			B = _color.B;
		}

		public static readonly FColor Empty = new FColor(0, 0, 0, 0);
		public static readonly FColor Black = new FColor(1f, 0, 0, 0);
		public static readonly FColor White = new FColor(1f, 1f, 1f, 1f);

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
	}
}