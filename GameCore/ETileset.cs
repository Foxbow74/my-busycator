﻿using System.Collections.Generic;

namespace GameCore
{
	public enum ETileset
	{
		NONE,
		HEAP_OF_ITEMS,
		FOG,
		TARGET_DOT,
		TARGET_CROSS,
		ON_WALL_LIGHT_SOURCE,
		LIGHT_SOURCE,
		TORCH,

		ZZ33,
		ZZ34,
		ZZ35,
		ZZ36,
		ZZ37,
		ZZ38,
		ZZ39,

		ZZ40,
        ZZ41,

        ZZ42,
        ZZ43,
        ZZ44,
        ZZ45,

		GUARD,
		FURNITURE,
		ZZ12,
		ZZ13,
		ZZ14,
		MONSTER,

		SWORD,
		MAGIC_SWORD,
		AXE,
		CROSSBOW,
		RING,
		POTION,
		CROSSBOW_BOLT,

		ZZ01,
		ZZ02,
		ZZ03,
		ZZ04,
		ZZ05,
		ZZ06,
		ZZ07,
		ZZ08,
		ZZ09,
		ZZ10,

		ZZ15,
		ZZ16,
		ZZ17,
		ZZ18,
		ZZ19,
		ZZ20,

		ZZ21,
		ZZ22,

		ZZ23,
		ZZ24,
		ZZ25,
		ZZ26,
		ZZ27,

		ZZ28,
		ZZ29,
		ZZ30,
		ZZ31,
		ZZ32,

		CITIZEN,
		AVATAR,
		TREES,
		SHRUBS,
		MUSHROOMS,
		FRAME1,
		FRAME2,
		FRAME3,
	}

	public static class TileInfoProvider
	{
		public static Dictionary<ETileset, List<float>> m_opacities = new Dictionary<ETileset, List<float>>();

		public static void SetOpacity(ETileset _tileset, int _index, float _opacity)
		{
			List<float> list;
			if (!m_opacities.TryGetValue(_tileset, out list))
			{
				list = new List<float>();
				m_opacities.Add(_tileset, list);
			}
			while (list.Count <= _index)
			{
				list.Add(0f);
			}
			list[_index] = _opacity;
		}

		public static float GetOpacity(ETileset _tileset, int _index)
		{
			var list = m_opacities[_tileset];
			return list[_index % list.Count];
		}
	}
}