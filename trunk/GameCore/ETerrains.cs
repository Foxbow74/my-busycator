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
	}

	public static class TerrainsHelper
	{
		public static bool IsPassable(this ETerrains _terrains)
		{
			return _terrains != ETerrains.MUSHROOM;
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