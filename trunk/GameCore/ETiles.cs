using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore
{
	public enum ETiles
	{
		[Tiles(0f)] NONE,
		[Tiles(0.5f)] HEAP_OF_ITEMS,
		[Tiles(0.2f)]FOG,
		[Tiles(0f)]TARGET_DOT,
		[Tiles(0f)]TARGET_CROSS,
		[Tiles(0f)]ON_WALL_LIGHT_SOURCE,
		[Tiles(0f)]LIGHT_SOURCE,
		[Tiles(0f)]TORCH,

		[Tiles(1f)] DOOR,
		[Tiles(0.03f)] OPEN_DOOR,
		[Tiles(0.2f)] CHEST,
		[Tiles(0f)] STAIR_DOWN,
		[Tiles(0.2f)] STAIR_UP,

		[Tiles(0.2f)]TABLE,
		[Tiles(0.1f)]CHAIR,
		[Tiles(0.4f)]BED,

		[Tiles(0.2f)] AVATAR,
		[Tiles(0.3f)] MONSTER,

		[Tiles(0.2f)] SWORD,
		[Tiles(0.3f)] AXE,
		[Tiles(0.2f)] CROSSBOW,
		[Tiles(0.01f)] RING,
		[Tiles(0.01f)] POTION,
		[Tiles(0.01f)] CROSSBOW_BOLT,

		[Tiles(0f)]FRAME_L,
		[Tiles(0f)]FRAME_R,
		[Tiles(0f)]FRAME_T,
		[Tiles(0f)]FRAME_B,
		[Tiles(0f)]FRAME_TL,
		[Tiles(0f)]FRAME_TR,
		[Tiles(0f)]FRAME_BL,
		[Tiles(0f)]FRAME_BR,
		[Tiles(0f)]SOLID,
		[Tiles(0f)]SIMPLE,
	}


	public class TilesAttribute : Attribute
	{
		private static Dictionary<ETiles, TilesAttribute> m_attrs;

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