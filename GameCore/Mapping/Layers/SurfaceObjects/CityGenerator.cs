using System;
using System.Linq;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	class CityGenerator
	{
		private const int INITIAL_CITY_SIZE = 4;

		private readonly EMapBlockTypes[,] m_worldMap;

		public CityGenerator(EMapBlockTypes[,] _worldMap)
		{
			m_worldMap = _worldMap;
		}

		public EMapBlockTypes[,] GenerateCityArea(Random _rnd)
		{
			var center = new Point(m_worldMap.GetLength(0) / 2, m_worldMap.GetLength(1) / 2);
			LayerHelper.GetRandomPoints(center, _rnd, m_worldMap, INITIAL_CITY_SIZE, EMapBlockTypes.CITY, EMapBlockTypes.GROUND);
			return m_worldMap;
		}

		public void GenerateCityBlock(Random _rnd, Surface _surface, Point _blockId, MapBlock _block)
		{
			var rooms = LayerHelper.GenerateRooms(_block, _rnd, MapBlock.Rect.Inflate(-1, -1), new Point[0], _surface).OrderByDescending(_room => _room.RoomRectangle.Size).ToArray();
			var cnt = Math.Sqrt(rooms.Length);
			for (var index = 0; index < cnt; index++)
			{
				var building = new Building(rooms[index].RoomRectangle, rooms[index].AreaRectangle, _block, _surface, _rnd);
				_block.Rooms.Add(building);
			}
		}
	}
}
