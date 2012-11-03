﻿using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using GameCore;
using GameCore.Misc;
using GameUi;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = GameCore.Misc.Point;

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

			if (File.Exists(Constants.RESOURCES_PNG_FILE))
			{
				var rsrs = (Bitmap)Bitmap.FromFile(Constants.RESOURCES_PNG_FILE);
				m_img = new Image(rsrs, false);
				foreach (var tile in _resourceProvider.Tiles)
				{
					if (tile.Tile==ETiles.FOG)
					{
						tile.IsFogTile = true;
						TileInfo.FogTexCoords = tile.Texcoords;
					}
					tile.UpdateTexCoords(tile.X, tile.Y, rsrs.Width, rsrs.Height);
				}
			}
			else
			{
				var size = (int)Math.Sqrt(_resourceProvider.Tiles.Count) + 1;
				var sizeInPixels = size * _tileSizeX;

				var begin = 16;
				for (var i = 1; ; ++i)
				{
					if (sizeInPixels <= begin)
					{
						sizeInPixels = begin;
						break;
					}
					begin *= 2;
				}

				var bmp = new Bitmap(sizeInPixels, sizeInPixels, PixelFormat.Format32bppPArgb);
				using (var gr = Graphics.FromImage(bmp))
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

						if (tile.IsFogTile)
						{
							TileInfo.FogTexCoords = tile.Texcoords;
						}

						gr.DrawImage(_resourceProvider[tile.Set].Bitmap, new Rectangle(dstRect.Left, dstRect.Top, dstRect.Width, dstRect.Height), new Rectangle(srcRect.Left, srcRect.Top, srcRect.Width, srcRect.Height), GraphicsUnit.Pixel);
					}
				}
				bmp.Save(Constants.RESOURCES_PNG_FILE, ImageFormat.Png);

				var sb = new StringBuilder();

				foreach (var pair in TileHelper.AllTiles)
				{
					sb.AppendLine("t|" + pair.Key + "|" + pair.Value.ToResurceText());
				}

				foreach (var tileset in TileHelper.AllTerrainTilesets)
				{
					foreach (var tile in tileset.Value.Tiles)
					{
						sb.AppendLine("r|" + tileset.Key + "|" + tile.ToResurceText());
					}
				}
				File.WriteAllText(Constants.RESOURCES_TXT_FILENAME, sb.ToString());
				m_img = new Image(bmp, true);
			}
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

			var needDraw = true;
			var layer = 0;
			while (needDraw)
			{
				needDraw = DrawQuads(true, false, layer);
				if (needDraw)
				{
					DrawQuads(true, true, layer);
				}
				layer++;
			}
		}

		private bool DrawQuads(bool _colored, bool _drawFog, int _layer)
		{
			bool flag = false;
			GL.Begin(BeginMode.Quads);
			for (var i = 0; i < m_tilesInRow; i++)
			{
				for (var j = 0; j < m_tilesInColumn; j++)
				{
					var tileInfo = m_tiles[i, j];
					if (tileInfo.Layers > _layer)
					{
						flag = true;
						tileInfo.Draw(Iteration, _colored, _drawFog, _layer);
					}
				}
			}
			GL.End();
			return flag;
		}

		public void Dispose()
		{
		}

		public void DrawTile(OpenTKTile _tile, int _x, int _y, FColor _color, EDirections _direction)
		{
			if(_x<0 || _y<0 || _x>=m_tilesInRow || _y>=m_tilesInColumn) return;
			var info = m_tiles[_x, _y];
			info.IsFogged = false;
			info.AddLayer(_tile, _color, _direction);
		}

		public void FogTile(Point _point)
		{
			m_tiles[_point.X, _point.Y].IsFogged = true;
		}

		public void Clear(Rct _rct, FColor _backgroundColor)
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.Begin(BeginMode.Quads);
			var xy = new Point(_rct.Left, _rct.Top) * Constants.TILE_SIZE;
			var xy1 = new Point(_rct.Right + 1, _rct.Bottom + 1) * Constants.TILE_SIZE;
			GL.Color4(_backgroundColor.R, _backgroundColor.G, _backgroundColor.B, _backgroundColor.A);
			GL.Vertex2(xy.X, xy.Y);
			GL.Vertex2(xy1.X, xy.Y);
			GL.Vertex2(xy1.X, xy1.Y);
			GL.Vertex2(xy.X, xy1.Y);
			GL.End();

			for (var i = _rct.Left; i <= _rct.Right; i++)
			{
				for (var j = _rct.Top; j <= _rct.Bottom; j++)
				{
					m_tiles[i, j].Clear();
				}
			}
		}
	}
}
