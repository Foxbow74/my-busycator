#region

using System;
using System.Collections.Generic;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace RGL1
{
	public static class TileHelper
	{
		public static Tile FogTile = new Tile(1, 11, new Color(5, 5, 10));

		private static readonly Dictionary<TextureSet, Texture2D> m_textures = new Dictionary<TextureSet, Texture2D>();

		private static readonly Dictionary<ETiles, Tile> m_tiles = new Dictionary<ETiles, Tile>();

		private static readonly Dictionary<EFrameTiles, Tile> m_frameTiles = new Dictionary<EFrameTiles, Tile>();

		public static Tile SolidTile
		{
			get { return m_frameTiles[EFrameTiles.SOLID]; }
		}

		public static void Init(ContentManager _content)
		{
			foreach (TextureSet set in Enum.GetValues(typeof (TextureSet)))
			{
				Texture2D texture;
				switch (set)
				{
					case TextureSet.REDJACK:
						texture = _content.Load<Texture2D>("redjack15v");
						break;
					case TextureSet.RR_BRICK_01:
						texture = _content.Load<Texture2D>("RantingRodent_Brick_01");
						break;
					case TextureSet.RR_BRICK_02:
						texture = _content.Load<Texture2D>("RantingRodent_Brick_02");
						break;
					case TextureSet.RR_NATURAL_01:
						texture = _content.Load<Texture2D>("RantingRodent_Natural_01");
						break;
					case TextureSet.RR_NATURAL_02:
						texture = _content.Load<Texture2D>("RantingRodent_Natural_02");
						break;
					case TextureSet.GP_X16:
						texture = _content.Load<Texture2D>("gold_plated_16x16");
						break;
					case TextureSet.NH:
						texture = _content.Load<Texture2D>("nethack");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				m_textures.Add(set, texture);
			}

			foreach (ETiles tile in Enum.GetValues(typeof (ETiles)))
			{
				Tile tl;
				switch (tile)
				{
					case ETiles.GRASS:
						tl = new TileSet(new[]
						                 	{
						                 		new Tile(3, 2, new Color(30, 50, 30)),
						                 		new Tile(5, 2, new Color(30, 60, 30)),
						                 		new Tile(7, 2, new Color(20, 80, 20)),
						                 		new Tile(12, 2, new Color(20, 100, 20)),
						                 		new Tile(14, 2, new Color(20, 120, 20)),
						                 		new Tile(TextureSet.RR_BRICK_01, 5, 2, new Color(30, 60, 30)),
						                 		new Tile(TextureSet.RR_BRICK_01, 7, 2, new Color(20, 80, 20)),
						                 		new Tile(TextureSet.RR_BRICK_01, 12, 2, new Color(20, 100, 20)),
						                 		new Tile(TextureSet.RR_BRICK_01, 14, 2, new Color(20, 120, 20)),
						                 		new Tile(TextureSet.GP_X16, 7, 2, new Color(20, 80, 20)),
						                 		new Tile(TextureSet.GP_X16, 12, 2, new Color(20, 100, 20)),
						                 		new Tile(TextureSet.GP_X16, 14, 2, new Color(20, 120, 20)),
						                 		new Tile(7, 0, new Color(55, 58, 50)),
						                 		new Tile(TextureSet.GP_X16, 7, 0, new Color(65, 68, 60)),
						                 	});
						break;
					case ETiles.BRICK:
						tl = new Tile(0, 12, Color.DarkRed);
						break;
					case ETiles.BRICK_WINDOW:
						tl = new Tile(1, 12, Color.DarkRed);
						break;
					case ETiles.DOOR:
						tl = new Tile(5, 12, Color.Brown);
						break;
					case ETiles.OPEN_DOOR:
						tl = new Tile(4, 12, Color.Brown);
						break;
					case ETiles.AVATAR:
						tl = new Tile(2, 0, Color.White);
						break;
					case ETiles.MASHROOM:
						tl = new TileSet(new[]
						                 	{
						                 		new Tile(5, 0, new Color(20, 160, 20)),
						                 		new Tile(6, 0, new Color(20, 80, 20)),
						                 		new Tile(7, 1, new Color(20, 90, 20)),
						                 		new Tile(8, 1, new Color(20, 120, 90)),
						                 		new Tile(12, 1, Color.Gray),
						                 	});
						break;
					case ETiles.GROUND:
						tl = new Tile(0, 0, new Color(10, 20, 10));
						break;
					case ETiles.SWORD:
						tl = new Tile(TextureSet.NH, 20, 10, Color.White);
						break;
					case ETiles.AXE:
						tl = new Tile(15, 2, Color.LightSteelBlue);
						break;
					case ETiles.CROSSBOW:
						tl = new Tile(TextureSet.GP_X16, 14, 14, Color.SkyBlue);
						break;
					case ETiles.CHEST:
						tl = new Tile(TextureSet.RR_BRICK_01, 2, 9, Color.Gold);
						break;
					case ETiles.MONSTER:
						tl = new Tile(TextureSet.NH, 0, 6, Color.White);
						break;
					case ETiles.RING:
						tl = new Tile(TextureSet.NH, 28, 13, Color.White);
						break;
					case ETiles.HEAP_OF_ITEMS:
						tl = new Tile(TextureSet.GP_X16, 11, 0, Color.DarkOrchid);
						break;
					case ETiles.POTION:
						tl = new Tile(TextureSet.RR_BRICK_01, 13, 10, Color.Gray);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				m_tiles.Add(tile, tl);
			}

			foreach (EFrameTiles tile in Enum.GetValues(typeof (EFrameTiles)))
			{
				Tile tl;
				switch (tile)
				{
					case EFrameTiles.SIMPLE:
						tl = new Tile(0, 12, Color.Green);
						break;
					case EFrameTiles.FRAME_L:
						tl = new Tile(TextureSet.GP_X16, 3, 11, Color.Gold);
						break;
					case EFrameTiles.FRAME_R:
						tl = new Tile(TextureSet.GP_X16, 3, 11, Color.Gold);
						break;
					case EFrameTiles.FRAME_T:
						tl = new Tile(TextureSet.GP_X16, 4, 12, Color.Gold);
						break;
					case EFrameTiles.FRAME_B:
						tl = new Tile(TextureSet.GP_X16, 4, 12, Color.Gold);
						break;
					case EFrameTiles.FRAME_TL:
						tl = new Tile(TextureSet.GP_X16, 10, 13, Color.Gold);
						break;
					case EFrameTiles.FRAME_TR:
						tl = new Tile(TextureSet.GP_X16, 15, 11, Color.Gold);
						break;
					case EFrameTiles.FRAME_BL:
						tl = new Tile(TextureSet.GP_X16, 0, 12, Color.Gold);
						break;
					case EFrameTiles.FRAME_BR:
						tl = new Tile(TextureSet.GP_X16, 9, 13, Color.Gold);
						break;
					case EFrameTiles.SOLID:
						tl = new Tile(11, 13, Color.White);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				m_frameTiles.Add(tile, tl);
			}
		}

		public static Texture2D GetTexture(this Tile _tile)
		{
			return m_textures[_tile.Set];
		}

		public static Tile GetTile(this ETiles _tile)
		{
			return m_tiles[_tile];
		}

		public static Tile GetTile(this EFrameTiles _tile)
		{
			return m_frameTiles[_tile];
		}

		public static Tile GetTile(this ETiles _tile, int _index)
		{
			var ts = (TileSet) m_tiles[_tile];
			return ts[_index];
		}

		private static void DrawAtPoint(this Tile _tile, SpriteBatch _spriteBatch, int _x, int _y, Color _color)
		{
			if (_tile == null) return;

			var destination = new Rectangle(_x, _y, Tile.Size, Tile.Size);
			_spriteBatch.Draw(_tile.GetTexture(), destination, _tile.Rectangle, _color);
		}

		public static void DrawAtCell(this Tile _tile, SpriteBatch _spriteBatch, int _col, int _row)
		{
			if (_tile == null) return;
			_tile.DrawAtPoint(_spriteBatch, _col*Tile.Size, _row*Tile.Size, _tile.Color);
		}

		public static void DrawAtCell(this Tile _tile, SpriteBatch _spriteBatch, int _col, int _row, Color _color)
		{
			if (_tile == null) return;
			_tile.DrawAtPoint(_spriteBatch, _col*Tile.Size, _row*Tile.Size, _color);
		}

		public static void DrawAtPoint(this ETiles _tile, SpriteBatch _spriteBatch, int _x, int _y, Color _color)
		{
			DrawAtPoint(m_tiles[_tile], _spriteBatch, _x, _y, _color);
		}

		public static void DrawAtCell(this ETiles _tile, SpriteBatch _spriteBatch, int _col, int _row)
		{
			DrawAtCell(m_tiles[_tile], _spriteBatch, _col, _row);
		}

		public static void DrawAtCell(this ETiles _tile, SpriteBatch _spriteBatch, int _col, int _row, Color _color)
		{
			DrawAtCell(m_tiles[_tile], _spriteBatch, _col, _row, _color);
		}

		public static void DrawAtPoint(this EFrameTiles _tile, SpriteBatch _spriteBatch, int _x, int _y, Color _color)
		{
			DrawAtPoint(m_frameTiles[_tile], _spriteBatch, _x, _y, _color);
		}

		public static void DrawAtCell(this EFrameTiles _tile, SpriteBatch _spriteBatch, int _col, int _row)
		{
			DrawAtCell(m_frameTiles[_tile], _spriteBatch, _col, _row);
		}

		public static void DrawAtCell(this EFrameTiles _tile, SpriteBatch _spriteBatch, int _col, int _row, Color _color)
		{
			DrawAtCell(m_frameTiles[_tile], _spriteBatch, _col, _row, _color);
		}
	}
}