using System.Collections.Generic;
using GameCore.Mapping;
using GameCore.Objects;

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
			OnWall = new TileInfoProvider(ETiles.ON_WALL_LIGHT_SOURCE, new FColor(1f, _fColor));
		}

		public TileInfoProvider OnWall { get; private set; }

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
