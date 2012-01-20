using System;

namespace GameCore
{
	public enum ETerrains
	{
		GROUND,
		WATER,
		GRASS,
		SWAMP,
		ROAD,
		MUSHROOM,
		BRICK
	}

	public static class TerrainsHelper
	{
		public static bool IsPassable(this ETerrains _terrains)
		{
			switch (_terrains)
			{
				case ETerrains.GROUND:
				case ETerrains.GRASS:
				case ETerrains.SWAMP:
				case ETerrains.ROAD:
					return true;
			}
			return false;
		}
	}

	public enum EItems
	{
		NONE,
		WEAPON,
		CHEST,
		DOOR,
	}
}