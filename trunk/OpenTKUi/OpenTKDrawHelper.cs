using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using GameUi;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace OpenTKUi
{
	internal class OpenTKDrawHelper : IDrawHelper, IDisposable
	{
		private readonly OpenTKResourceProvider m_resourceProvider;
		private OpenTKGameProvider m_gameProvider;
		private bool m_isTextBitmapChanged;

		private Image m_textImage;

		public OpenTKDrawHelper(OpenTKResourceProvider _resourceProvider, OpenTKGameProvider _gameProvider)
		{
			m_gameProvider = _gameProvider;
			m_resourceProvider = _resourceProvider;
			var bitmap = new Bitmap(_gameProvider.Width, _gameProvider.Height, PixelFormat.Format32bppArgb);
			m_textImage = new Image(bitmap, false);
		}

		#region IDisposable Members

		public void Dispose()
		{
			m_textImage.Dispose();
		}

		#endregion

		#region IDrawHelper Members

		public void ClearTiles(Rectangle _rectangle, Color _backgroundColor)
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);
			GL.Color4(_backgroundColor.R, _backgroundColor.G, _backgroundColor.B, _backgroundColor.A);
			GL.Begin(BeginMode.Quads);
			GL.Vertex2(_rectangle.Left * m_gameProvider.TileSizeX, _rectangle.Top * m_gameProvider.TileSizeY);
			GL.Vertex2(_rectangle.Right * m_gameProvider.TileSizeX, _rectangle.Top * m_gameProvider.TileSizeY);
			GL.Vertex2(_rectangle.Right * m_gameProvider.TileSizeX, _rectangle.Bottom * m_gameProvider.TileSizeY);
			GL.Vertex2(_rectangle.Left * m_gameProvider.TileSizeX, _rectangle.Bottom * m_gameProvider.TileSizeY);
			GL.End();
			m_gameProvider.TileMapRenderer.Clear(_rectangle);
		}

		public SizeF MeasureString(EFonts _font, string _string)
		{
			var font = m_resourceProvider[_font];
			using (var gr = Graphics.FromImage(m_textImage.Bitmap))
			{
				gr.SmoothingMode = SmoothingMode.HighSpeed;
				return gr.MeasureString(_string, font);
			}
		}

		public void DrawString(EFonts _font, string _string, float _x, float _y, Color _color)
		{
			var font = m_resourceProvider[_font];
			using (var gr = Graphics.FromImage(m_textImage.Bitmap))
			{
				gr.SmoothingMode = SmoothingMode.HighSpeed;
				//gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				using (var br = new SolidBrush(_color))
				{
					gr.DrawString(_string, font, br, _x, _y);
				}
			}
			m_isTextBitmapChanged = true;
		}

		public void ClearText(Rectangle _rectangle, Color _backgroundColor)
		{
			using (var gr = Graphics.FromImage(m_textImage.Bitmap))
			{
				gr.Clip = new Region(_rectangle);
				gr.Clear(Color.Empty);
			}
			m_isTextBitmapChanged = true;
		}

		#endregion

		public void ClearText()
		{
			using (var gr = Graphics.FromImage(m_textImage.Bitmap))
			{
				gr.Clear(Color.Empty);
			}
			m_isTextBitmapChanged = true;
		}

		public void DrawTextBitmap()
		{
			//return;
			if (m_isTextBitmapChanged)
			{
				m_textImage.Update();
				m_isTextBitmapChanged = false;
			}

			DrawTexture(m_textImage);
		}

		public static void DrawTexture(Image _image)
		{
			GL.BindTexture(TextureTarget.Texture2D, _image.Texture);

			GL.Color4(1f, 1f, 1f, 1f);
			GL.BlendEquation(BlendEquationMode.FuncAdd);

			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Begin(BeginMode.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex2(0, 0);
			GL.TexCoord2(1f, 0);
			GL.Vertex2(_image.Width, 0);
			//GL.Color4(1f, 1f, 0f, 0f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex2(_image.Width, _image.Height);
			GL.TexCoord2(0, 1f);
			GL.Vertex2(0, _image.Height);
			GL.End();
		}

		public void Resize(int _width, int _height)
		{
			if (m_textImage != null) m_textImage.Dispose();
			var bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);
			m_textImage = new Image(bitmap, false);
		}
	}
}