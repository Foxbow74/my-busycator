using System;
using System.Collections.Generic;
using System.Drawing;
using GameUi;
using Image = OpenTKUi.Image;

namespace OpenTKUi
{
	internal class OpenTKResourceProvider : IResourceProvider, IDisposable
	{
		private readonly Dictionary<EFonts, Font> m_fonts = new Dictionary<EFonts, Font>();
		private readonly OpenTKGameProvider m_gameProvider;
		private readonly Dictionary<ETextureSet, Image> m_surfaces = new Dictionary<ETextureSet, Image>();

		public OpenTKResourceProvider(OpenTKGameProvider _gameProvider)
		{
			m_gameProvider = _gameProvider;
			OpenTKTile.ResourceProvider = this;
		}

		public Image this[ETextureSet _set]
		{
			get { return m_surfaces[_set]; }
		}

		public Font this[EFonts _font]
		{
			get { return m_fonts[_font]; }
		}

		#region IResourceProvider Members

		public void RegisterTexture(ETextureSet _eTextureSet, string _fileName)
		{
			var image = new Image(_fileName);
			m_surfaces.Add(_eTextureSet, image);
		}

		public void RegisterFont(EFonts _font, string _fileName, int _pointSize)
		{
			m_fonts[_font] = new Font(_fileName, _pointSize);
		}

		public ATile CreateTile(ETextureSet _eTextureSet, int _col, int _row, Color _color)
		{
			return new OpenTKTile(_eTextureSet, _col, _row, _color);
		}

		public ATile CreateTile(int _col, int _row, Color _color)
		{
			return CreateTile(ETextureSet.REDJACK, _col, _row, _color);
		}

		#endregion

		public void Dispose()
		{
			foreach (var image in m_surfaces.Values)
			{
				image.Dispose();
			}
		}
	}
}