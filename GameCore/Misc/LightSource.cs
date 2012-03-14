using System;
using System.Collections.Generic;
using GameCore.Mapping;
using GameCore.Objects;

namespace GameCore.Misc
{
	public interface ILightSource
	{
		void LightCells(LiveMap _liveMap, Point _point);
		int Radius { get; }
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

		public int Radius
		{
			get { return m_radius; }
		}

		public void LightCells(LiveMap _liveMap, Point _point)
		{
			if (!m_lightManagers.ContainsKey(Radius))
			{
				m_lightManagers.Add(Radius, new LosManager(Radius));
			}
			m_lightManagers[Radius].LightCells(_liveMap, _point, Color);
		}
	}
}
