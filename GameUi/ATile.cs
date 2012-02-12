using System.Drawing;
using GameCore;

namespace GameUi
{
	public abstract class ATile
	{
		public static int Size = 16;
		public readonly ETextureSet Set;

		protected ATile(ETextureSet _set, int _x, int _y, Color _color)
		{
			Set = _set;
			Color = _color;
			Rectangle = new Rectangle(_x*Size, _y*Size, Size, Size);
		}

		protected ATile()
		{
		}
		
		public Color Color { get; private set; }

		public Rectangle Rectangle { get; private set; }

		public bool IsFogTile { get; set; }

		public abstract void Draw(int _x, int _y, Color _color);

		public abstract void DrawFog(int _col, int _row, Color _color);
	}
}