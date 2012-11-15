using GameCore;

namespace GameUi
{
	public interface IResourceProvider
	{
		void RegisterFont(EFonts _font, string _fileName, int _pointSize);

		ATile CreateTile(int _col, int _row, FColor _color);
	}
}