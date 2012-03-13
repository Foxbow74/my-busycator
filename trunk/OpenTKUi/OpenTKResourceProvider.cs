using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using GameCore;
using GameUi;
using QuickFont;

namespace OpenTKUi
{
	internal class OpenTKResourceProvider : IResourceProvider, IDisposable
	{
		private readonly Dictionary<EFonts, QFont> m_fonts = new Dictionary<EFonts, QFont>();

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

		public QFont this[EFonts _font]
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
			var qfc = new QFontBuilderConfiguration()
			          	{
			          		charSet = s, 
							TextGenerationRenderHint = TextGenerationRenderHint.SizeDependent,
							SuperSampleLevels = 1,
			          	};
			var qFont = new QFont(_fileName, _pointSize, FontStyle.Regular, qfc);

			m_fonts[_font] = qFont;
		}

		public ATile CreateTile(ETextureSet _eTextureSet, int _col, int _row, FColor _color)
		{
			var openTKTile = new OpenTKTile(_eTextureSet, _col, _row, _color);
			Tiles.Add(openTKTile);
			return openTKTile;
		}

		public ATile CreateTile(int _col, int _row, FColor _color)
		{
			return CreateTile(ETextureSet.RJ, _col, _row, _color);
		}

		#endregion
	}
}