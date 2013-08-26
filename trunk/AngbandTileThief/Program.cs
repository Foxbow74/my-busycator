using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GameCore;
using Image = OpenTKUi.Image;

namespace AngbandTileThief
{
	class Program
	{
		private static int TILE_SIZE = 64;
		private static decimal uu= 0;
		static readonly Color m_transp = Color.FromArgb(1, 0, 0, 0);

		static void Main(string[] args)
		{

			var bmps = new List<Bitmap>();
			foreach (var fileName in Directory.GetFiles(@"..\\RawTiles", "*.*"))
			{
				var bitmap = new Image((Bitmap)Bitmap.FromFile(fileName), true, false).Bitmap;


				var xx = new List<int>();
				var yy = new List<int>();
				{
					var empty = true;
					for (var x = 0; x < bitmap.Width; x++)
					{
						int y = 0;
						for (; y < bitmap.Height; y++)
						{

							if (empty)
							{
								if (bitmap.GetPixel(x, y) != m_transp)
								{
									xx.Add(x);
									empty = false;
									break;
								}
							}
							else
							{
								if (bitmap.GetPixel(x, y) != m_transp)
								{
									break;
								}
							}
						}
						if (!empty && y == bitmap.Height)
						{
							if (x - xx[xx.Count - 1] < TILE_SIZE)
							{
								break;
							}
							xx.Add(x);
							empty = true;
						}
					}
				}
				{
					var empty = true;
					for (var y = 0; y < bitmap.Height; y++)
					{
						int x = 0;
						for (; x < xx[xx.Count - 1]; x++)
						{

							if (empty)
							{
								if (bitmap.GetPixel(x, y) != m_transp)
								{
									yy.Add(y);
									empty = false;
									break;
								}
							}
							else
							{
								if (bitmap.GetPixel(x, y) != m_transp)
								{
									break;
								}
							}
						}
						if (!empty && x == xx[xx.Count - 1])
						{
							if (y - yy[yy.Count - 1] < TILE_SIZE)
							{
								break;
							}
							yy.Add(y);
							empty = true;
						}
					}
					yy.Add(bitmap.Height-1);
				}
				for (var ix = 0; ix < xx.Count-1; ix+=2)
				{
					for (var iy = 0; iy < yy.Count-1; iy+=2)
					{
						var rct = new Rectangle(xx[ix], yy[iy], xx[ix + 1] - xx[ix], yy[iy + 1] - yy[iy]);
						var bmp = GetTile(bitmap, rct);
						if (bmp != null)
						{
							bmps.Add(bmp);
						}
					}
				}

				#region vis

//using (var gr = Graphics.FromImage(bitmap))
				//{
				//	{
				//		var x = 0;
				//		for (var index = 0; index < xx.Count; index += 2)
				//		{
				//			var x1 = xx[index];
				//			gr.FillRectangle(Brushes.Aqua, x, 0, x1 - x, bitmap.Height);
				//			if (index == xx.Count - 1)
				//			{
				//				break;
				//			}
				//			x = xx[index + 1];
				//		}

				//	}
				//	{
				//		var y = 0;
				//		for (var index = 0; index < yy.Count; index += 2)
				//		{
				//			var y1 = yy[index];
				//			gr.FillRectangle(Brushes.Aquamarine, 0, y, bitmap.Width, y1 - y);
				//			if (index == yy.Count - 1)
				//			{
				//				break;
				//			}
				//			y = yy[index + 1];
				//		}

				//	}
				//}
				//bitmap.Save(@"d:\\bmp.png", ImageFormat.Png);

				#endregion

			}

			var ideal = bmps.Where(x => x.Width == TILE_SIZE && x.Height == TILE_SIZE).ToArray();
			var texture = new Bitmap(TILE_SIZE, TILE_SIZE, PixelFormat.Format32bppPArgb);
			for (var x = 0; x < TILE_SIZE; x++)
			{
				for (var y = 0; y < TILE_SIZE; y++)
				{
					var pixels = ideal.Select(b=>b.GetPixel(x,y)).Where(c=>c!=m_transp).ToArray();

					var average = Average(pixels);
					var ordered = pixels.GroupBy(c => Distance(c, average)).OrderByDescending(g => g.Count());
					var first = ordered.First();
					if (first.Count() > 5)
					{
						texture.SetPixel(x, y, Average(first));
					}
				}
			}
			texture.Save(@"d:\\text.png", ImageFormat.Png);

			foreach (var bitmap in ideal)
			{
				var b = new Bitmap(TILE_SIZE, TILE_SIZE, PixelFormat.Format32bppPArgb);
				for (var x = 0; x < TILE_SIZE; x++)
				{
					for (var y = 0; y < TILE_SIZE; y++)
					{
						if (Distance(bitmap.GetPixel(x, y), texture.GetPixel(x, y)) <2)
						{
							b.SetPixel(x, y, Color.Empty);
						}
						else
						{
							b.SetPixel(x, y, bitmap.GetPixel(x, y));
						}
					}
				}
				b.Save(@"d:\\u" + uu++ + ".png", ImageFormat.Png);
			}
		}

