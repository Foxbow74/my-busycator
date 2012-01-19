using System;
using GameCore;
using Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1
{
	public static class MapVisualizator
	{
		public static void Draw(this MapCell _mapCell, SpriteBatch _spriteBatch, int _x, int _y)
		{
			_mapCell.Terrain.Tile(_mapCell.WorldCoords, _mapCell.BlockRandomSeed).DrawAtCell(_spriteBatch, _x, _y);
			if(_mapCell.Item!=EItems.NONE)
			{
				_mapCell.Item.Tile(_mapCell.WorldCoords, _mapCell.BlockRandomSeed).DrawAtCell(_spriteBatch, _x, _y);
			}
		}

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
				default:
					throw new ArgumentOutOfRangeException("_terrain");
			}
		}

		public static Tile Tile(this EItems _item, Point _worldCoords, int _blockRandomSeed)
		{
			switch (_item)
			{
				case EItems.WEAPON:
					return Tiles.WeaponTile;
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