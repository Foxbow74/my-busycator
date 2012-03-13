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
			Rct = new Rct(_x*Size, _y*Size, Size, Size);
		}

		protected ATile()
		{
		}
		
		public FColor Color { get; set; }

		public Rct Rct { get; private set; }

		public bool IsFogTile { get; set; }

		public virtual object Tile { get; set; }

		public abstract void Draw(Point _point, FColor _color, EDirections _direction);
		public abstract void Draw(Point _point, FColor _color);
		public abstract void Draw(Point _point);
		public abstract void FogIt(Point _point);

		public string ToText()
		{
			return string.Format("{0}|{4}|{1},{2}|{3}", Tile, Rct.Left/Size, Rct.Top/Size, Color.ToShortText(), Set);
		}
	}
}