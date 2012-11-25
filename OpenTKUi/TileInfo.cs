using System;
using System.Collections.Generic;
using GameCore;
using OpenTK.Graphics.OpenGL;

namespace OpenTKUi
{
	struct LayerInfo
	{
		public OpenTKTile Tile;
		public EDirections Direction;
		public FColor Color;
		public bool IsCorpse;
	}
	class TileInfo
	{
		public static TexCoord[] FogTexCoords;

		private readonly int m_x;
		private readonly int m_y;
		private readonly int m_width;
		private readonly int m_height;

		public bool IsVisible { get; set; }

		private readonly List<LayerInfo> m_layers = new List<LayerInfo>();

		public int Layers { get { return m_layers.Count; } }

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
			var layer = m_layers[_layer];
			if (layer.Tile == null && !_fogOnly) return;
			if (_fogOnly && !IsFogged) return;


			TexCoord[] texcoords;
			if (_fogOnly)
			{
				if (IsFogged)
				{
					texcoords = FogTexCoords;
					layer.Color = FColor.Black;
				}
				else
				{
					return;
				}
			}
			else
			{
				texcoords = layer.Tile.Texcoords;
			}
			if(_colored)
			{
				GL.Color4(layer.Color.R, layer.Color.G, layer.Color.B, layer.Color.A);
			}
			else
			{
				GL.Color4(1f, 1f, 1f, layer.Color.A);
			}


			if(layer.IsCorpse)
			{
				var d1 = 2f / 16f;
				var d2 = d1*3f;

				switch (layer.Direction)
				{
					case EDirections.UP:
						GL.TexCoord2(texcoords[2].U, texcoords[2].V); GL.Vertex2(m_x + m_width * d1, m_y + m_height*d2);
						GL.TexCoord2(texcoords[3].U, texcoords[3].V); GL.Vertex2(m_x + m_width * (1 - d1), m_y + m_height*d2);
						GL.TexCoord2(texcoords[0].U, texcoords[0].V); GL.Vertex2(m_x + m_width, m_y + m_height);
						GL.TexCoord2(texcoords[1].U, texcoords[1].V); GL.Vertex2(m_x, m_y + m_height);
						break;
					case EDirections.DOWN:
						GL.TexCoord2(texcoords[0].U, texcoords[0].V); GL.Vertex2(m_x + m_width * d1, m_y + m_height*d2);
						GL.TexCoord2(texcoords[1].U, texcoords[1].V);  GL.Vertex2(m_x + m_width * (1 - d1), m_y + m_height*d2);
						GL.TexCoord2(texcoords[2].U, texcoords[2].V); GL.Vertex2(m_x + m_width, m_y + m_height);
						GL.TexCoord2(texcoords[3].U, texcoords[3].V);  GL.Vertex2(m_x, m_y + m_height);
						break;
					case EDirections.RIGHT:
						GL.TexCoord2(texcoords[1].U, texcoords[1].V); GL.Vertex2(m_x + m_width * d1, m_y + m_height*d2);
						GL.TexCoord2(texcoords[2].U, texcoords[2].V);  GL.Vertex2(m_x + m_width * (1 - d1), m_y + m_height*d2);
						GL.TexCoord2(texcoords[3].U, texcoords[3].V);GL.Vertex2(m_x + m_width, m_y + m_height);
						GL.TexCoord2(texcoords[0].U, texcoords[0].V);  GL.Vertex2(m_x, m_y + m_height);
						break;
					case EDirections.LEFT:
						GL.TexCoord2(texcoords[3].U, texcoords[3].V); GL.Vertex2(m_x + m_width * d1, m_y + m_height*d2);
						GL.TexCoord2(texcoords[0].U, texcoords[0].V);  GL.Vertex2(m_x + m_width * (1 - d1), m_y + m_height*d2);
						GL.TexCoord2(texcoords[1].U, texcoords[1].V);GL.Vertex2(m_x + m_width, m_y + m_height);
						GL.TexCoord2(texcoords[2].U, texcoords[2].V);  GL.Vertex2(m_x, m_y + m_height);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				switch (layer.Direction)
				{
					case EDirections.UP:
						GL.TexCoord2(texcoords[2].U, texcoords[2].V); GL.Vertex2(m_x, m_y);
						GL.TexCoord2(texcoords[3].U, texcoords[3].V); GL.Vertex2(m_x + m_width, m_y);
						GL.TexCoord2(texcoords[0].U, texcoords[0].V); GL.Vertex2(m_x + m_width, m_y + m_height);
						GL.TexCoord2(texcoords[1].U, texcoords[1].V); GL.Vertex2(m_x, m_y + m_height);
						break;
					case EDirections.DOWN:
						GL.TexCoord2(texcoords[0].U, texcoords[0].V); GL.Vertex2(m_x, m_y);
						GL.TexCoord2(texcoords[1].U, texcoords[1].V); GL.Vertex2(m_x + m_width, m_y);
						GL.TexCoord2(texcoords[2].U, texcoords[2].V); GL.Vertex2(m_x + m_width, m_y + m_height);
						GL.TexCoord2(texcoords[3].U, texcoords[3].V); GL.Vertex2(m_x, m_y + m_height);
						break;
					case EDirections.RIGHT:
						GL.TexCoord2(texcoords[1].U, texcoords[1].V); GL.Vertex2(m_x, m_y);
						GL.TexCoord2(texcoords[2].U, texcoords[2].V); GL.Vertex2(m_x + m_width, m_y);
						GL.TexCoord2(texcoords[3].U, texcoords[3].V); GL.Vertex2(m_x + m_width, m_y + m_height);
						GL.TexCoord2(texcoords[0].U, texcoords[0].V); GL.Vertex2(m_x, m_y + m_height);
						break;
					case EDirections.LEFT:
						GL.TexCoord2(texcoords[3].U, texcoords[3].V); GL.Vertex2(m_x, m_y);
						GL.TexCoord2(texcoords[0].U, texcoords[0].V); GL.Vertex2(m_x + m_width, m_y);
						GL.TexCoord2(texcoords[1].U, texcoords[1].V); GL.Vertex2(m_x + m_width, m_y + m_height);
						GL.TexCoord2(texcoords[2].U, texcoords[2].V); GL.Vertex2(m_x, m_y + m_height);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public void Clear()
		{
			m_layers.Clear();
		}

		public void AddLayer(OpenTKTile _tile, FColor _color, EDirections _direction, bool _isCorpse)
		{
			m_layers.Add(new LayerInfo(){Tile = _tile, Color = _color, Direction = _direction, IsCorpse = _isCorpse});
		}
	}
}