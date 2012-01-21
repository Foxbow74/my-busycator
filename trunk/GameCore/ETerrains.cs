using System;
using Common;
using System.Linq;
using System.Reflection;

namespace GameCore
{
	public enum ETerrains
	{
		[Terrain("почва")]
		GROUND,
		[Terrain("вода")]
		WATER,
		[Terrain("трава")]
		GRASS,
		[Terrain("болото")]
		SWAMP,
		[Terrain("дорога")]
		ROAD,
		[Terrain("гриб", false)]
		MUSHROOM,
		[Terrain("кирпичная стена", false)]
		BRICK_WALL
	}

	public class TerrainAttribute : Attribute
	{
		public string DisplayName { get; private set; }
		public bool IsPassable { get; private set; }

		public TerrainAttribute(string _displayName):this(_displayName, true)
		{}

		public TerrainAttribute(string _displayName, bool _isPassable)
		{
			DisplayName = _displayName;
			IsPassable = _isPassable;
		}

		public static TerrainAttribute GetAttribute(ETerrains _enum)
		{
			return _enum.GetAttribute<ETerrains, TerrainAttribute>();
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