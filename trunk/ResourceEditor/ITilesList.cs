using GameUi;

namespace ResourceEditor
{
	public interface ITilesList
	{
		void AddTile(Tile _tile);
		void SetTile(ETextureSet _selectedItem, int _x, int _y);
	}
}