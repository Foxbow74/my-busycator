using System;
using System.Drawing;
using GameUi;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace OpenTKUi
{
	internal class OpenTKDrawHelper : IDrawHelper, IDisposable
	{
		private readonly OpenTKResourceProvider m_resourceProvider;
		private Image m_textImage;

		private bool m_isTextBitmapChanged;

		public OpenTKDrawHelper(OpenTKResourceProvider _resourceProvider, OpenTKGameProvider _gameProvider)
		{
			m_resourceProvider = _resourceProvider;
			var bitmap = new Bitmap(_gameProvider.Width, _gameProvider.Height, PixelFormat.Format32bppArgb);
			m_textImage = new Image(bitmap, false);
		}

		#region IDrawHelper Members

		public void Clear(Rectangle _rectangle, Color _backgroundColor)
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);
			GL.Color4(_backgroundColor.R, _backgroundColor.G, _backgroundColor.B, _backgroundColor.A);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Begin(BeginMode.Quads);
			GL.Vertex2(_rectangle.Left , _rectangle.Top );
			GL.Vertex2(_rectangle.Right , _rectangle.Top );
			GL.Vertex2(_rectangle.Right , _rectangle.Bottom );
			GL.Vertex2(_rectangle.Left , _rectangle.Bottom );
			GL.End();
			using (var gr = Graphics.FromImage(m_textImage.Bitmap))
			{
				gr.Clip = new Region(new Rectangle(_rectangle.Left , _rectangle.Top , _rectangle.Width , _rectangle.Height ));
				gr.Clear(Color.Empty);
			}
			m_isTextBitmapChanged = true;
		}

		public SizeF MeasureString(EFonts _font, string _string)
		{
			var font = m_resourceProvider[_font];
			using (var gr = Graphics.FromImage(m_textImage.Bitmap))
			{
				return gr.MeasureString(_string, font);
			}
		}

		public void DrawString(EFonts _font, string _string, float _x, float _y, Color _color)
		{
			var font = m_resourceProvider[_font];
			using (var gr = Graphics.FromImage(m_textImage.Bitmap))
			{
				gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				using (var br = new SolidBrush(_color))
				{
					gr.DrawString(_string, font, br, _x, _y);
				}
			}
			m_isTextBitmapChanged = true;
		}

		#endregion

		public void ClearText(Rectangle _rectangle, Color _backgroundColor)
		{
			using (var gr = Graphics.FromImage(m_textImage.Bitmap))
			{
				gr.Clear(Color.Empty);
			}
		}

		public void DrawTextBitmap()
		{
			//return;
			if (m_isTextBitmapChanged)
			{
				m_textImage.Update();
				m_isTextBitmapChanged = false;
			}
			GL.BindTexture(TextureTarget.Texture2D, m_textImage.Texture);

			GL.Color4(1f,1f, 1f, 1f);

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Begin(BeginMode.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex2(0, 0);
			GL.TexCoord2(1f, 0);
			GL.Vertex2(m_textImage.Width, 0);
			GL.TexCoord2(1f, 1f);
			GL.Vertex2(m_textImage.Width, m_textImage.Height);
			GL.TexCoord2(0, 1f);
			GL.Vertex2(0, m_textImage.Height);
			GL.End();
		}

		public void Dispose()
		{
			m_textImage.Dispose();
		}

		public void Resize(int _width, int _height)
		{
			if(m_textImage!=null) m_textImage.Dispose();
			var bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);
			m_textImage = new Image(bitmap, false);
		}
	}
}