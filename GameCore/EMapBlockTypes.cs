using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore
{
	public enum EMapBlockTypes
	{
		[MapBlockType("ничто")] NONE,
		[MapBlockType("суша")] GROUND,
		[MapBlockType("лес")] FOREST,
		[MapBlockType("море")] SEA,
		[MapBlockType("океан")] DEEP_SEA,
		[MapBlockType("вода")] FRESH_WATER,
		[MapBlockType("вода")] DEEP_FRESH_WATER,
		[MapBlockType("суша")] CITY,
		[MapBlockType("берег")] COAST,
		[MapBlockType("берег")] LAKE_COAST,
		[MapBlockType("горы")] MOUNT,
		[MapBlockType("болото")]SWAMP,
		[MapBlockType("вечный снег")]ETERNAL_SNOW,
		[MapBlockType("редколесье")]SHRUBS,

	}

	public class MapBlockTypeAttribute : Attribute
	{
		private static Dictionary<EMapBlockTypes, MapBlockTypeAttribute> m_attrs;

		public MapBlockTypeAttribute(string _displayName) { DisplayName = _displayName; }

		public string DisplayName { get; private set; }

		public static MapBlockTypeAttribute GetAttribute(EMapBlockTypes _enum)
		{
			if (m_attrs == null)
			{
				m_attrs = Util.Fill<EMapBlockTypes, MapBlockTypeAttribute>();
			}
			return m_attrs[_enum];
		}
	}
}