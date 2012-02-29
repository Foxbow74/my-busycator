using System;
using System.Drawing;
using GameCore;
using GameCore.Misc;
using GameUi;
using OpenTK.Graphics.OpenGL;

namespace OpenTKUi
{
	class TileMapRenderer:IDisposable
	{
		private static int m_tileSizeX;
		private static int m_tileSizeY;
		private readonly int m_screenWidth;
		private readonly int m_screenHeight;
		private readonly TileInfo[,] m_tiles;
		static private int m_tilesInRow;
		static private int m_tilesInColumn;
		private static Image m_img;

		public int Iteration { get; set; }

		public static OpenTKResourceProvider ResourceProvider { get; private set; }

		public static void Init(int _tileSizeX, int _tileSizeY, OpenTKResourceProvider _resourceProvider)
		{
			m_tileSizeX = _tileSizeX;
			m_tileSizeY = _tileSizeY;
			ResourceProvider = _resourceProvider;

			var size = (int)Math.Sqrt(_resourceProvider.Tiles.Count) + 1;
			var sizeInPixels = size * _tileSizeX;

			var begin = 16;
			for (var i = 1; ;++i )
			{
				if(sizeInPixels<=begin)
				{
					sizeInPixels = begin;
					break;
				}
				begin *= 2;
			}

			var bmp = new Bitmap(sizeInPixels, sizeInPixels);
			using(var gr = Graphics.FromImage(bmp))
			{
				var perRow = sizeInPixels / 16;
				for (var index = 0; index < _resourceProvider.Tiles.Count; index++)
				{
					var tile = _resourceProvider.Tiles[index];
					var x = index % perRow;
					var y = index / perRow;
					var dstRect = new Rct(x * _tileSizeX, y * _tileSizeY, _tileSizeX, _tileSizeY);
					var srcRect = new Rct(tile.X * _tileSizeX, tile.Y * _tileSizeY, _tileSizeX, _tileSizeY);

					tile.UpdateTexCoords(x, y, sizeInPixels, sizeInPixels);

					if(tile.IsFogTile)
					{
						TileInfo.FogTexCoords = tile.Texcoords;
					}

					gr.DrawImage(_resourceProvider[tile.Set].Bitmap, new Rectangle(dstRect.Left, dstRect.Top, dstRect.Width+1, dstRect.Height+1), new Rectangle(srcRect.Left, srcRect.Top, srcRect.Width+1, srcRect.Height+1), GraphicsUnit.Pixel);
				}
			}
			m_img = new Image(bmp, true);
		}

		public TileMapRenderer(int _screenWidth, int _screenHeight)
		{
			m_screenWidth = _screenWidth;
			m_screenHeight = _screenHeight;
			
			m_tilesInRow = m_screenWidth/m_tileSizeX;
			m_tilesInColumn = _screenHeight/m_tileSizeY;
			m_tiles = new TileInfo[m_tilesInRow, m_tilesInColumn];
			for (var i = 0; i < m_tilesInRow; i++)
			{
				for (var j = 0; j < m_tilesInColumn; j++)
				{
					m_tiles[i,j] = new TileInfo(i, j, m_tileSizeX, m_tileSizeY);
				}
			}
		}

		public void Draw()
		{
			GL.BindTexture(TextureTarget.Texture2D, m_img.Texture);
			//GL.BlendEquation(BlendEquationMode.FuncReverseSubtract);
			//DrawQuads(false, false);
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			DrawQuads(true, false);
			//GL.BlendEquation(BlendEquationMode.FuncReverseSubtract);
			//DrawQuads(false, true);
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			DrawQuads(true, true);
		}

		private void DrawQuads(bool _colored, bool _drawFog)
		{
			GL.Begin(BeginMode.Quads);
			for (var i = 0; i < m_tilesInRow; i++)
			{
				for (var j = 0; j < m_tilesInColumn; j++)
				{
					m_tiles[i, j].Draw(Iteration, _colored, _drawFog);
				}
			}
			GL.End();
		}

