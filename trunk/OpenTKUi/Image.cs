using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace OpenTKUi
{
	public class Image : IDisposable
	{
		private readonly bool m_alpha;
		private uint m_texture;

		public Image(string _path)
			: this(new Bitmap(_path), true)
		{
		}

		public Image(Bitmap _bmp, bool _isAlpha)
		{
			Bitmap = _bmp;
			m_alpha = _isAlpha;

			Width = _bmp.Width;
			Height = _bmp.Height;

			GL.GenTextures(1, out m_texture);
			GL.BindTexture(TextureTarget.Texture2D, m_texture);

			if (m_alpha)
			{
				FillBackgroundByTransparentColorAndCopyBits();
			}
			else
			{
				CopyBits();
			}

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
		}

		public float Width { get; private set; }
		public float Height { get; private set; }
		public Bitmap Bitmap { get; private set; }

		public uint Texture
		{
			get { return m_texture; }
		}

		#region IDisposable Members

		public void Dispose()
		{
			GL.DeleteTextures(1, ref m_texture);
			Bitmap.Dispose();
		}

		#endregion

		private void FillBackgroundByTransparentColorAndCopyBits()
		{
			var data = Bitmap.LockBits(new Rectangle(0, 0, (int) Width, (int) Height), ImageLockMode.ReadWrite,
			                           PixelFormat.Format32bppArgb);

			FillBackgroundByTransparentColor(data);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
			              OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
			Bitmap.UnlockBits(data);
		}

		private void CopyBits()
		{
			var data = Bitmap.LockBits(new Rectangle(0, 0, (int) Width, (int) Height), ImageLockMode.ReadOnly,
			                           PixelFormat.Format32bppArgb);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
			              OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
			Bitmap.UnlockBits(data);
			GL.Finish();
		}

		private unsafe void FillBackgroundByTransparentColor(BitmapData _data)
		{
			var length = _data.Stride*Height/4;

			var ptr1 = (int*) _data.Scan0.ToPointer();

			var tr = *ptr1;
			while ((length--) > 0)
			{
				if (*ptr1 == tr)
				{
					*ptr1 = 255;
				}
				ptr1++;
			}
		}

		public void Update()
		{
			GL.BindTexture(TextureTarget.Texture2D, m_texture);
			var data = Bitmap.LockBits(new Rectangle(0, 0, (int) Width, (int) Height), ImageLockMode.ReadOnly,
			                           PixelFormat.Format32bppArgb);
			GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, (int) Width, (int) Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
			                 PixelType.UnsignedByte, data.Scan0);
			Bitmap.UnlockBits(data);
		}
	}
}