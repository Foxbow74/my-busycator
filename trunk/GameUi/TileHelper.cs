using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GameCore;
using GameCore.Misc;
using GameCore.Storeable;

namespace GameUi
{
	public static class TileHelper
	{
		static TileHelper()
		{
			AllTiles = new Dictionary<ETiles, TileSet>();
			AllTerrainTilesets = new Dictionary<ETerrains, TileSet>();
		}

		public static Dictionary<ETerrains, TileSet> AllTerrainTilesets { get; private set; }
		public static Dictionary<ETiles, TileSet> AllTiles { get; private set; }

		public static IResourceProvider Rp { get; private set; }
		public static IDrawHelper DrawHelper { get; private set; }

		public static void Init(IResourceProvider _resourceProvider, IDrawHelper _drawHelper)
		{
			Rp = _resourceProvider;
			DrawHelper = _drawHelper;

			Rp.RegisterFont(EFonts.COMMON, "res\\monof55.ttf", 12);
			Rp.RegisterFont(EFonts.SMALL, "res\\monof55.ttf", 8);

			if (_drawHelper!=null && World.XResourceRoot.TileSets.Count > 0)
			{
				foreach (var xTileSet in World.XResourceRoot.TileSets)
				{
					var set = new TileSet();
					AllTiles.Add(xTileSet.Tile, set);
					foreach (XTileInfo tileInfo in xTileSet.Children)
					{
						set.AddTile(Rp.CreateTile(ETextureSet.GP, tileInfo.CX, tileInfo.CY, tileInfo.Color));
					}
				}
				foreach (var xTileSet in World.XResourceRoot.TerrainSets)
				{
					var set = new TileSet();
					AllTerrainTilesets.Add(xTileSet.Terrains, set);
					foreach (var tileInfo in xTileSet.Children)
					{
						set.AddTile(Rp.CreateTile(ETextureSet.GP, tileInfo.CX, tileInfo.CY, tileInfo.Color));
					}
				}
			}
			else
			{
				foreach (ETextureSet set in Enum.GetValues(typeof(ETextureSet)))
				{
					switch (set)
					{
						case ETextureSet.RJ:
							Rp.RegisterTexture(set, "Resources\\redjack15v.bmp");
							break;
						case ETextureSet.RB1:
							Rp.RegisterTexture(set, "Resources\\RantingRodent_Brick_01.bmp");
							break;
						case ETextureSet.RB2:
							Rp.RegisterTexture(set, "Resources\\RantingRodent_Brick_02.bmp");
							break;
						case ETextureSet.RN1:
							Rp.RegisterTexture(set, "Resources\\RantingRodent_Natural_01.bmp");
							break;
						case ETextureSet.RN2:
							Rp.RegisterTexture(set, "Resources\\RantingRodent_Natural_02.bmp");
							break;
						case ETextureSet.GP:
							Rp.RegisterTexture(set, "Resources\\gold_plated_16x16.bmp");
							break;
						case ETextureSet.NH:
							Rp.RegisterTexture(set, "Resources\\nethack.bmp");
							break;
						case ETextureSet.HM:
							Rp.RegisterTexture(set, "Resources\\aq.png");
							break;
						case ETextureSet.PH:
							Rp.RegisterTexture(set, "Resources\\Phoebus_16x16.png");
							break;
						case ETextureSet.U4:
							Rp.RegisterTexture(set, "Resources\\Ultima4.png");
							break;
						case ETextureSet.U5:
							Rp.RegisterTexture(set, "Resources\\Ultima5.png");
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				AllTiles.Add(ETiles.NONE, new TileSet(Rp.CreateTile(ETextureSet.GP, 0, 0, FColor.Empty))); 

				foreach (var line in File.ReadAllLines(@"Resources\terrains.dat"))
				{
					var ss = line.Split(new[] { '|' }, StringSplitOptions.None);
					var terrain = (ETerrains)Enum.Parse(typeof(ETerrains), ss[0]);
					if (!AllTerrainTilesets.ContainsKey(terrain))
					{
						AllTerrainTilesets[terrain] = new TileSet();
					}
					var set = (ETextureSet)Enum.Parse(typeof(ETextureSet), ss[1]);
					var xy = Point.Parse(ss[2]);
					var tile = Rp.CreateTile(set, xy.X, xy.Y, FColor.Parse(ss[3]));
					AllTerrainTilesets[terrain].AddTile(tile);
				}
				foreach (var key in Enum.GetValues(typeof(ETerrains)).Cast<ETerrains>().Where(_key => !AllTerrainTilesets.ContainsKey(_key)))
				{
					Trace.WriteLine("Tile for Terrain " + key + " not defined.");
				}
				foreach (var line in File.ReadAllLines(@"Resources\tiles.dat"))
				{
					var ss = line.Split(new[] { '|' }, StringSplitOptions.None);
					var etile = (ETiles)Enum.Parse(typeof(ETiles), ss[0]);
					if (etile == ETiles.NONE)
					{
						continue;
					}
					var set = (ETextureSet)Enum.Parse(typeof(ETextureSet), ss[1]);
					var xy = Point.Parse(ss[2]);
					var tile = Rp.CreateTile(set, xy.X, xy.Y, FColor.Parse(ss[3]));
					AllTiles.Add(etile, new TileSet(tile));
				}
				foreach (var key in Enum.GetValues(typeof(ETiles)).Cast<ETiles>().Where(_key => !AllTiles.ContainsKey(_key)))
				{
					Trace.WriteLine("Tile for Tile" + key + " not defined.");
				}
			}
			return;

			#region old

			//foreach (ETerrains terrain in Enum.GetValues(typeof(ETerrains)))
			//{
			//    TileSet tl;
			//    switch (terrain)
			//    {
			//        case ETerrains.GROUND:
			//            tl = new TileSet(Rp.CreateTile(0, 0, FColor.FromArgb(10, 20, 10)));
			//            break;
			//        case ETerrains.GRASS:
			//            tl = new TileSet(
			//                Rp.CreateTile(3, 2, FColor.FromArgb(30, 50, 30)),
			//                Rp.CreateTile(5, 2, FColor.FromArgb(30, 60, 30)),
			//                Rp.CreateTile(7, 2, FColor.FromArgb(20, 80, 20)),
			//                Rp.CreateTile(12, 2, FColor.FromArgb(20, 100, 20)),
			//                Rp.CreateTile(14, 2, FColor.FromArgb(20, 120, 20)),
			//                Rp.CreateTile(ETextureSet.RB1, 5, 2, FColor.FromArgb(30, 60, 30)),
			//                Rp.CreateTile(ETextureSet.RB1, 7, 2, FColor.FromArgb(20, 80, 20)),
			//                Rp.CreateTile(ETextureSet.RB1, 12, 2, FColor.FromArgb(20, 100, 20)),
			//                Rp.CreateTile(ETextureSet.RB1, 14, 2, FColor.FromArgb(20, 120, 20)),
			//                Rp.CreateTile(ETextureSet.GP, 7, 2, FColor.FromArgb(20, 80, 20)),
			//                Rp.CreateTile(ETextureSet.GP, 12, 2, FColor.FromArgb(20, 100, 20)),
			//                Rp.CreateTile(ETextureSet.GP, 14, 2, FColor.FromArgb(20, 120, 20)),
			//                Rp.CreateTile(3, 2, FColor.FromArgb(30, 30, 50)),
			//                Rp.CreateTile(5, 2, FColor.FromArgb(30, 30, 60)),
			//                Rp.CreateTile(7, 2, FColor.FromArgb(20, 20, 80)),
			//                Rp.CreateTile(12, 2, FColor.FromArgb(20, 20, 70)),
			//                Rp.CreateTile(14, 2, FColor.FromArgb(20, 100, 120)),
			//                Rp.CreateTile(ETextureSet.RB1, 5, 2, FColor.FromArgb(30, 30, 60)),
			//                Rp.CreateTile(ETextureSet.RB1, 7, 2, FColor.FromArgb(20, 20, 80)),
			//                Rp.CreateTile(ETextureSet.RB1, 12, 2, FColor.FromArgb(20, 40, 80)),
			//                Rp.CreateTile(ETextureSet.RB1, 14, 2, FColor.FromArgb(20, 90, 30)),
			//                Rp.CreateTile(ETextureSet.GP, 7, 2, FColor.FromArgb(20, 80, 50)),
			//                Rp.CreateTile(ETextureSet.GP, 12, 2, FColor.FromArgb(20, 100, 60)),
			//                Rp.CreateTile(ETextureSet.GP, 14, 2, FColor.FromArgb(20, 120, 70)),
			//                Rp.CreateTile(2, 2, FColor.FromArgb(30, 50, 30)),
			//                Rp.CreateTile(ETextureSet.RB1, 2, 2, FColor.FromArgb(30, 60, 30)),
			//                Rp.CreateTile(ETextureSet.GP, 2, 2, FColor.FromArgb(20, 80, 20))
			//                );
			//            break;
			//        case ETerrains.ROAD:
			//            tl = new TileSet();
			//            break;
			//        case ETerrains.RED_BRICK_WALL:
			//            tl = new TileSet(Rp.CreateTile(0, 12, FColor.DarkRed));
			//            break;
			//        case ETerrains.YELLOW_BRICK_WALL:
			//            tl = new TileSet(Rp.CreateTile(0, 12, FColor.DarkGoldenrod));
			//            break;
			//        case ETerrains.GRAY_BRICK_WALL:
			//            tl = new TileSet(Rp.CreateTile(0, 12, FColor.Gray));
			//            break;
			//        case ETerrains.RED_BRICK_WINDOW:
			//            tl = new TileSet(Rp.CreateTile(1, 12, FColor.DarkRed));
			//            break;
			//        case ETerrains.GRAY_BRICK_WINDOW:
			//            tl = new TileSet(Rp.CreateTile(1, 12, FColor.DarkGoldenrod));
			//            break;
			//        case ETerrains.YELLOW_BRICK_WINDOW:
			//            tl = new TileSet(Rp.CreateTile(1, 12, FColor.Gray));
			//            break;
			//        case ETerrains.FRESH_WATER:
			//            tl = new TileSet(Rp.CreateTile(ETextureSet.RB1, 14, 8, FColor.Blue));
			//            break;
			//        case ETerrains.LAVA:
			//            tl = new TileSet(Rp.CreateTile(ETextureSet.RB1, 14, 8, FColor.Red));
			//            break;
			//        case ETerrains.SWAMP:
			//            tl = new TileSet(Rp.CreateTile(ETextureSet.RB1, 14, 8, FColor.DarkKhaki));
			//            break;
			//        case ETerrains.UP:
			//            tl = new TileSet(Rp.CreateTile(14, 1, FColor.Gray));
			//            break;
			//        case ETerrains.DOWN:
			//            tl = new TileSet(Rp.CreateTile(15, 1, FColor.Gray));
			//            break;
			//        case ETerrains.LEFT:
			//            tl = new TileSet(Rp.CreateTile(1, 1, FColor.Gray));
			//            break;
			//        case ETerrains.RIGHT:
			//            tl = new TileSet(Rp.CreateTile(0, 1, FColor.Gray));
			//            break;
			//        case ETerrains.STONE_FLOOR:
			//            tl = new TileSet(Rp.CreateTile(ETextureSet.RB1, 4, 8, FColor.FromArgb(255, 30, 30, 50)));
			//            break;
			//        case ETerrains.WOOD_FLOOR_MAPPLE:
			//            tl = new TileSet(Rp.CreateTile(ETextureSet.RB1, 13, 3, FColor.Maple.Multiply(0.4f)));
			//            break;
			//        case ETerrains.WOOD_FLOOR_OAK:
			//            tl = new TileSet(Rp.CreateTile(ETextureSet.RB1, 13, 3, FColor.DarkOak.Multiply(0.4f)));
			//            break;
			//        case ETerrains.STONE_WALL:
			//            tl = new TileSet(Rp.CreateTile(ETextureSet.GP, 3, 10, FColor.FromArgb(255, 100, 100, 200)));
			//            break;
			//        default:
			//            throw new ArgumentOutOfRangeException();
			//    }
			//    tl.Terrains = terrain;
			//    AllTerrainTilesets.Add(terrain, tl);
			//}

			//foreach (ETiles tile in Enum.GetValues(typeof(ETiles)))
			//{
			//    ATile tl;
			//    switch (tile)
			//    {
			//        case ETiles.STAIR_DOWN:
			//            tl = Rp.CreateTile(ETextureSet.GP, 14, 3, FColor.FromArgb(255, 100, 100, 50));
			//            break;
			//        case ETiles.STAIR_UP:
			//            tl = Rp.CreateTile(ETextureSet.RJ, 12, 3, FColor.FromArgb(255, 100, 100, 50));
			//            break;
			//        case ETiles.ON_WALL_LIGHT_SOURCE:
			//            tl = Rp.CreateTile(ETextureSet.HM, 0, 0, FColor.White);
			//            break;
			//        case ETiles.LIGHT_SOURCE:
			//            tl = Rp.CreateTile(ETextureSet.HM, 1, 0, FColor.White);
			//            break;
			//        case ETiles.DOOR:
			//            tl = Rp.CreateTile(5, 12, FColor.White);
			//            break;
			//        case ETiles.OPEN_DOOR:
			//            tl = Rp.CreateTile(4, 12, FColor.White);
			//            break;
			//        case ETiles.GUARD:
			//            tl = Rp.CreateTile(2, 0, FColor.White);
			//            break;
			//        case ETiles.TORCH:
			//            tl = Rp.CreateTile(ETextureSet.NH, 16, 11, FColor.White);
			//            break;
			//        case ETiles.SWORD:
			//            tl = Rp.CreateTile(ETextureSet.NH, 20, 10, FColor.White);
			//            break;
			//        case ETiles.AXE:
			//            tl = Rp.CreateTile(15, 2, FColor.LightSteelBlue);
			//            break;
			//        case ETiles.CROSSBOW:
			//            tl = Rp.CreateTile(ETextureSet.GP, 14, 14, FColor.SkyBlue);
			//            break;
			//        case ETiles.CHEST:
			//            tl = Rp.CreateTile(ETextureSet.RB1, 2, 9, FColor.Gold);
			//            break;
			//        case ETiles.MONSTER:
			//            //tl = Rp.CreateTile(ETextureSet.NH, 0, 6, FColor.White);
			//            tl = Rp.CreateTile(ETextureSet.NH, 0, 8, FColor.White);
			//            break;
			//        case ETiles.RING:
			//            tl = Rp.CreateTile(ETextureSet.GP, 8, 15, FColor.Gold);
			//            break;
			//        case ETiles.HEAP_OF_ITEMS:
			//            tl = Rp.CreateTile(ETextureSet.GP, 11, 0, FColor.DarkOrchid);
			//            break;
			//        case ETiles.POTION:
			//            tl = Rp.CreateTile(ETextureSet.RB1, 13, 10, FColor.Gray);
			//            break;
			//        case ETiles.CROSSBOW_BOLT:
			//            tl = Rp.CreateTile(ETextureSet.NH, 0, 10, FColor.White);
			//            break;
			//        case ETiles.TARGET_DOT:
			//            tl = Rp.CreateTile(ETextureSet.GP, 10, 15, FColor.Gold);
			//            break;
			//        case ETiles.TARGET_CROSS:
			//            tl = Rp.CreateTile(ETextureSet.RB1, 8, 5, FColor.Gold);
			//            break;
			//        case ETiles.FOG:
			//            tl = Rp.CreateTile(1, 11, FColor.FromArgb(255, 5, 5, 10));
			//            tl.IsFogTile = true;
			//            break;
			//        case ETiles.NONE:
			//            tl = null;
			//            break;
			//        case ETiles.BED:
			//            tl = Rp.CreateTile(ETextureSet.GP, 9, 14, FColor.White);
			//            break;
			//        case ETiles.CHAIR:
			//            tl = Rp.CreateTile(ETextureSet.GP, 2, 13, FColor.White);
			//            break;
			//        case ETiles.TABLE:
			//            tl = Rp.CreateTile(ETextureSet.GP, 1, 13, FColor.White);
			//            break;
			//        case ETiles.SIMPLE:
			//            tl = Rp.CreateTile(0, 12, FColor.Green);
			//            break;
			//        case ETiles.FRAME_L:
			//            tl = Rp.CreateTile(ETextureSet.GP, 3, 11, FColor.Gold);
			//            break;
			//        case ETiles.FRAME_R:
			//            tl = Rp.CreateTile(ETextureSet.GP, 3, 11, FColor.Gold);
			//            break;
			//        case ETiles.FRAME_T:
			//            tl = Rp.CreateTile(ETextureSet.GP, 4, 12, FColor.Gold);
			//            break;
			//        case ETiles.FRAME_B:
			//            tl = Rp.CreateTile(ETextureSet.GP, 4, 12, FColor.Gold);
			//            break;
			//        case ETiles.FRAME_TL:
			//            tl = Rp.CreateTile(ETextureSet.GP, 10, 13, FColor.Gold);
			//            break;
			//        case ETiles.FRAME_TR:
			//            tl = Rp.CreateTile(ETextureSet.GP, 15, 11, FColor.Gold);
			//            break;
			//        case ETiles.FRAME_BL:
			//            tl = Rp.CreateTile(ETextureSet.GP, 0, 12, FColor.Gold);
			//            break;
			//        case ETiles.FRAME_BR:
			//            tl = Rp.CreateTile(ETextureSet.GP, 9, 13, FColor.Gold);
			//            break;
			//        case ETiles.SOLID:
			//            tl = Rp.CreateTile(11, 13, FColor.White);
			//            break;
			//        default:
			//            throw new ArgumentOutOfRangeException();
			//    }
			//    if (tl != null)
			//    {
			//        tl.Tile = tile;
			//    }
			//    AllTiles.Add(tile, tl);
			//}

			#endregion
		}

		public static ATile GetTile(this ETiles _tile) { return AllTiles[_tile][0]; }

		public static ATile GetTile(this ETerrains terrains, int _index)
		{
			if(terrains==ETerrains.NONE)
			{
				return GetTile(ETiles.NONE);
			}
			var ts = AllTerrainTilesets[terrains];
			return ts[_index];
		}
	}
}