		public void Dispose()
		{
		}

		public void DrawTile(OpenTKTile _tile, int _x, int _y, FColor _color, FColor _background)
		{
			var info = m_tiles[_x, _y];
			info.IsFogged = false;
			info.Tile = _tile;
			info.Forecolor = _color;
		}

		public void  FogTile(int _col, int _row)
		{
			m_tiles[_col, _row].IsFogged = true;
		}

		public void Clear(Rct _rct, FColor _backgroundColor)
		{
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.Begin(BeginMode.Quads);
			var xy = new GameCore.Misc.Point(_rct.Left, _rct.Top) * ATile.Size;
			var xy1 = new GameCore.Misc.Point(_rct.Right, _rct.Bottom) * ATile.Size;
			GL.Color4(_backgroundColor.R, _backgroundColor.G, _backgroundColor.B, _backgroundColor.A);
			GL.Vertex2(xy.X, xy.Y);
			GL.Vertex2(xy1.X, xy.Y);
			GL.Vertex2(xy1.X, xy1.Y);
			GL.Vertex2(xy.X, xy1.Y);
			GL.End();

			for (var i = _rct.Left; i < _rct.Right; i++)
			{
				for (var j = _rct.Top; j < _rct.Bottom; j++)
				{
					m_tiles[i, j].Clear();
				}
			}
		}
	}

	class TileInfo
	{
		private readonly int m_x;
		private readonly int m_y;
		private readonly int m_width;
		private readonly int m_height;

		public static OpenTKTile.TexCoord[] FogTexCoords;

		public bool IsVisible { get; set; }
		public OpenTKTile Tile { get; set; }
		public FColor Forecolor { get; set; }

		public bool IsFogged { get; set; }

		public TileInfo(int _x, int _y, int _width, int _height)
		{
			m_x = _x * _width;
			m_y = _y * _height;
			m_width = _width;
			m_height = _height;
		}

		public void Draw(int _iteration, bool _colored, bool _fogOnly)
		{
			if (Tile == null && !_fogOnly) return;
			if (_fogOnly && !IsFogged) return;


			OpenTKTile.TexCoord[] texcoords;
			var color = Forecolor;
			if (_fogOnly)
			{
				if (IsFogged)
				{
					texcoords = FogTexCoords;
					color = FColor.Black;
					//return;
				}
				else
				{
					return;
				}
			}
			else
			{
				texcoords = Tile.Texcoords;
			}
			if(_colored)
			{
				GL.Color4(color.R, color.G, color.B, color.A);
			}
			else
			{
				GL.Color4(1f, 1f, 1f, color.A);
			}

			GL.TexCoord2(texcoords[0].U, texcoords[0].V);
			GL.Vertex2(m_x, m_y);
			GL.TexCoord2(texcoords[1].U, texcoords[1].V);
			GL.Vertex2(m_x + m_width, m_y);
			GL.TexCoord2(texcoords[2].U, texcoords[2].V);
			GL.Vertex2(m_x + m_width, m_y + m_height);
			GL.TexCoord2(texcoords[3].U, texcoords[3].V);
			GL.Vertex2(m_x, m_y + m_height);

			//GL.TexCoord2(texcoords[3].U, texcoords[3].V);
			//GL.Vertex2(m_x, m_y);
			//GL.TexCoord2(texcoords[0].U, texcoords[0].V);
			//GL.Vertex2(m_x + m_width, m_y);
			//GL.TexCoord2(texcoords[1].U, texcoords[1].V);
			//GL.Vertex2(m_x + m_width, m_y + m_height);
			//GL.TexCoord2(texcoords[2].U, texcoords[2].V);
			//GL.Vertex2(m_x, m_y + m_height);

		}

		public void SendVertices()
		{
			
		}

		public void Clear()
		{
			Tile = null;
		}
	}
}
