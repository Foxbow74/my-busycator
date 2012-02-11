using System.Drawing;
using GameUi;
using OpenTK.Graphics.OpenGL;

namespace OpenTKUi
{
	internal class OpenTKTile : ATile
	{
		private readonly Image m_image;
		private readonly TexCoord[] m_texcoords = new TexCoord[4]; // Image texture coordinates

		private readonly Vertex[] m_vertices = new Vertex[4]; // Image vertices

		public OpenTKTile(ETextureSet _set, int _x, int _y, Color _color) : base(_set, _x, _y, _color)
		{
			m_image = ResourceProvider[_set];

			float u1 = 0.0f, u2 = 0.0f, v1 = 0.0f, v2 = 0.0f;

			// Calculate coordinates, prevent dividing by zero
			if (_x != 0) u1 = 1.0f/(m_image.Width/_x/Size);
			if (Size != 0) u2 = 1.0f/(m_image.Width/Size);
			if (_y != 0) v1 = 1.0f/(m_image.Height/_y/Size);
			if (Size != 0) v2 = 1.0f/(m_image.Height/Size);

			m_texcoords[0].U = u1;
			m_texcoords[0].V = v1 + v2;
			m_texcoords[1].U = u1 + u2;
			m_texcoords[1].V = v1 + v2;
			m_texcoords[2].U = u1 + u2;
			m_texcoords[2].V = v1;
			m_texcoords[3].U = u1;
			m_texcoords[3].V = v1;
		}

		internal static OpenTKResourceProvider ResourceProvider { get; set; }

		/// <summary>
		/// 	Draws VBO.
		/// </summary>
		/// <param name = "_lenght">Number of vertices to be drawn from array.</param>
		/// <param name = "_mode">Mode used for drawing.</param>
		public void Draw(int _lenght, BeginMode _mode)
		{
			GL.Begin(_mode);

			for (var i = 0; i < _lenght; i++)
			{
				GL.TexCoord2(m_texcoords[i].U, m_texcoords[i].V);
				GL.Vertex2(m_vertices[i].X, m_vertices[i].Y);
			}

			GL.End();
		}

		public override void Draw(int _x, int _y, Color _color)
		{
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

		public void DrawQuad()
		{
			GL.Begin(BeginMode.Quads);

			for (var i = 0; i < 4; i++)
			{
				GL.TexCoord2(m_texcoords[i].U, m_texcoords[i].V);
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