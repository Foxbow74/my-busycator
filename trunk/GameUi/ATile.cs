using GameCore;
using GameCore.Misc;

namespace GameUi
{
	public abstract class ATile
	{
		protected ATile(int _x, int _y, FColor _color)
		{
			Color = _color;
			Point = new Point(_x, _y);
			SrcPoint = new Point(_x, _y);
		}

		protected ATile() { }
		public FColor Color { get; set; }

		public Point Point { get; protected set; }
		public Point SrcPoint { get; protected set; }
		public bool IsFogTile { get; set; }

		public abstract void Draw(Point _point, FColor _color, EDirections _direction);
		public abstract void Draw(Point _point, FColor _color);
		public abstract void Draw(Point _point);
		public abstract void FogIt(Point _point);

        public string ToResurceText() { return string.Format("{0}|{1}|{2}", Point.X, Point.Y, Color.ToShortText()); }
    }
}