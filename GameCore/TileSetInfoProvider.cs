using System.Collections.Generic;

namespace GameCore
{
	public static class TileSetInfoProvider
	{
		public static Dictionary<ETileset, List<float>> m_opacities = new Dictionary<ETileset, List<float>>();

		static TileSetInfoProvider()
		{
			m_opacities.Add(ETileset.NONE, new List<float> {0f});
		}

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