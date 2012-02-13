using System.Drawing;
using GameCore;

namespace GameUi
{
	public interface IDrawHelper
	{
		void ClearText(Rectangle _rectangle, FColor _toFColor);
		void ClearTiles(Rectangle _rectangle, FColor _backgroundColor);
		SizeF MeasureString(EFonts _font, string _string);
		void DrawString(EFonts _font, string _string, float _x, float _y, FColor _color);
		void FogTile(int _col, int _row);
	}
}