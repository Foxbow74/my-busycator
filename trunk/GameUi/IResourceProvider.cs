using GameCore;

namespace GameUi
{
	public interface IResourceProvider
	{
		void RegisterTexture(ETextureSet _eTextureSet, string _fileName);
		void RegisterFont(EFonts _font, string _fileName, int _pointSize);

		ATile CreateTile(ETextureSet _eTextureSet, int _col, int _row, FColor _color);
		ATile CreateTile(int _col, int _row, FColor _color);
	}
}