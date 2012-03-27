using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore
{
	public enum ETiles
	{
		NONE,
		[Tiles(0.5f)] HEAP_OF_ITEMS,
		[Tiles(0.2f)] FOG,
		TARGET_DOT,
		TARGET_CROSS,
		ON_WALL_LIGHT_SOURCE,
		LIGHT_SOURCE,
		TORCH,

		[Tiles(0.9f)] CABINET,
		[Tiles(0.5f)] ARMOR_RACK,
		[Tiles(0.5f)] WEAPON_RACK,
		[Tiles(1f)] DOOR,
		[Tiles(0.03f)] OPEN_DOOR,
		[Tiles(0.2f)] CHEST,
		[Tiles(0.4f)] GRAVE,

		STAIR_DOWN,
		[Tiles(0.2f)] STAIR_UP,

		[Tiles(0.2f)] TABLE,
		[Tiles(0.1f)] CHAIR,
		[Tiles(0.4f)] BED,
		[Tiles(0.4f)] BARREL,

		[Tiles(0.2f)] GUARD,
		[Tiles(0.2f)] CITIZEN_MALE,
		[Tiles(0.2f)] CITIZEN_MALE2,
		[Tiles(0.2f)] CITIZEN_FEMALE,
		[Tiles(0.2f)] CITIZEN_FEMALE2,
		[Tiles(0.3f)] MONSTER,

		[Tiles(0.2f)] SWORD,
		[Tiles(0.2f)] MAGIC_SWORD,
		[Tiles(0.3f)] AXE,
		[Tiles(0.2f)] CROSSBOW,
		[Tiles(0.01f)] RING,
		[Tiles(0.01f)] POTION,
		[Tiles(0.01f)] CROSSBOW_BOLT,

		#region frame tiles

		FRAME_L,
		FRAME_R,
		FRAME_T,
		FRAME_B,
		FRAME_TL,
		FRAME_TR,
		FRAME_BL,
		FRAME_BR,
		SOLID,
		SIMPLE,

		#endregion

		#region forest tiles

		[Tiles(0.3f)] FOREST_TREE_OAK,
		[Tiles(0.5f)] FOREST_TREE_MAPLE,
		[Tiles(0.4f)] FOREST_TREE_ELM,
		[Tiles(0.2f)] FOREST_TREE_ASH,
		[Tiles(0.3f)] FOREST_TREE_WILLOW,
		[Tiles(0.3f)] FOREST_TREE_WALNUT,

		[Tiles(0.4f)] FOREST_TREE_PINE,
		[Tiles(0.3f)] FOREST_TREE_SPRUCE,

		[Tiles(0.2f)] FOREST_SHRUB_1,
		[Tiles(0.2f)] FOREST_SHRUB_2,
		[Tiles(0.2f)] FOREST_SHRUB_3,
		[Tiles(0.2f)] FOREST_SHRUB_4,
		[Tiles(0.2f)] FOREST_SHRUB_5,

		#endregion

	}


	public class TilesAttribute : Attribute
	{
		private static Dictionary<ETiles, TilesAttribute> m_attrs;

		public TilesAttribute() : this(0f) { }

		public TilesAttribute(float _opacity)
		{
			Opacity = _opacity;
		}

		public float Opacity { get; private set; }

		public static TilesAttribute GetAttribute(ETiles _enum)
		{
			if (m_attrs == null)
			{
				m_attrs = Util.Fill<ETiles, TilesAttribute>();
			}
			return m_attrs[_enum];
		}
	}
}