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
		private static readonly Dictionary<Tuple<FColor, EDirections>, TileInfoProvider> m_onWalls = new Dictionary<Tuple<FColor, EDirections>, TileInfoProvider>();

		private readonly int m_radius;
		private readonly FColor m_fColor;
		private readonly FColor m_lerpColor;

		public LightSource(int _radius, FColor _fColor)
		{
			m_radius = _radius;
			m_fColor = _fColor;
			m_lerpColor = new FColor(1f, _fColor);
			foreach (var dir in Util.AllDirections)
			{
				var key = Tuple.Create(m_lerpColor, dir);
				if(!m_onWalls.ContainsKey(key))
				{
					m_onWalls.Add(key, new TileInfoProvider(ETiles.ON_WALL_LIGHT_SOURCE, key.Item1, key.Item2));
				}
			}
		}

		public TileInfoProvider GetOnWallTileInfo(EDirections _directions)
		{
			return m_onWalls[new Tuple<FColor, EDirections>(m_lerpColor, _directions)];
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
