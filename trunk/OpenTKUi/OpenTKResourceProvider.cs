using System;
using System.Collections.Generic;
using System.Drawing;
using GameCore.Misc;
using GameUi;

namespace OpenTKUi
{
	internal class OpenTKResourceProvider : IResourceProvider, IDisposable
	{
		private readonly Dictionary<EFonts, Font> m_fonts = new Dictionary<EFonts, Font>();

		public OpenTKResourceProvider()
		{
			Images = new Dictionary<ETextureSet, Image>();
			Tiles = new List<OpenTKTile>();
			OpenTKTile.ResourceProvider = this;
		}

		public Image this[ETextureSet _set]
		{
			get { return Images[_set]; }
		}

		public Font this[EFonts _font]
		{
			get { return m_fonts[_font]; }
		}

		public List<OpenTKTile> Tiles { get; private set; }

		public Dictionary<ETextureSet, Image> Images { get; private set; }

		#region IDisposable Members

		public void Dispose()
		{
			foreach (var image in Images.Values)
			{
				image.Dispose();
			}
		}

		#endregion

		#region IResourceProvider Members

		public void RegisterTexture(ETextureSet _eTextureSet, string _fileName)
		{
			var image = new Image(_fileName);
			Images.Add(_eTextureSet, image);
		}

		public void RegisterFont(EFonts _font, string _fileName, int _pointSize)
		{
			m_fonts[_font] = new Font(_fileName, _pointSize);
		}

		public ATile CreateTile(ETextureSet _eTextureSet, int _col, int _row, Color _color)
		{
			var openTKTile = new OpenTKTile(_eTextureSet, _col, _row, _color.ToFColor());
			Tiles.Add(openTKTile);
			return openTKTile;
		}

		public ATile CreateTile(int _col, int _row, Color _color)
		{
			return CreateTile(ETextureSet.REDJACK, _col, _row, _color);
		}

		#endregion
	}
}