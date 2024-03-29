﻿using GameCore;
using GameCore.Misc;
using GameUi;

namespace OpenTKUi
{
	internal class OpenTKTile : ATile
	{
		public static OpenTKGameProvider GameProvider { get; set; }

		public int X { get; private set; }
		public int Y { get; private set; }

		public OpenTKTile(int _x, int _y, FColor _color) : base(_x, _y, _color)
		{
			Texcoords = new TexCoord[4];
			X = _x;
			Y = _y;
		}

		public void UpdateTexCoords(int _x, int _y, float _imgWidth, float _imgHeight)
		{
			Point = new Point(_x, _y);
			float u1 = 0.0f, u2 = 0.0f, v1 = 0.0f, v2 = 0.0f;

			if (_x != 0) u1 = 1.0f/(_imgWidth/_x/Constants.TILE_SIZE);
			if (Constants.TILE_SIZE != 0) u2 = 1.0f/(_imgWidth/Constants.TILE_SIZE);
			if (_y != 0) v1 = 1.0f/(_imgHeight/_y/Constants.TILE_SIZE);
			if (Constants.TILE_SIZE != 0) v2 = 1.0f/(_imgHeight/Constants.TILE_SIZE);

			Texcoords[0].U = u1;
			Texcoords[0].V = v1;
			Texcoords[1].U = u1 + u2;
			Texcoords[1].V = v1;
			Texcoords[2].U = u1 + u2;
			Texcoords[2].V = v1 + v2;
			Texcoords[3].U = u1;
			Texcoords[3].V = v1 + v2;
		}

		internal static OpenTKResourceProvider ResourceProvider { get; set; }

		public TexCoord[] Texcoords { get; private set; }

		public override void Draw(Point _point, FColor _color, EDirections _direction, bool _isCorpse)
		{
			GameProvider.TileMapRenderer.DrawTile(this, _point.X, _point.Y, _color, _direction, _isCorpse);
		}

		public override void Draw(Point _point, FColor _color)
		{
			GameProvider.TileMapRenderer.DrawTile(this, _point.X, _point.Y, _color, EDirections.DOWN, false);
		}

		public override void Draw(Point _point)
		{
			GameProvider.TileMapRenderer.DrawTile(this, _point.X, _point.Y, Color, EDirections.DOWN, false);
		}

		public override void FogIt(Point _point)
		{
			GameProvider.TileMapRenderer.FogTile(_point);
		}
	}

	internal struct TexCoord
	{
		public float U, V;
	}
}