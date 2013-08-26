using System;
using System.Drawing;
using GameCore;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameUi
{
	public interface IDrawHelper
	{
		void DrawRect(Rct _rct, FColor _toFColor);
		void DrawRect(RectangleF _rct, FColor _toFColor);
        void ClearTiles(Rct _rct, FColor _backgroundColor);
		SizeF MeasureString(EFonts _font, string _string);
		void DrawString(EFonts _font, string _string, float _x, float _y, FColor _color);
		void FogTile(Point _point);
	    IImageContainer CreateImageContainer(Bitmap _bitmap);
	}

    public interface IImageContainer:IDisposable
    {
        float Width { get; }
        float Height { get; }
    }
}