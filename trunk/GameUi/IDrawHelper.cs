using System.Drawing;

namespace GameUi
{
	public interface IDrawHelper
	{
		void Clear(Rectangle _rectangle, Color _backgroundColor);
		SizeF MeasureString(EFonts _font, string _string);
		void DrawString(EFonts _font, string _string, float _x, float _y, Color _color);
	}
}