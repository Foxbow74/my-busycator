using System;
using System.Collections.Generic;
using GameCore.Misc;
using RusLanguage;

namespace GameCore
{
	public enum ETerrains
	{
		[Terrain("почва", ESex.FEMALE)] GROUND,
		[Terrain("вода", ESex.FEMALE)] WATER,
		[Terrain("трава", ESex.FEMALE)] GRASS,
		[Terrain("болото", ESex.IT)] SWAMP,
		[Terrain("лава", ESex.FEMALE)] LAVA,
		[Terrain("дорога", ESex.FEMALE)] ROAD,
		[Terrain("гриб", 0.7f, 0.4f, true)] MUSHROOM,
		[Terrain("стена", 0, 1, false, ESex.FEMALE)] RED_BRICK_WALL,
		[Terrain("стена", 0, 1, false, ESex.FEMALE)] YELLOW_BRICK_WALL,
		[Terrain("стена", 0, 1, false, ESex.FEMALE)] GRAY_BRICK_WALL,
		[Terrain("могила", 0.7f, 0.4f, true, ESex.FEMALE)] GRAVE,
		[Terrain("статуя", 0.7f, 0.8f, true, ESex.FEMALE)] STATUE,

		[Terrain("дубовый пол", ESex.MALE)] WOOD_FLOOR_OAK,
		[Terrain("кленовый пол", ESex.MALE)] WOOD_FLOOR_MAPPLE,
		[Terrain("каменный пол", ESex.MALE)] STONE_FLOOR,
		[Terrain("стена", 0, 1, false, ESex.FEMALE)] STONE_WALL,

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

		public TerrainAttribute(string _displayName, ESex _sex)
			: this(_displayName, 1.0f, 0.0f, true, _sex) { }

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
	}
}