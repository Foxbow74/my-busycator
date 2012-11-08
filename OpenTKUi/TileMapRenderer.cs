﻿using System;
using System.IO;
using System.Drawing;
using GameCore;
using GameCore.Misc;
using GameUi;
using OpenTK.Graphics.OpenGL;
using Point = GameCore.Misc.Point;

namespace OpenTKUi
{
	class TileMapRenderer:IDisposable
	{
		private readonly int m_screenWidth;
		private readonly int m_screenHeight;
		private readonly TileInfo[,] m_tiles;
		static private int m_tilesInRow;
		static private int m_tilesInColumn;
		private static Image m_img;

		public int Iteration { get; set; }

		public static OpenTKResourceProvider ResourceProvider { get; private set; }

		public static void Init(OpenTKResourceProvider _resourceProvider)
		{
			ResourceProvider = _resourceProvider;

			if (File.Exists(Constants.RESOURCES_PNG_FILE))
			{
				var rsrs = new Bitmap(Constants.RESOURCES_PNG_FILE);
				m_img = new Image(rsrs, false);
				foreach (var tile in _resourceProvider.Tiles)
				{
					tile.UpdateTexCoords(tile.X, tile.Y, rsrs.Width, rsrs.Height);
				}
			}
			else
			{
                throw new ApplicationException("Не найден файл с тайлами " + Path.GetFullPath(Constants.RESOURCES_PNG_FILE));
                //var size = (int)Math.Sqrt(_resourceProvider.Tiles.Count) + 1;
                //var sizeInPixels = size * Constants.TILE_SIZE;

                //var begin = 16;
                //for (var i = 1; ; ++i)
                //{
                //    if (sizeInPixels <= begin)
                //    {
                //        sizeInPixels = begin;
                //        break;
                //    }
                //    begin *= 2;
                //}

                //var bmp = new Bitmap(sizeInPixels, sizeInPixels, PixelFormat.Format32bppPArgb);
                //using (var gr = Graphics.FromImage(bmp))
                //{
                //    var perRow = sizeInPixels / 16;
                //    for (var index = 0; index < _resourceProvider.Tiles.Count; index++)
                //    {
                //        var tile = _resourceProvider.Tiles[index];
                //        var x = index % perRow;
                //        var y = index / perRow;
                //        var dstRect = new Rct(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                //        var srcRect = new Rct(tile.X * Constants.TILE_SIZE, tile.Y * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);

                //        tile.UpdateTexCoords(x, y, sizeInPixels, sizeInPixels);

                //        gr.DrawImage(_resourceProvider[tile.Set].Bitmap, new Rectangle(dstRect.Left, dstRect.Top, dstRect.Width, dstRect.Height), new Rectangle(srcRect.Left, srcRect.Top, srcRect.Width, srcRect.Height), GraphicsUnit.Pixel);
                //    }
                //}
                //bmp.Save(Constants.RESOURCES_PNG_FILE, ImageFormat.Png);

                //foreach (var pair in TileHelper.AllTiles)
                //{
                //    var xTileSet = new XTileSet {Tile = pair.Key,};
                //    World.XResourceRoot.TileSets.Add(xTileSet);
                //    for (int index = 0; index < pair.Value.Tiles.Count; index++)
                //    {
                //        var tile = pair.Value.Tiles[index];
                //        var xcolor = tile.Color.GetXColor();
                //        World.XResourceRoot.Colors.Add(xcolor);
                //        xTileSet.Children.Add(new XTileInfo { Texture = (int)tile.Set, CX = tile.Point.X, CY = tile.Point.Y, X = tile.SrcPoint.X, Y = tile.SrcPoint.Y, Color = xcolor, Order = index });
                //    }
                //}

                //foreach (var tileset in TileHelper.AllTerrainTilesets)
                //{
                //    var xTerrainSet = new XTerrainSet { Terrains = tileset.Key };
                //    World.XResourceRoot.TerrainSets.Add(xTerrainSet);
                //    for (int index = 0; index < tileset.Value.Tiles.Count; index++)
                //    {
                //        var tile = tileset.Value.Tiles[index];
                //        var xcolor = tile.Color.GetXColor();
                //        World.XResourceRoot.Colors.Add(xcolor);
                //        xTerrainSet.Children.Add(new XTileInfo {Texture = (int) tile.Set, CX = tile.Point.X, CY = tile.Point.Y, X = tile.SrcPoint.X, Y = tile.SrcPoint.Y, Color = xcolor, Order = index });
                //    }
                //}

                //World.SaveResources();
                //m_img = new Image(bmp, true);
			}

			TileInfo.FogTexCoords = ((OpenTKTile)TileHelper.AllTiles[ETiles.FOG].Tiles[0]).Texcoords;
		}

		public TileMapRenderer(int _screenWidth, int _screenHeight)
		{
			m_screenWidth = _screenWidth;
			m_screenHeight = _screenHeight;

			m_tilesInRow = m_screenWidth / Constants.TILE_SIZE;
			m_tilesInColumn = _screenHeight / Constants.TILE_SIZE;
			m_tiles = new TileInfo[m_tilesInRow, m_tilesInColumn];
			for (var i = 0; i < m_tilesInRow; i++)
			{
				for (var j = 0; j < m_tilesInColumn; j++)
				{
					m_tiles[i, j] = new TileInfo(i, j, Constants.TILE_SIZE, Constants.TILE_SIZE);
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
