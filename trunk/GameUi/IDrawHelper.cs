using GameCore;
using GameCore.Misc;

namespace GameUi
{
	public interface IDrawHelper
	{
		void DrawRect(Rct _rct, FColor _toFColor);
		void ClearTiles(Rct _rct, FColor _backgroundColor);
		System.Drawing.SizeF MeasureString(EFonts _font, string _string);
		void DrawString(EFonts _font, string _string, float _x, float _y, FColor _color);
		void FogTile(Point _point);
	}
}