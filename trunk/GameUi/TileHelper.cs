using System;
using System.Collections.Generic;
using System.Drawing;
using GameCore;
using Point = GameCore.Misc.Point;

namespace GameUi
{
	public static class TileHelper
	{
		private static readonly Dictionary<ETiles, ATile> m_tiles = new Dictionary<ETiles, ATile>();

		private static readonly Dictionary<EFrameTiles, ATile> m_frameTiles = new Dictionary<EFrameTiles, ATile>();

		public static ATile FogTile
		{
			get { return m_tiles[ETiles.FOG]; }
		}

		public static ATile SolidTile
		{
			get { return m_frameTiles[EFrameTiles.SOLID]; }
		}

		public static IResourceProvider Rp { get; private set; }
		public static IDrawHelper DrawHelper { get; private set; }

		public static void Init(IResourceProvider _resourceProvider, IDrawHelper _drawHelper)
		{
			Rp = _resourceProvider;
			DrawHelper = _drawHelper;

			Rp.RegisterFont(EFonts.COMMON, "Resources\\cour.ttf", 10);
			Rp.RegisterFont(EFonts.SMALL, "Resources\\cour.ttf", 5);

			foreach (ETextureSet set in Enum.GetValues(typeof (ETextureSet)))
			{
				switch (set)
				{
					case ETextureSet.REDJACK:
						Rp.RegisterTexture(set, "Resources\\redjack15v.bmp");
						break;
					case ETextureSet.RR_BRICK_01:
						Rp.RegisterTexture(set, "Resources\\RantingRodent_Brick_01.bmp");
						break;
					case ETextureSet.RR_BRICK_02:
						Rp.RegisterTexture(set, "Resources\\RantingRodent_Brick_02.bmp");
						break;
					case ETextureSet.RR_NATURAL_01:
						Rp.RegisterTexture(set, "Resources\\RantingRodent_Natural_01.bmp");
						break;
					case ETextureSet.RR_NATURAL_02:
						Rp.RegisterTexture(set, "Resources\\RantingRodent_Natural_02.bmp");
						break;
					case ETextureSet.GP_X16:
						Rp.RegisterTexture(set, "Resources\\gold_plated_16x16.bmp");
						break;
					case ETextureSet.NH:
						Rp.RegisterTexture(set, "Resources\\nethack.bmp");
						break;
					case ETextureSet.SELF:
						Rp.RegisterTexture(set, "Resources\\aq.png");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			foreach (ETiles tile in Enum.GetValues(typeof (ETiles)))
			{
				ATile tl;
				switch (tile)
				{
					case ETiles.GRASS:
						tl = new TileSet(new[]
						                 	{
						                 		Rp.CreateTile(3, 2, Color.FromArgb(30, 50, 30)),
						                 		Rp.CreateTile(5, 2, Color.FromArgb(30, 60, 30)),
						                 		Rp.CreateTile(7, 2, Color.FromArgb(20, 80, 20)),
						                 		Rp.CreateTile(12, 2, Color.FromArgb(20, 100, 20)),
						                 		Rp.CreateTile(14, 2, Color.FromArgb(20, 120, 20)),
						                 		Rp.CreateTile(ETextureSet.RR_BRICK_01, 5, 2, Color.FromArgb(30, 60, 30)),
						                 		Rp.CreateTile(ETextureSet.RR_BRICK_01, 7, 2, Color.FromArgb(20, 80, 20)),
						                 		Rp.CreateTile(ETextureSet.RR_BRICK_01, 12, 2, Color.FromArgb(20, 100, 20)),
						                 		Rp.CreateTile(ETextureSet.RR_BRICK_01, 14, 2, Color.FromArgb(20, 120, 20)),
						                 		Rp.CreateTile(ETextureSet.GP_X16, 7, 2, Color.FromArgb(20, 80, 20)),
						                 		Rp.CreateTile(ETextureSet.GP_X16, 12, 2, Color.FromArgb(20, 100, 20)),
						                 		Rp.CreateTile(ETextureSet.GP_X16, 14, 2, Color.FromArgb(20, 120, 20)),

												Rp.CreateTile(3, 2, Color.FromArgb(30, 30, 50)),
						                 		Rp.CreateTile(5, 2, Color.FromArgb(30, 30, 60)),
						                 		Rp.CreateTile(7, 2, Color.FromArgb(20, 20, 80)),
						                 		Rp.CreateTile(12, 2, Color.FromArgb(20, 20, 70)),
						                 		Rp.CreateTile(14, 2, Color.FromArgb(20, 100, 120)),
						                 		Rp.CreateTile(ETextureSet.RR_BRICK_01, 5, 2, Color.FromArgb(30, 30, 60)),
						                 		Rp.CreateTile(ETextureSet.RR_BRICK_01, 7, 2, Color.FromArgb(20, 20, 80)),
						                 		Rp.CreateTile(ETextureSet.RR_BRICK_01, 12, 2, Color.FromArgb(20, 40, 80)),
						                 		Rp.CreateTile(ETextureSet.RR_BRICK_01, 14, 2, Color.FromArgb(20, 90, 30)),
						                 		Rp.CreateTile(ETextureSet.GP_X16, 7, 2, Color.FromArgb(20, 80,50)),
						                 		Rp.CreateTile(ETextureSet.GP_X16, 12, 2, Color.FromArgb(20, 100, 60)),
						                 		Rp.CreateTile(ETextureSet.GP_X16, 14, 2, Color.FromArgb(20, 120, 70)),

												Rp.CreateTile(2, 2, Color.FromArgb(30, 50, 30)),
												Rp.CreateTile(ETextureSet.RR_BRICK_01, 2, 2, Color.FromArgb(30, 60, 30)),
												Rp.CreateTile(ETextureSet.GP_X16, 2, 2, Color.FromArgb(20, 80, 20)),
						                 	});
						break;
					case ETiles.BRICK:
						tl = Rp.CreateTile(0, 12, Color.DarkRed);
						break;
					case ETiles.BRICK_WINDOW:
						tl = Rp.CreateTile(1, 12, Color.DarkRed);
						break;
					case ETiles.ON_WALL_LIGHT_SOURCE:
						tl = Rp.CreateTile(ETextureSet.SELF, 0, 0, Color.White);
						break;
					case ETiles.DOOR:
						tl = Rp.CreateTile(5, 12, Color.Brown);
						break;
					case ETiles.OPEN_DOOR:
						tl = Rp.CreateTile(4, 12, Color.Brown);
						break;
					case ETiles.AVATAR:
						tl = Rp.CreateTile(2, 0, Color.White);
						break;
					case ETiles.MASHROOM:
						tl = new TileSet(new[]
						                 	{
						                 		Rp.CreateTile(5, 0, Color.FromArgb(20, 160, 20)),
						                 		Rp.CreateTile(6, 0, Color.FromArgb(20, 80, 20)),
						                 		Rp.CreateTile(7, 1, Color.FromArgb(20, 90, 20)),
						                 		Rp.CreateTile(8, 1, Color.FromArgb(20, 120, 90)),
						                 		Rp.CreateTile(12, 1, Color.Gray),
						                 	});
						break;
					case ETiles.GROUND:
						tl = Rp.CreateTile(0, 0, Color.FromArgb(10, 20, 10));
						break;
					case ETiles.SWORD:
						tl = Rp.CreateTile(ETextureSet.NH, 20, 10, Color.White);
						break;
					case ETiles.AXE:
						tl = Rp.CreateTile(15, 2, Color.LightSteelBlue);
						break;
					case ETiles.CROSSBOW:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 14, 14, Color.SkyBlue);
						break;
					case ETiles.CHEST:
						tl = Rp.CreateTile(ETextureSet.RR_BRICK_01, 2, 9, Color.Gold);
						break;
					case ETiles.MONSTER:
						//tl = Rp.CreateTile(ETextureSet.NH, 0, 6, Color.White);
						tl = Rp.CreateTile(ETextureSet.NH, 0, 8, Color.White);
						break;
					case ETiles.RING:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 8, 15, Color.Gold);
						break;
					case ETiles.HEAP_OF_ITEMS:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 11, 0, Color.DarkOrchid);
						break;
					case ETiles.POTION:
						tl = Rp.CreateTile(ETextureSet.RR_BRICK_01, 13, 10, Color.Gray);
						break;
					case ETiles.CROSSBOW_BOLT:
						tl = Rp.CreateTile(ETextureSet.NH, 0, 10, Color.White);
						break;
					case ETiles.TARGET_DOT:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 10, 15, Color.Gold);
						break;
					case ETiles.TARGET_CROSS:
						tl = Rp.CreateTile(ETextureSet.RR_BRICK_01, 8, 5, Color.Gold);
						break;
					case ETiles.FOG:
						tl = Rp.CreateTile(1, 11, Color.FromArgb(255, 5, 5, 10));
						tl.IsFogTile = true;
						break;
					case ETiles.STAIR_DOWN:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 14, 3, Color.FromArgb(255, 100, 100, 50));
						break;
					case ETiles.STAIR_UP:
						tl = Rp.CreateTile(ETextureSet.REDJACK, 12, 3, Color.FromArgb(255, 100, 100, 50));
						break;
					case ETiles.STONE_FLOOR:
						//tl = Rp.CreateTile(ETextureSet.RR_BRICK_01, 4, 8, Color.FromArgb(255, 100, 100, 200));
						tl = Rp.CreateTile(ETextureSet.RR_BRICK_01, 4, 8, Color.FromArgb(255, 30, 30, 50));
						break;
					case ETiles.STONE_WALL:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 3, 10, Color.FromArgb(255, 100, 100, 200));
						break;
					case ETiles.NONE:
						tl = null;
						break;
					case ETiles.WATER:
						tl = Rp.CreateTile(ETextureSet.RR_BRICK_01, 14, 8, Color.Blue);
						break;
					case ETiles.LAVA:
						tl = Rp.CreateTile(ETextureSet.RR_BRICK_01, 14, 8, Color.Red);
						break;
					case ETiles.SWAMP:
						tl = Rp.CreateTile(ETextureSet.RR_BRICK_01, 14, 8, Color.DarkKhaki);
						break;
					case ETiles.UP:
						tl = Rp.CreateTile(14, 1, Color.Gray);
						break;
					case ETiles.DOWN:
						tl = Rp.CreateTile(15, 1, Color.Gray);
						break;
					case ETiles.LEFT:
						tl = Rp.CreateTile(1, 1, Color.Gray);
						break;
					case ETiles.RIGHT:
						tl = Rp.CreateTile(0, 1, Color.Gray);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				if(tl!=null)
				{
					tl.Tile = tile;
				}
				m_tiles.Add(tile, tl);
			}

			foreach (EFrameTiles tile in Enum.GetValues(typeof (EFrameTiles)))
			{
				ATile tl;
				switch (tile)
				{
					case EFrameTiles.SIMPLE:
						tl = Rp.CreateTile(0, 12, Color.Green);
						break;
					case EFrameTiles.FRAME_L:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 3, 11, Color.Gold);
						break;
					case EFrameTiles.FRAME_R:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 3, 11, Color.Gold);
						break;
					case EFrameTiles.FRAME_T:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 4, 12, Color.Gold);
						break;
					case EFrameTiles.FRAME_B:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 4, 12, Color.Gold);
						break;
					case EFrameTiles.FRAME_TL:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 10, 13, Color.Gold);
						break;
					case EFrameTiles.FRAME_TR:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 15, 11, Color.Gold);
						break;
					case EFrameTiles.FRAME_BL:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 0, 12, Color.Gold);
						break;
					case EFrameTiles.FRAME_BR:
						tl = Rp.CreateTile(ETextureSet.GP_X16, 9, 13, Color.Gold);
						break;
					case EFrameTiles.SOLID:
						tl = Rp.CreateTile(11, 13, Color.White);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				if (tl != null)
				{
					tl.Tile = tile;
				}
				m_frameTiles.Add(tile, tl);
			}
		}

		public static ATile GetTile(this ETiles _tile)
		{
			return m_tiles[_tile];
		}

		public static ATile GetTile(this EFrameTiles _tile)
		{
			return m_frameTiles[_tile];
		}

		public static ATile GetTile(this ETiles _tile, int _index)
		{
			var ts = (TileSet) m_tiles[_tile];
			return ts[_index];
		}

		public static void DrawAtCell(this EFrameTiles _tile, int _col, int _row, FColor _backgroundColor)
		{
			var tile = m_frameTiles[_tile];
			tile.Draw(new Point(_col, _row), tile.Color, _backgroundColor);
		}

		public static ATile Tile(this ETerrains _terrain, Point _worldCoords, float _rnd)
		{
			switch (_terrain)
			{
				case ETerrains.STONE_FLOOR:
					return m_tiles[ETiles.STONE_FLOOR];
				case ETerrains.STONE_WALL:
					return m_tiles[ETiles.STONE_WALL];
				case ETerrains.GROUND:
					return m_tiles[ETiles.GROUND];
				case ETerrains.GRASS:
					return ((TileSet)m_tiles[ETiles.GRASS])[(int)Math.Abs((_worldCoords.GetHashCode() * _rnd))];
				case ETerrains.MUSHROOM:
					return ((TileSet)m_tiles[ETiles.MASHROOM])[(int)Math.Abs((_worldCoords.GetHashCode() * _rnd))];
				case ETerrains.BRICK_WALL:
					return m_tiles[ETiles.BRICK];
				case ETerrains.WINDOW:
					return m_tiles[ETiles.BRICK_WINDOW];
				case ETerrains.WATER:
					return m_tiles[ETiles.WATER];
				case ETerrains.SWAMP:
					return m_tiles[ETiles.SWAMP];
				case ETerrains.LAVA:
					return m_tiles[ETiles.LAVA];
				case ETerrains.UP:
					return m_tiles[ETiles.UP];
				case ETerrains.DOWN:
					return m_tiles[ETiles.DOWN];
				case ETerrains.LEFT:
					return m_tiles[ETiles.LEFT];
				case ETerrains.RIGHT:
					return m_tiles[ETiles.RIGHT];
				default:
					throw new ArgumentOutOfRangeException("_terrain");
			}
		}
	}
}