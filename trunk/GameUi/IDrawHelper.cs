using System.Drawing;
using GameCore;
using GameCore.Misc;

namespace GameUi
{
	public interface IDrawHelper
	{
		void ClearText(Rct _rct, FColor _toFColor);
		void ClearTiles(Rct _rct, FColor _backgroundColor);
		SizeF MeasureString(EFonts _font, string _string);
		void DrawString(EFonts _font, string _string, float _x, float _y, FColor _color);
		void FogTile(int _col, int _row);
	}
}