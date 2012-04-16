using System;
using System.Collections.Generic;
using GameCore;
using GameCore.PathFinding;
using OpenTK.Graphics.OpenGL;

namespace OpenTKUi
{
	class TileInfo
	{
		public static TexCoord[] FogTexCoords;

		private readonly int m_x;
		private readonly int m_y;
		private readonly int m_width;
		private readonly int m_height;

		public bool IsVisible { get; set; }

		private readonly List<OpenTKTile> m_tiles = new List<OpenTKTile>();
		private readonly List<FColor> m_colors = new List<FColor>();
		private readonly List<EDirections> m_directions = new List<EDirections>();

		public int Layers { get; private set; }

		public bool IsFogged { get; set; }

		public TileInfo(int _x, int _y, int _width, int _height)
		{
			m_x = _x * _width;
			m_y = _y * _height;
			m_width = _width;
			m_height = _height;
		}

		public void Draw(int _iteration, bool _colored, bool _fogOnly, int _layer)
		{
			if (m_tiles == null && !_fogOnly) return;
			if (IsFogged)
			{
				
			}
			if (_fogOnly && !IsFogged) return;


			TexCoord[] texcoords;
			var color = m_colors[_layer];
			var dir = EDirections.DOWN;
			if (_fogOnly)
			{
				if (IsFogged)
				{
					texcoords = FogTexCoords;
					color = FColor.Black;
				}
				else
				{
					return;
				}
			}
			else
			{
				texcoords = m_tiles[_layer].Texcoords;
				dir = m_directions[_layer];
			}
			if(_colored)
			{
				GL.Color4(color.R, color.G, color.B, color.A);
			}
			else
			{
				GL.Color4(1f, 1f, 1f, color.A);
			}

			switch (dir)
			{
				case EDirections.UP:
					GL.TexCoord2(texcoords[2].U, texcoords[2].V);
					GL.Vertex2(m_x, m_y);
					GL.TexCoord2(texcoords[3].U, texcoords[3].V);
					GL.Vertex2(m_x + m_width, m_y);
					GL.TexCoord2(texcoords[0].U, texcoords[0].V);
					GL.Vertex2(m_x + m_width, m_y + m_height);
					GL.TexCoord2(texcoords[1].U, texcoords[1].V);
					GL.Vertex2(m_x, m_y + m_height);
					break;
				case EDirections.DOWN:
					GL.TexCoord2(texcoords[0].U, texcoords[0].V);
					GL.Vertex2(m_x, m_y);
					GL.TexCoord2(texcoords[1].U, texcoords[1].V);
					GL.Vertex2(m_x + m_width, m_y);
					GL.TexCoord2(texcoords[2].U, texcoords[2].V);
					GL.Vertex2(m_x + m_width, m_y + m_height);
					GL.TexCoord2(texcoords[3].U, texcoords[3].V);
					GL.Vertex2(m_x, m_y + m_height);
					break;
				case EDirections.RIGHT:
					GL.TexCoord2(texcoords[1].U, texcoords[1].V);
					GL.Vertex2(m_x, m_y);
					GL.TexCoord2(texcoords[2].U, texcoords[2].V);
					GL.Vertex2(m_x + m_width, m_y);
					GL.TexCoord2(texcoords[3].U, texcoords[3].V);
					GL.Vertex2(m_x + m_width, m_y + m_height);
					GL.TexCoord2(texcoords[0].U, texcoords[0].V);
					GL.Vertex2(m_x, m_y + m_height);
					break;
				case EDirections.LEFT:
					GL.TexCoord2(texcoords[3].U, texcoords[3].V);
					GL.Vertex2(m_x, m_y);
					GL.TexCoord2(texcoords[0].U, texcoords[0].V);
					GL.Vertex2(m_x + m_width, m_y);
					GL.TexCoord2(texcoords[1].U, texcoords[1].V);
					GL.Vertex2(m_x + m_width, m_y + m_height);
					GL.TexCoord2(texcoords[2].U, texcoords[2].V);
					GL.Vertex2(m_x, m_y + m_height);					
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Clear()
		{
			m_tiles.Clear();
			m_colors.Clear();
			m_directions.Clear();
			Layers = 0;
		}

		public void AddLayer(OpenTKTile _tile, FColor _color, EDirections _direction)
		{
			m_tiles.Add(_tile);
			m_colors.Add(_color);
			m_directions.Add(_direction);
			Layers++;
		}
	}
}