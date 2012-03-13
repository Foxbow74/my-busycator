﻿using System;
using GameCore;
using GameCore.Misc;
using GameUi;

namespace ResourceEditor
{
	public class Tile:ATile
	{
		public int X { get; private set; }
		public int Y { get; private set; }

		public Tile(ETextureSet _set, int _x, int _y, FColor _color) : base(_set, _x, _y, _color)
		{
			X = _x;
			Y = _y;
		}

		public override void Draw(Point _point, FColor _color, EDirections _direction)
		{
			throw new NotImplementedException();
		}

		public override void Draw(Point _point, FColor _color)
		{
			throw new NotImplementedException();
		}

		public override void Draw(Point _point)
		{
			throw new NotImplementedException();
		}

		public override void FogIt(Point _point)
		{
			throw new NotImplementedException();
		}
	}
}