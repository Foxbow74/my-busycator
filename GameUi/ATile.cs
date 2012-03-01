﻿using GameCore;
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
		
		public FColor Color { get; private set; }

		public Rct Rct { get; private set; }

		public bool IsFogTile { get; set; }

		public object Tile { get; set; }

		public abstract void Draw(Point _point, FColor _color, EDirections _direction);
		public abstract void Draw(Point _point, FColor _color);
		public abstract void FogIt(Point _point);
	}
}