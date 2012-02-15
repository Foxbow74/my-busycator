using System;
using GameCore.Misc;

namespace GameCore.Mapping
{
	internal class WorldMapGenerator
	{
		private readonly Random m_rnd = new Random(World.WorldSeed);
		private readonly int m_size;

		public WorldMapGenerator(int _size)
		{
			m_size = _size;
		}

		public EMapBlockTypes[,] Generate()
		{
			var map = new EMapBlockTypes[m_size,m_size];

			var radius = m_size/2 - 2;

			var center = new Point(m_size/2, m_size/2);

			for (var i = 0; i < m_size; ++i)
			{
				for (var j = 0; j < m_size; ++j)
				{
					var pnt = new Point(i, j) - center;
					var r = pnt.Lenght;
					var d = r/radius - 0.5;
					if (d > 0)
					{
						if (m_rnd.NextDouble() < d*2)
						{
							map[i, j] = EMapBlockTypes.NONE;
							continue;
						}
					}
					d += 0.3;
					if (d > 0)
					{
						if (m_rnd.NextDouble() < d)
						{
							map[i, j] = EMapBlockTypes.SEA;
							continue;
						}
					}
					map[i, j] = EMapBlockTypes.GROUND;
				}
			}
			return map;
		}
	}
}