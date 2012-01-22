using System;
using Graphics;

namespace GameCore
{
	public static class MapVisualizator
	{
		public static Tile Tile(this ETerrains _terrain, Point _worldCoords, int _blockRandomSeed)
		{
			switch (_terrain)
			{
				case ETerrains.GROUND:
					return Tiles.GrowndTile;
				case ETerrains.GRASS:
					return Tiles.GrassTiles[Math.Abs((_worldCoords.GetHashCode() ^ _blockRandomSeed) % Tiles.GrassTiles.Length)];
				case ETerrains.MUSHROOM:
					return Tiles.MashtoomTiles[Math.Abs((_worldCoords.GetHashCode() ^ _blockRandomSeed) % Tiles.MashtoomTiles.Length)];
				case ETerrains.BRICK_WALL:
					return Tiles.BrickTile;
				case ETerrains.WINDOW:
					return Tiles.BrickWindowTile;
				default:
					throw new ArgumentOutOfRangeException("_terrain");
			}
		}

		public static Tile Tile(this EItems _item)
		{
			switch (_item)
			{
				case EItems.AXE:
					return Tiles.Axe;
				case EItems.SWORD:
					return Tiles.SwordTile;
				case EItems.CHEST:
					return Tiles.ChestTile;
				case EItems.DOOR:
					return Tiles.DoorTile;
				default:
					throw new ArgumentOutOfRangeException("_item");
			}
		}
	}
}