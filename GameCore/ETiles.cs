using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore
{
	public enum ETiles
	{
		NONE,
		[Tiles(0.5f)] HEAP_OF_ITEMS,
		[Tiles(0.2f)]FOG,
		TARGET_DOT,
		TARGET_CROSS,
		ON_WALL_LIGHT_SOURCE,
		LIGHT_SOURCE,
		TORCH,

		[Tiles(1f)] DOOR,
		[Tiles(0.03f)] OPEN_DOOR,
		[Tiles(0.2f)] CHEST,
		 STAIR_DOWN,
		[Tiles(0.2f)] STAIR_UP,

		[Tiles(0.2f)]TABLE,
		[Tiles(0.1f)]CHAIR,
		[Tiles(0.4f)]BED,

		[Tiles(0.2f)] AVATAR,
		[Tiles(0.3f)] MONSTER,

		[Tiles(0.2f)] SWORD,
		[Tiles(0.2f)] MAGIC_SWORD,
		[Tiles(0.3f)] AXE,
		[Tiles(0.2f)] CROSSBOW,
		[Tiles(0.01f)] RING,
		[Tiles(0.01f)] POTION,
		[Tiles(0.01f)] CROSSBOW_BOLT,

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
	}


	public class TilesAttribute : Attribute
	{
		private static Dictionary<ETiles, TilesAttribute> m_attrs;
		
		public TilesAttribute():this(0f)
		{}

		public TilesAttribute(float _opacity)
		{
			//IsPassable = _isPassable;
			Opacity = _opacity;
			//IsCanShootThrough = _isCanShootThrough;
		}

		//public float IsPassable { get; private set; }
		public float Opacity { get; private set; }
		//public bool IsCanShootThrough { get; private set; }

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