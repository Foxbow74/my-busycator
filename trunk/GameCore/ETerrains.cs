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
		[Terrain("болото")] SWAMP,
		[Terrain("дорога")] ROAD,
		[Terrain("гриб", 0.4f, 0.3f)] MUSHROOM,
		[Terrain("кирпичная стена", 0, 1)] BRICK_WALL,
		[Terrain("окно", 0, 0.2f)] WINDOW,
	}

	public class TerrainAttribute : Attribute
	{
		private static Dictionary<ETerrains, TerrainAttribute> m_attrs;

		public TerrainAttribute(string _displayName)
			: this(_displayName, 1.0f, 0.0f)
		{
		}

		public TerrainAttribute(string _displayName, float _isPassable, float _transparency)
		{
			DisplayName = _displayName;
			IsPassable = _isPassable;
			Opaque = _transparency;
		}

		public string DisplayName { get; private set; }
		public float IsPassable { get; private set; }
		public float Opaque { get; private set; }

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