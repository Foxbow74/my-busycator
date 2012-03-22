using GameCore;
using GameCore.Misc;
using GameCore.PathFinding;
using GameUi;

namespace OpenTKUi
{
	internal class OpenTKTile : ATile
	{
		public static OpenTKGameProvider GameProvider { get; set; }

		public int X { get; private set; }
		public int Y { get; private set; }

		public OpenTKTile(ETextureSet _set, int _x, int _y, FColor _color) : base(_set, _x, _y, _color)
		{
			Texcoords = new TexCoord[4];
			X = _x;
			Y = _y;
		}

		public void UpdateTexCoords(int _x, int _y, float _imgWidth, float _imgHeight)
		{
			float u1 = 0.0f, u2 = 0.0f, v1 = 0.0f, v2 = 0.0f;

			if (_x != 0) u1 = 1.0f/(_imgWidth/_x/Size);
			if (Size != 0) u2 = 1.0f/(_imgWidth/Size);
			if (_y != 0) v1 = 1.0f/(_imgHeight/_y/Size);
			if (Size != 0) v2 = 1.0f/(_imgHeight/Size);

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

		public override void Draw(Point _point, FColor _color, EDirections _direction)
		{
			GameProvider.TileMapRenderer.DrawTile(this, _point.X, _point.Y, _color, _direction);
		}

		public override void Draw(Point _point, FColor _color)
		{
			GameProvider.TileMapRenderer.DrawTile(this, _point.X, _point.Y, _color, EDirections.DOWN);
		}

		public override void Draw(Point _point)
		{
			GameProvider.TileMapRenderer.DrawTile(this, _point.X, _point.Y, Color, EDirections.DOWN);
		}

		public override void FogIt(Point _point)
		{
			GameProvider.TileMapRenderer.FogTile(_point);
		}

		#region Nested type: TexCoord

		#endregion
	}

	internal struct TexCoord
	{
		public float U, V;
	}
}