		private static Color Average(IEnumerable<Color> pixels)
		{
			var count = pixels.Count();
			var averageR = pixels.Sum(c => c.R) / count;
			var averageG = pixels.Sum(c => c.G) / count;
			var averageB = pixels.Sum(c => c.B) / count;
			var average = Color.FromArgb(255, averageR, averageG, averageB);
			return average;
		}

		public static double Distance(Color a, Color b)
		{
			var pow = Math.Pow((a.R - b.R)*(a.R - b.R) + (a.G - b.G)*(a.G - b.G) + (a.B - b.B)*(a.B - b.B), 1.0/3);
			return (int)(pow/2);
		}

		private static Bitmap  GetTile(Bitmap _bitmap, Rectangle _rct)
		{
			_rct = OgrankaI(_bitmap, _rct);
			while (_rct.Width > TILE_SIZE)
			{
				var fl = false;
				{
					var x = _rct.Left;
					for (var y = _rct.Top; y < _rct.Bottom; y++)
					{
						if (_bitmap.GetPixel(x, y) != m_transp) continue;
						_rct = new Rectangle(_rct.Left + 1, _rct.Top, _rct.Width - 1, _rct.Height);
						fl = true;
						break;
					}
				}
				{
					var x = _rct.Right;
					for (var y = _rct.Top; y < _rct.Bottom; y++)
					{
						if (_bitmap.GetPixel(x, y) != m_transp) continue;
						_rct = new Rectangle(_rct.Left, _rct.Top, _rct.Width-1, _rct.Height);
						fl = true;
						break;
					}
				}
				if (!fl)
				{
					break;
				}
			}

			while (_rct.Height > TILE_SIZE)
			{
				var fl = false;
				{
					var y = _rct.Top;
					for (var x = _rct.Left; x < _rct.Right; x++)
					{
						if (_bitmap.GetPixel(x, y) != m_transp) continue;
						_rct = new Rectangle(_rct.Left, _rct.Top+1, _rct.Width, _rct.Height-1);
						fl = true;
						break;
					}
				}
				{
					var y = _rct.Bottom;
					for (var x = _rct.Left; x < _rct.Right; x++)
					{
						if (_bitmap.GetPixel(x, y) != m_transp) continue;
						_rct = new Rectangle(_rct.Left, _rct.Top , _rct.Width, _rct.Height - 1);
						fl = true;
						break;
					}
				}
				if (!fl)
				{
					break;
				}
			}

			var result = new Bitmap(_rct.Width, _rct.Height, _bitmap.PixelFormat);
			using (var gr=Graphics.FromImage(result))
			{
				gr.DrawImage(_bitmap, 0,0,_rct, GraphicsUnit.Pixel);
			}
			result.Save(@"test.png", ImageFormat.Png);
			if (new FileInfo(@"test.png").Length < 1000)
			{
				return null;
			}
			return result;
		}

		private static Rectangle OgrankaI(Bitmap _bitmap, Rectangle _rct)
		{
			var l = _rct.Left;
			var r = _rct.Right;
			var t = _rct.Top;
			var b = _rct.Bottom;
			{
				var ww = _rct.Width/2;
				var halfH = _rct.Top + _rct.Height/2;
				for (var x = 0; x < ww; x++)
				{
					if (_bitmap.GetPixel(l + x, halfH) == m_transp || _bitmap.GetPixel(l + x, halfH - 5) == m_transp || _bitmap.GetPixel(l + x, halfH + 5) == m_transp) continue;
					l += x;
					break;
				}
				for (var x = 0; x < ww; x++)
				{
					if (_bitmap.GetPixel(r - x, halfH) == m_transp || _bitmap.GetPixel(r - x, halfH - 5) == m_transp || _bitmap.GetPixel(r - x, halfH + 5) == m_transp) continue;
					r -= x;
					break;
				}
			}

			{
				var hh = _rct.Height/2;
				var halfW = _rct.Left + _rct.Width/2;
				for (var y = 0; y < hh; y++)
				{
					if (_bitmap.GetPixel(halfW, t + y) == m_transp || _bitmap.GetPixel(halfW - 5, t + y) == m_transp || _bitmap.GetPixel(halfW + 5, t + y) == m_transp) continue;
					t += y;
					break;
				}
				for (var y = 0; y < hh; y++)
				{
					if (_bitmap.GetPixel(halfW, b - y) == m_transp || _bitmap.GetPixel(halfW - 5, b - y) == m_transp || _bitmap.GetPixel(halfW + 5, b - y) == m_transp) continue;
					b -= y;
					break;
				}
			}
			_rct = new Rectangle(l, t, r - l + 1, b - t + 1);
			return _rct;
		}
	}
}
