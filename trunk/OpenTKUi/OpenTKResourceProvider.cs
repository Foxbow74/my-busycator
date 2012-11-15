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
			Tiles = new List<OpenTKTile>();
			OpenTKTile.ResourceProvider = this;
		}

		public QFont this[EFonts _font]
		{
			get { return m_fonts[_font]; }
		}

		public List<OpenTKTile> Tiles { get; private set; }

		#region IResourceProvider Members

		public void RegisterFont(EFonts _font, string _fileName, int _pointSize)
		{
            if(!File.Exists(_fileName))
            {
                throw new ApplicationException("Не найден файл " + Path.GetFullPath(_fileName));
            }
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
			var qfc = new QFontBuilderConfiguration
			              {
			          		charSet = s,
                            TextGenerationRenderHint = TextGenerationRenderHint.SizeDependent | TextGenerationRenderHint.AntiAlias,
							SuperSampleLevels = 4,
			          	};
			var qFont = new QFont(_fileName, _pointSize, FontStyle.Bold, qfc);

			m_fonts[_font] = qFont;
		}

		public ATile CreateTile(int _col, int _row, FColor _color)
		{
			var openTKTile = new OpenTKTile(_col, _row, _color);
			Tiles.Add(openTKTile);
			return openTKTile;
		}

		#endregion

		public void Dispose()
		{
		}
	}
}