using System;
using System.Collections.Generic;
using GameCore.Misc;
using RusLanguage;

namespace GameCore
{
	public enum ETerrains
	{
		[Terrain("ничто", ESex.IT, 0)]NONE,
		[Terrain("почва", ESex.FEMALE)]GROUND,
		[Terrain("вода", ESex.FEMALE, 0f)] FRESH_WATER,
		[Terrain("трава", ESex.FEMALE)] GRASS,
		[Terrain("болото", ESex.IT)] SWAMP,
		[Terrain("лава", ESex.FEMALE)] LAVA,
		[Terrain("дорога", ESex.FEMALE)] ROAD,
		[Terrain("стена", 0, 1, false, ESex.FEMALE)] RED_BRICK_WALL,
		[Terrain("стена", 0, 1, false, ESex.FEMALE)] YELLOW_BRICK_WALL,
		[Terrain("стена", 0, 1, false, ESex.FEMALE)] GRAY_BRICK_WALL,
		[Terrain("статуя", 0.7f, 0.8f, true, ESex.FEMALE)] STATUE,

		[Terrain("дубовый пол")] WOOD_FLOOR_OAK,
		[Terrain("кленовый пол")] WOOD_FLOOR_MAPPLE,
		[Terrain("каменный пол")] STONE_FLOOR,
		[Terrain("стена", 0, 1, false, ESex.FEMALE)] STONE_WALL,

		[Terrain("лес")] FOREST,
        [Terrain("море", ESex.IT, 0f)] SEA,
        [Terrain("океан", ESex.MALE, 0f)] DEEP_SEA,
        [Terrain("вода", ESex.FEMALE, 0f)] DEEP_FRESH_WATER,
		[Terrain("песок")] COAST,
		[Terrain("песок")] LAKE_COAST,
		[Terrain("скала",ESex.FEMALE)] MOUNT,
		[Terrain("ледник")] ETERNAL_SNOW,
		[Terrain("кустарник")] SHRUBS,


		[Terrain("up", ESex.IT)] UP,
		[Terrain("down", ESex.IT)] DOWN,
		[Terrain("left", ESex.IT)] LEFT,
		[Terrain("right", ESex.IT)] RIGHT,
		[Terrain("окно", 0, 0.1f, true, ESex.IT)] RED_BRICK_WINDOW,
		[Terrain("окно", 0, 0.1f, true, ESex.IT)] GRAY_BRICK_WINDOW,
		[Terrain("окно", 0, 0.1f, true, ESex.IT)] YELLOW_BRICK_WINDOW,
	}

	public class TerrainAttribute : Attribute
	{
		private static Dictionary<ETerrains, TerrainAttribute> m_attrs;

		public TerrainAttribute(string _displayName, ESex _sex = ESex.MALE)
			: this(_displayName, 1.0f, 0.0f, true, _sex) { }

        public TerrainAttribute(string _displayName, ESex _sex, float isPassable)
            : this(_displayName, isPassable, 0.0f, true, _sex) { }

		public TerrainAttribute(string _displayName, float _isPassable, float _transparency, bool _isCanShootThrough, ESex _sex = ESex.MALE)
		{
			DisplayName = _displayName;
			IsPassable = _isPassable;
			Opacity = _transparency;
			IsCanShootThrough = _isCanShootThrough;
			Sex = _sex;
		}

		public ESex Sex { get; private set; }

		public string DisplayName { get; private set; }
		public float IsPassable { get; private set; }
		public float Opacity { get; private set; }
		public bool IsCanShootThrough { get; private set; }

		public static TerrainAttribute GetAttribute(ETerrains _enum)
		{
			if (m_attrs == null)
			{
				m_attrs = Util.Fill<ETerrains, TerrainAttribute>();
			}
			return m_attrs[_enum];
		}

		public static EMapBlockTypes GetMapBlockType(ETerrains terrains)
		{
			switch (terrains)
			{
				case ETerrains.GRASS:
				case ETerrains.GROUND:
				case ETerrains.ROAD:
				case ETerrains.RED_BRICK_WALL:
				case ETerrains.YELLOW_BRICK_WALL:
				case ETerrains.GRAY_BRICK_WALL:
				case ETerrains.STATUE:
				case ETerrains.WOOD_FLOOR_OAK:
				case ETerrains.WOOD_FLOOR_MAPPLE:
				case ETerrains.STONE_FLOOR:
				case ETerrains.RED_BRICK_WINDOW:
				case ETerrains.GRAY_BRICK_WINDOW:
				case ETerrains.YELLOW_BRICK_WINDOW:
				case ETerrains.STONE_WALL:
					return EMapBlockTypes.GROUND;
				case ETerrains.FRESH_WATER:
					return EMapBlockTypes.FRESH_WATER;
				case ETerrains.DEEP_FRESH_WATER:
					return EMapBlockTypes.DEEP_FRESH_WATER;
				case ETerrains.SWAMP:
					return EMapBlockTypes.SWAMP;
				case ETerrains.LAKE_COAST:
					return EMapBlockTypes.LAKE_COAST;
				case ETerrains.COAST:
					return EMapBlockTypes.COAST;
				case ETerrains.DEEP_SEA:
					return EMapBlockTypes.DEEP_SEA;
				case ETerrains.ETERNAL_SNOW:
					return EMapBlockTypes.ETERNAL_SNOW;
				case ETerrains.FOREST:
					return EMapBlockTypes.FOREST;
				case ETerrains.MOUNT:
					return EMapBlockTypes.MOUNT;
				case ETerrains.SEA:
					return EMapBlockTypes.SEA;
				case ETerrains.SHRUBS:
					return EMapBlockTypes.SHRUBS;
				default:
					throw new ArgumentOutOfRangeException("terrains");
			}
		}
	}
}