using System.Drawing;
using GameCore;
using GameUi;
using OpenTK.Graphics.OpenGL;

namespace OpenTKUi
{
	internal class OpenTKTile : ATile
	{
		public int X { get; private set; }
		public int Y { get; private set; }

		private readonly Image m_image;
		private readonly Vertex[] m_vertices = new Vertex[4]; // Image vertices

		public OpenTKTile(ETextureSet _set, int _x, int _y, Color _color) : base(_set, _x, _y, _color)
		{
			Texcoords = new TexCoord[4];
			X = _x;
			Y = _y;
			m_image = ResourceProvider[_set];

			UpdateTexCoords(_x, _y, m_image.Width, m_image.Height);
		}

		public void UpdateTexCoords(int _x, int _y, float _imgWidth, float _imgHeight)
		{
			float u1 = 0.0f, u2 = 0.0f, v1 = 0.0f, v2 = 0.0f;

			// Calculate coordinates, prevent dividing by zero
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

		public static TileMapRenderer TileMapRenderer { get; set; }

		public TexCoord[] Texcoords { get; private set; }

		public override void Draw(int _x, int _y, Color _color)
		{
			TileMapRenderer.DrawTile(this, _x/16, _y/16, _color);
			return;
			m_vertices[0].X = _x;
			m_vertices[0].Y = _y + Size;
			m_vertices[1].X = _x + Size;
			m_vertices[1].Y = _y + Size;
			m_vertices[2].X = _x + Size;
			m_vertices[2].Y = _y;
			m_vertices[3].X = _x;
			m_vertices[3].Y = _y;

			var opacity = _color.A/255f;

			GL.Color4(1f, 1f, 1f, opacity);

			GL.BindTexture(TextureTarget.Texture2D, m_image.Texture);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.BlendEquation(BlendEquationMode.FuncReverseSubtract);
			DrawQuad();
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			if (_color != Color.Black)
			{
				GL.Color4(_color.R / 255f, _color.G / 255f, _color.B / 255f, opacity);
				DrawQuad();
			}
		}

		public override void DrawFog(int _col, int _row, Color _color)
		{
			TileMapRenderer.FogTile(this, _col, _row, _color);
		}

		public void DrawQuad()
		{
			GL.Begin(BeginMode.Quads);

			for (var i = 0; i < 4; i++)
			{
				GL.TexCoord2(Texcoords[i].U, Texcoords[i].V);
				GL.Vertex2(m_vertices[i].X, m_vertices[i].Y);
			}

			GL.End();
		}

		#region Nested type: TexCoord

		public struct TexCoord
		{
			public float U, V;
		}

		#endregion

		#region Nested type: Vertex

		public struct Vertex
		{
			public float X, Y;
		}

		#endregion
	}
}