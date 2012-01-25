using System;
using GameCore;
using GameCore.Misc;

namespace RGL1.UIBlocks.Map
{
	public static class MapVisualizator
	{
		public static Tile Tile(this ETerrains _terrain, Point _worldCoords, int _blockRandomSeed)
		{
			switch (_terrain)
			{
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