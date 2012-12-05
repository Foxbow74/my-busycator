using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore
{
	public enum ETerrains
	{
		[Terrain(0)]NONE,
		[Terrain]GROUND,
		[Terrain(0f)] FRESH_WATER,
		[Terrain] GRASS,
		[Terrain] SWAMP,
		[Terrain] LAVA,
		[Terrain] ROAD,
		[Terrain(0, 1, false)] RED_BRICK_WALL,
		[Terrain(0, 1, false)] YELLOW_BRICK_WALL,
		[Terrain(0, 1, false)] GRAY_BRICK_WALL,
		[Terrain(0.7f, 0.8f, true)] STATUE,

		[Terrain] WOOD_FLOOR_OAK,
		[Terrain] WOOD_FLOOR_MAPPLE,
		[Terrain] STONE_FLOOR,
		[Terrain(0, 1, false)] STONE_WALL,

		[Terrain] FOREST,
        [Terrain(0f)] SEA,
        [Terrain(0f)] DEEP_SEA,
        [Terrain(0f)] DEEP_FRESH_WATER,
		[Terrain] COAST,
		[Terrain] LAKE_COAST,
		[Terrain(0.1f,0.5f,false)] MOUNT,
		[Terrain] ETERNAL_SNOW,
		[Terrain(0.5f,0.7f,true)] SHRUBS,


		[Terrain] UP,
		[Terrain] DOWN,
		[Terrain] LEFT,
		[Terrain] RIGHT,
		[Terrain(0, 0.1f, true)] RED_BRICK_WINDOW,
		[Terrain(0, 0.1f, true)] GRAY_BRICK_WINDOW,
		[Terrain(0, 0.1f, true)] YELLOW_BRICK_WINDOW,
	}

	public class TerrainAttribute : Attribute
	{
		private static Dictionary<ETerrains, TerrainAttribute> m_attrs;

		public TerrainAttribute()
			: this(1.0f, 0.0f, true) { }

        public TerrainAttribute(float _isPassable)
            : this(_isPassable, 0.0f, true) { }

		public TerrainAttribute(float _isPassable, float _transparency, bool _isCanShootThrough)
		{
			Passability = _isPassable;
			IsPassable = _isPassable > 0;
			Opacity = _transparency;
			IsCanShootThrough = _isCanShootThrough;
		}

		public float Passability { get; private set; }
		public bool IsPassable { get; private set; }
		public bool IsNotPassable { get { return !IsPassable; } }
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

		public static EMapBlockTypes GetMapBlockType(ETerrains _terrains)
		{
			switch (_terrains)
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
				case ETerrains.NONE:
					return EMapBlockTypes.NONE;
				default:
					throw new ArgumentOutOfRangeException("_terrains");
			}
		}
	}
}