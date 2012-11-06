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

		public Image(Bitmap _bmp, bool _isAlpha, bool _glFunctionality = true)
		{
            Width = _bmp.Width;
            Height = _bmp.Height;

            if(_bmp.PixelFormat==PixelFormat.Format32bppPArgb)
		    {
		        Bitmap = _bmp;
		    }
    		else
		    {
		        Bitmap = new Bitmap(_bmp.Width, _bmp.Height, PixelFormat.Format32bppPArgb);
		        using (var gr = Graphics.FromImage(Bitmap))
		        {
		            gr.DrawImage(_bmp, 0, 0, Width, Height);
		        }
		    }

		    m_alpha = _isAlpha;

			if (_glFunctionality)
			{
				GL.GenTextures(1, out m_texture);
				GL.BindTexture(TextureTarget.Texture2D, m_texture);
			}

			if (m_alpha)
			{
				FillBackgroundByTransparentColorAndCopyBits(_glFunctionality);
			}
			else
			{
				CopyBits();
			}

			if (_glFunctionality)
			{
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
			}
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

		private void FillBackgroundByTransparentColorAndCopyBits(bool _glFunctionality = true)
		{
			var data = Bitmap.LockBits(new Rectangle(0, 0, (int) Width, (int) Height), ImageLockMode.ReadWrite,
			                           PixelFormat.Format32bppPArgb);

			FillBackgroundByTransparentColor(data, Color.FromArgb(1,0,0,0));

			if (_glFunctionality)
			{
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
			}
			Bitmap.UnlockBits(data);
		}

		private void CopyBits(bool _glFunctionality = true)
		{
			var data = Bitmap.LockBits(new Rectangle(0, 0, (int) Width, (int) Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);

			if (_glFunctionality)
			{
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
				GL.Finish();
			}
			Bitmap.UnlockBits(data);
		}

		public unsafe void ReplaceColor(Color _color, Color _byColor)
		{
			var data = Bitmap.LockBits(new Rectangle(0, 0, (int)Width, (int)Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);

			var length = data.Stride * Height / 4;
			var ptr1 = (int*)data.Scan0.ToPointer();
	
			var argb = _byColor.ToArgb();
			var tr = _color.ToArgb();

			while ((length--) > 0)
			{
				if (*ptr1 == tr)
				{
					*ptr1 = argb;
				}
				ptr1++;
			}

			Bitmap.UnlockBits(data);
		}

		private unsafe void FillBackgroundByTransparentColor(BitmapData _data, Color _color)
		{
			var length = _data.Stride * Height / 4;
			var ptr1 = (int*)_data.Scan0.ToPointer();
			var argb = _color.ToArgb();

			var tr = *ptr1;
			while ((length--) > 0)
			{
				if (*ptr1 == tr)
				{
					*ptr1 = argb;
				}
				ptr1++;
			}
		}
	}
}