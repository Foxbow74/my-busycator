using System;
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

		public SizeF MeasureString(EFonts _font, string _string)
		{
			var qFont = m_resourceProvider[_font];
			return qFont.Measure(_string);
		}

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

	    public IImageContainer CreateImageContainer(Bitmap _bitmap)
	    {
	        return new Image(_bitmap, false);
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
	}
}