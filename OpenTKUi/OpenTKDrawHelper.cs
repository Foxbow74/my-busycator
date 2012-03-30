using System;
using System.Collections.Generic;
using System.Drawing;
using GameCore;
using GameCore.Misc;
using GameUi;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using QuickFont;
using Point = GameCore.Misc.Point;

namespace OpenTKUi
{
	internal class OpenTKDrawHelper : IDrawHelper, IDisposable
	{
		private readonly OpenTKResourceProvider m_resourceProvider;
		private readonly OpenTKGameProvider m_gameProvider;
		//private bool m_isTextBitmapChanged;

		//private Image m_textImage;

		public OpenTKDrawHelper(OpenTKResourceProvider _resourceProvider, OpenTKGameProvider _gameProvider)
		{

			m_gameProvider = _gameProvider;
			m_resourceProvider = _resourceProvider;
		}

		#region IDisposable Members

		public void Dispose()
		{
			//m_textImage.Dispose();
		}

		#endregion

		#region IDrawHelper Members

		public void ClearTiles(Rct _rct, FColor _backgroundColor)
		{
			if(m_gameProvider.TileMapRenderer==null) return;
			m_gameProvider.TileMapRenderer.Clear(_rct, _backgroundColor);
		}

		public System.Drawing.SizeF MeasureString(EFonts _font, string _string)
		{
			var qFont = m_resourceProvider[_font];
			return qFont.Measure(_string);
		}

		private readonly Dictionary<EFonts, QFont> m_qfonts = new Dictionary<EFonts, QFont>();

		public void DrawString(EFonts _font, string _string, float _x, float _y, FColor _color)
		{
			var qFont = m_resourceProvider[_font];
			QFont.Begin();
			qFont.Options.Colour = new Color4(_color.R, _color.G, _color.B, _color.A);
			qFont.Print(_string, new OpenTK.Vector2(_x, _y));
			QFont.End();
		}

		public void FogTile(Point _point)
		{
			m_gameProvider.TileMapRenderer.FogTile(_point);
		}

		public void DrawRect(Rct _rct, FColor _toFColor)
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.Color4(_toFColor.R, _toFColor.G, _toFColor.B, _toFColor.A);

			GL.Begin(BeginMode.Quads);
			GL.Vertex2(_rct.Left, _rct.Top);
			GL.Vertex2(_rct.Right + 1, _rct.Top);
			GL.Vertex2(_rct.Right + 1, _rct.Bottom + 1);
			GL.Vertex2(_rct.Left, _rct.Bottom + 1);
			GL.End();
		}

		public void DrawRect(RectangleF _rct, FColor _toFColor)
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.Color4(_toFColor.R, _toFColor.G, _toFColor.B, _toFColor.A);

			GL.Begin(BeginMode.Quads);
			GL.Vertex2(_rct.Left, _rct.Top);
			GL.Vertex2(_rct.Right + 1, _rct.Top);
			GL.Vertex2(_rct.Right + 1, _rct.Bottom + 1);
			GL.Vertex2(_rct.Left, _rct.Bottom + 1);
			GL.End();
		}

		#endregion

		public static void DrawTexture(Image _image)
		{
			GL.BindTexture(TextureTarget.Texture2D, _image.Texture);

			GL.Color4(1f, 1f, 1f, 1f);

			GL.Begin(BeginMode.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex2(0, 0);
			GL.TexCoord2(1f, 0);
			GL.Vertex2(_image.Width, 0);
			GL.TexCoord2(1f, 1f);
			GL.Vertex2(_image.Width, _image.Height);
			GL.TexCoord2(0, 1f);
			GL.Vertex2(0, _image.Height);
			GL.End();
		}

		public void Resize(int _width, int _height)
		{
			QFont.RefreshViewport();
			//if (m_textImage != null) m_textImage.Dispose();
			//var bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppPArgb);
			//m_textImage = new Image(bitmap, false);
		}
	}
}