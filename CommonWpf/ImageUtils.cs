﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClientCommonWpf
{
	public static class ImageUtils
	{
		private static readonly Dictionary<Bitmap, BitmapSource> m_sources = new Dictionary<Bitmap, BitmapSource>();

		public static BitmapSource Source(this Bitmap _bmp)
		{
            if (_bmp == null) return null;

			BitmapSource value;
			if(!m_sources.TryGetValue(_bmp, out value))
			{
				value = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(_bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				m_sources[_bmp] = value;
			}
			return value;
		}

		public static BitmapSource SourceDisabled(this Bitmap _bmp)
		{
			var orgBmp = _bmp.Source();
			if (orgBmp.Format == PixelFormats.Bgra32)
			{
				var orgPixels = new byte[orgBmp.PixelHeight * orgBmp.PixelWidth * 4];
				var newPixels = new byte[orgPixels.Length];
				orgBmp.CopyPixels(orgPixels, orgBmp.PixelWidth * 4, 0);
				for (var i = 3; i < orgPixels.Length; i += 4)
				{
					var grayVal = (orgPixels[i - 3] + orgPixels[i - 2] + orgPixels[i - 1]) / 6 + 128; 

					//if (grayVal != 0) grayVal = grayVal / 3;
					newPixels[i] = orgPixels[i];
					newPixels[i - 3] = (byte)grayVal;
					newPixels[i - 2] = (byte)grayVal;
					newPixels[i - 1] = (byte)grayVal;
				}
				return BitmapSource.Create(orgBmp.PixelWidth, orgBmp.PixelHeight,
					96, 96, PixelFormats.Bgra32, null, newPixels,
					orgBmp.PixelWidth * 4);
			}
			return orgBmp;
		}
	}
}