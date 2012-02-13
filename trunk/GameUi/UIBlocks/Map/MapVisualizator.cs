using System;
using GameCore;
using GameCore.Misc;

namespace GameUi.UIBlocks.Map
{
	public static class MapVisualizator
	{
		public static ATile Tile(this ETerrains _terrain, Point _worldCoords, int _blockRandomSeed)
		{
			switch (_terrain)
			{
				case ETerrains.STONE_FLOOR:
					return ETiles.STONE_FLOOR.GetTile();
				case ETerrains.STONE_WALL:
					return ETiles.STONE_WALL.GetTile();
				case ETerrains.GROUND:
					return ETiles.GROUND.GetTile();
				case ETerrains.GRASS:
					return ETiles.GRASS.GetTile(Math.Abs((_worldCoords.GetHashCode() ^ _blockRandomSeed)));
				case ETerrains.MUSHROOM:
					return ETiles.MASHROOM.GetTile(Math.Abs((_worldCoords.GetHashCode() ^ _blockRandomSeed)));
				case ETerrains.BRICK_WALL:
					return ETiles.BRICK.GetTile();
				case ETerrains.WINDOW:
					return ETiles.BRICK_WINDOW.GetTile();
				default:
					throw new ArgumentOutOfRangeException("_terrain");
			}
		}
	}
}