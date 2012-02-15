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
		[Tiles(0f)]
		TARGET_DOT,
		[Tiles(0f)] TARGET_CROSS,

		[Tiles(0f)] GRASS,
		[Tiles(1f)] BRICK,
		[Tiles(0.2f)] BRICK_WINDOW,
		[Tiles(0.9f)] MASHROOM,
		[Tiles(0f)] GROUND,
		[Tiles(0f)]STONE_FLOOR,
		[Tiles(0f)]STONE_WALL,

		[Tiles(1f)] DOOR,
		[Tiles(0.03f)] OPEN_DOOR,
		[Tiles(0.2f)] CHEST,
		[Tiles(0f)] STAIR_DOWN,
		[Tiles(0.2f)] STAIR_UP,

		[Tiles(0.2f)] AVATAR,
		[Tiles(0.3f)] MONSTER,

		[Tiles(0.2f)] SWORD,
		[Tiles(0.3f)] AXE,
		[Tiles(0.2f)] CROSSBOW,
		[Tiles(0.01f)] RING,
		[Tiles(0.01f)] POTION,
		[Tiles(0.01f)] CROSSBOW_BOLT,
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