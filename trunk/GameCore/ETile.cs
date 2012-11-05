using System;
using System.Collections.Generic;
using GameCore.Misc;
using RusLanguage;

namespace GameCore
{
	public enum ETile
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
		private static Dictionary<ETile, TerrainAttribute> m_attrs;

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

		public static TerrainAttribute GetAttribute(ETile _enum)
		{
			if (m_attrs == null)
			{
				m_attrs = Util.Fill<ETile, TerrainAttribute>();
			}
			return m_attrs[_enum];
		}

		public static EMapBlockTypes GetMapBlockType(ETile _tile)
		{
			switch (_tile)
			{
				case ETile.GRASS:
				case ETile.GROUND:
				case ETile.ROAD:
				case ETile.RED_BRICK_WALL:
				case ETile.YELLOW_BRICK_WALL:
				case ETile.GRAY_BRICK_WALL:
				case ETile.STATUE:
				case ETile.WOOD_FLOOR_OAK:
				case ETile.WOOD_FLOOR_MAPPLE:
				case ETile.STONE_FLOOR:
				case ETile.RED_BRICK_WINDOW:
				case ETile.GRAY_BRICK_WINDOW:
				case ETile.YELLOW_BRICK_WINDOW:
				case ETile.STONE_WALL:
					return EMapBlockTypes.GROUND;
				case ETile.FRESH_WATER:
					return EMapBlockTypes.FRESH_WATER;
				case ETile.DEEP_FRESH_WATER:
					return EMapBlockTypes.DEEP_FRESH_WATER;
				case ETile.SWAMP:
					return EMapBlockTypes.SWAMP;
				case ETile.LAKE_COAST:
					return EMapBlockTypes.LAKE_COAST;
				case ETile.COAST:
					return EMapBlockTypes.COAST;
				case ETile.DEEP_SEA:
					return EMapBlockTypes.DEEP_SEA;
				case ETile.ETERNAL_SNOW:
					return EMapBlockTypes.ETERNAL_SNOW;
				case ETile.FOREST:
					return EMapBlockTypes.FOREST;
				case ETile.MOUNT:
					return EMapBlockTypes.MOUNT;
				case ETile.SEA:
					return EMapBlockTypes.SEA;
				case ETile.SHRUBS:
					return EMapBlockTypes.SHRUBS;
				default:
					throw new ArgumentOutOfRangeException("_tile");
			}
		}
	}
}