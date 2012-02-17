using GameCore;
using GameCore.Misc;

namespace GameUi
{
	public abstract class ATile
	{
		public static int Size = 16;
		public readonly ETextureSet Set;

		protected ATile(ETextureSet _set, int _x, int _y, FColor _color)
		{
			Set = _set;
			Color = _color;
			Rectangle = new System.Drawing.Rectangle(_x*Size, _y*Size, Size, Size);
		}

		protected ATile()
		{
		}
		
		public FColor Color { get; private set; }

		public System.Drawing.Rectangle Rectangle { get; private set; }

		public bool IsFogTile { get; set; }

		public object Tile { get; set; }

		public abstract void Draw(int _x, int _y, FColor _color, FColor _background);
		public abstract void Draw(Point _point, FColor _color, FColor _background);
		public abstract void FogIt(int _col, int _row);
	}
}