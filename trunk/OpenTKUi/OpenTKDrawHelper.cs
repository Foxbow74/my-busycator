using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using GameCore;
using GameCore.Misc;
using GameUi;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using QuickFont;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = GameCore.Misc.Point;
using Vector2 = OpenTK.Vector2;

namespace OpenTKUi
{
	internal class OpenTKDrawHelper : IDrawHelper, IDisposable
	{
		private readonly OpenTKResourceProvider m_resourceProvider;
		private readonly OpenTKGameProvider m_gameProvider;
		private bool m_isTextBitmapChanged;

		private Image m_textImage;

		public OpenTKDrawHelper(OpenTKResourceProvider _resourceProvider, OpenTKGameProvider _gameProvider)
		{

			m_gameProvider = _gameProvider;
			m_resourceProvider = _resourceProvider;
			var bitmap = new Bitmap(_gameProvider.Width, _gameProvider.Height, PixelFormat.Format32bppPArgb);
			m_textImage = new Image(bitmap, false);
		}

		#region IDisposable Members

		public void Dispose()
		{
			m_textImage.Dispose();
		}

		#endregion

		#region IDrawHelper Members

		public void ClearTiles(Rct _rct, FColor _backgroundColor)
		{
			if(m_gameProvider.TileMapRenderer==null) return;
			m_gameProvider.TileMapRenderer.Clear(_rct, _backgroundColor);
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

		private Dictionary<EFonts, QFont> m_qfonts = new Dictionary<EFonts, QFont>();

		public void DrawString(EFonts _font, string _string, float _x, float _y, FColor _color)
		{
			QFont value;
			if(!m_qfonts.TryGetValue(_font, out value))
			{
				var charSet = new List<char>();
				for (var c = ' '; c < '}'; ++c)
				{
					
						charSet.Add(c);
					
				}
				for (var c = 'А'; c <= 'я'; ++c)
				{
					
						charSet.Add(c);
					
				}
				var s = new string(charSet.ToArray());
				var qfc = new QFontBuilderConfiguration() { charSet = s, ShadowConfig = new QFontShadowConfiguration() { }, TextGenerationRenderHint = TextGenerationRenderHint.AntiAlias};
				value = new QFont(m_resourceProvider[_font], qfc);
				m_qfonts[_font] = value;
			}
			QFont.Begin();
			value.Options.Colour = new Color4(_color.R, _color.G, _color.B, _color.A);
			value.Print(_string, new Vector2(_x, _y));
			QFont.End();
		}

		public void FogTile(Point _point)
		{
			m_gameProvider.TileMapRenderer.FogTile(_point);
		}

		public void ClearText(Rct _rct, FColor _toFColor)
		{
			using (var gr = Graphics.FromImage(m_textImage.Bitmap))
			{
				gr.Clip = new Region(new Rectangle(_rct.Left, _rct.Top, _rct.Width, _rct.Height));
				gr.Clear(_toFColor.ToColor());
			}
			m_isTextBitmapChanged = true;
		}

		#endregion

		public void DrawTextBitmap()
		{
			//if (m_isTextBitmapChanged)
			//{
			//    m_textImage.Update();
			//    m_isTextBitmapChanged = false;
			//}

			//DrawTexture(m_textImage);
		}

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
			if (m_textImage != null) m_textImage.Dispose();
			var bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppPArgb);
			m_textImage = new Image(bitmap, false);
		}
	}
}