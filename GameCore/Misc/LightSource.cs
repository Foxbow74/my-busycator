using System;
using System.Collections.Generic;
using GameCore.Mapping;
using GameCore.Objects;

namespace GameCore.Misc
{
	public interface ILightSource
	{
		void LightCells(LiveMap _liveMap, Point _point);
	}

	public class LightSource : ILightSource
	{
		private static readonly Dictionary<int, LosManager> m_lightManagers = new Dictionary<int, LosManager>();

		private readonly int m_radius;
		private readonly FColor m_color;

		public LightSource(int _radius, FColor _color)
		{
			m_radius = _radius;
			m_color = _color;
		}

		public FColor Color
		{
			get { return m_color; }
		}

		public void LightCells(LiveMap _liveMap, Point _point)
		{
			if (!m_lightManagers.ContainsKey(m_radius))
			{
				m_lightManagers.Add(m_radius, new LosManager(m_radius));
			}
			m_lightManagers[m_radius].LightCells(_liveMap, _point, Color);
		}
	}
}
