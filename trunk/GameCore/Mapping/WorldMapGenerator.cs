using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore.Mapping
{
	internal class WorldMapGenerator
	{
		private readonly Random m_rnd = new Random();//World.WorldSeed);
		private readonly int m_size;

		public WorldMapGenerator(int _size)
		{
			m_size = _size;
		}

		public EMapBlockTypes[,] Generate1()
		{
			var map = new EMapBlockTypes[m_size, m_size];
			var center = new Point(m_size / 2, m_size / 2);
			var size = m_rnd.Next(830) + 80;
			var list = new List<Point>(){center};
			do
			{
				var point = list[m_rnd.Next(list.Count)];
				list.AddRange(Add(point, map, ref size));
			} while (size > 0);
			return map;
		}

		private IEnumerable<Point> Add(Point _xy,  EMapBlockTypes[,] _map, ref int _size)
		{
			var list = new List<Point>();
			if (_map[_xy.X, _xy.Y] != EMapBlockTypes.GROUND)
			{
				list.Add(_xy);
				_map[_xy.X, _xy.Y] = EMapBlockTypes.GROUND;
				if (_size == 0 || m_rnd.NextDouble() < 0.1)
				{
					return list;
				}
				_size--;
			}
			var dirs = m_rnd.GetRandomDirections();

			foreach (var dir in dirs.AllDirectionsIn())
			{
				var xy = _xy + dir.GetDelta();
				if (_map.GetLength(0) <= xy.X || xy.X<0) continue;
				if (_map.GetLength(1) <= xy.Y || xy.Y<0) continue;

				if (_map[xy.X, xy.Y] != EMapBlockTypes.GROUND)
				{
					list.AddRange(Add(xy, _map, ref _size));
				}
			}
			return list;
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