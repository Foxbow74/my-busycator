using System.Collections.Generic;
using GameCore.Mapping;

namespace GameCore.Misc
{
	public class LightSource
	{
		private static readonly Dictionary<int, LosManager> m_lightManagers = new Dictionary<int, LosManager>();

		private readonly int m_radius;
		private readonly FColor m_fColor;

		public LightSource(int _radius, FColor _fColor)
		{
			m_radius = _radius;
			m_fColor = _fColor;
		}

		public void LightCells(LiveMap _liveMap, Point _point)
		{
			if (!m_lightManagers.ContainsKey(m_radius))
			{
				m_lightManagers.Add(m_radius, new LosManager(m_radius));
			}
			m_lightManagers[m_radius].LightCells(_liveMap, _point, m_fColor);
		}
	}
}
