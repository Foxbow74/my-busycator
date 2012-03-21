using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore
{
	public enum ETerrains
	{
		[Terrain("почва")] GROUND,
		[Terrain("вода")] WATER,
		[Terrain("трава")] GRASS,
		[Terrain("болото")]SWAMP,
		[Terrain("лава")]LAVA,
		[Terrain("дорога")] ROAD,
		[Terrain("гриб", 0.7f, 0.4f, true)] MUSHROOM,
		[Terrain("кирпичная стена", 0, 1, false)]RED_BRICK_WALL,
		[Terrain("кирпичная стена", 0, 1, false)]YELLOW_BRICK_WALL,
		[Terrain("кирпичная стена", 0, 1, false)]GRAY_BRICK_WALL,
		[Terrain("могила", 0.7f, 0.4f, true)] GRAVE,
		[Terrain("статуя", 0.7f, 0.8f, true)] STATUE,

		[Terrain("дубовый пол")]WOOD_FLOOR_OAK,
		[Terrain("кленовый пол")]WOOD_FLOOR_MAPPLE,

		[Terrain("каменный пол")]STONE_FLOOR,
		[Terrain("каменная стена",0,1,false)]STONE_WALL,

		[Terrain("up")]UP,
		[Terrain("down")]DOWN,
		[Terrain("left")]LEFT,
		[Terrain("right")]RIGHT,
		[Terrain("окно", 0, 0.1f, true)]RED_BRICK_WINDOW,
		[Terrain("окно", 0, 0.1f, true)]GRAY_BRICK_WINDOW,
		[Terrain("окно", 0, 0.1f, true)]YELLOW_BRICK_WINDOW,
	}

	public class TerrainAttribute : Attribute
	{
		private static Dictionary<ETerrains, TerrainAttribute> m_attrs;

		public TerrainAttribute(string _displayName)
			: this(_displayName, 1.0f, 0.0f, true)
		{
		}

		public TerrainAttribute(string _displayName, float _isPassable, float _transparency, bool _isCanShootThrough)
		{
			DisplayName = _displayName;
			IsPassable = _isPassable;
			Opacity = _transparency;
			IsCanShootThrough = _isCanShootThrough;
		}

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
	}
}