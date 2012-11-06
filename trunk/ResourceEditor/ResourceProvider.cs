using System.Collections.Generic;
using System.Drawing;
using GameCore;
using GameUi;

namespace ResourceEditor
{
	public class ResourceProvider:IResourceProvider
	{
		readonly Dictionary<EFonts, string> m_fonts = new Dictionary<EFonts, string>();

		public ResourceProvider()
		{
			TextureSets = new Dictionary<ETextureSet, Bitmap>();
		}

		public Dictionary<ETextureSet, Bitmap> TextureSets { get; private set; }

		public Dictionary<ETiles, ATile> Tiles { get; private set; }

		public void RegisterTexture(ETextureSet _eTextureSet, string _fileName)
		{
			var fromFile = Image.FromFile(_fileName);
			var bitmap = new Bitmap(fromFile);
			TextureSets[_eTextureSet] = bitmap;
		}

		public void RegisterFont(EFonts _font, string _fileName, int _pointSize)
		{
			m_fonts[_font] = _fileName;
		}

		public ATile CreateTile(ETextureSet _eTextureSet, int _col, int _row, FColor _color)
		{
			var tile = new Tile(_eTextureSet, _col, _row, _color);
			return tile;
		}

		public ATile CreateTile(int _col, int _row, FColor _color)
		{
			var tile = new Tile(ETextureSet.RJ, _col, _row, _color);
			return tile;
		}
	}
}
