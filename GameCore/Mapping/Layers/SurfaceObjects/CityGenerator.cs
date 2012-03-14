using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Misc;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	class CityGenerator
	{
		public const int INITIAL_CITY_SIZE = 4;
		public const int MAX_CITY_BUILDINGS_COUNT = 30;

		private readonly EMapBlockTypes[,] m_worldMap;

		public CityGenerator(EMapBlockTypes[,] _worldMap)
		{
			m_worldMap = _worldMap;
		}

		public IEnumerable<Point> GenerateCityArea(Random _rnd)
		{
			var center = new Point(m_worldMap.GetLength(0) / 2, m_worldMap.GetLength(1) / 2);
			return LayerHelper.GetRandomPoints(center, _rnd, m_worldMap, INITIAL_CITY_SIZE, EMapBlockTypes.CITY, EMapBlockTypes.GROUND).Select(_point => _point - center);
		}
	}
}
