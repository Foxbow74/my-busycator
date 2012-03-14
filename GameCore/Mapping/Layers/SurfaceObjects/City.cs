using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	public class City
	{
		private readonly Point[] m_points;
		private readonly CityGenerator m_cityGenerator;
		readonly Dictionary<Building, Point> m_buildings = new Dictionary<Building, Point>();
		private readonly List<Citizen> m_citizens = new List<Citizen>();

		public City(Surface _surface, Random _rnd)
		{
			Surface = _surface;
			m_cityGenerator = new CityGenerator(_surface.WorldMap);
			m_points = m_cityGenerator.GenerateCityArea(_rnd).ToArray();

			AddBuildings(_rnd);
		}

		private void AddBuildings(Random _rnd)
		{
			var allRooms = new Dictionary<Room, Point>();
			foreach (var point in m_points)
			{
				var rooms = LayerHelper.GenerateRooms(_rnd, MapBlock.Rect.Inflate(-2, -2).Offset(2,2), new Point[0]).OrderByDescending(_room => _room.RoomRectangle.Size);
				foreach (var room in rooms)
				{
					allRooms.Add(room, point);
					if (allRooms.Count > CityGenerator.MAX_CITY_BUILDINGS_COUNT)
					{
						break;
					}
				} 
			}

			var abuildings = Util.GetAllTypesOf<Building>().Select(_type => (Building) Activator.CreateInstance(_type, new object[] {this})).ToArray();

			var buildings = new List<Building>();

			foreach (var building in abuildings)
			{
				for (var i = 0; i < building.MinCountInCity; i++)
				{
					buildings.Add((Building)Activator.CreateInstance(building.GetType(), new object[] { this }));
				}
			}

			while (buildings.Count<allRooms.Count)
			{
				foreach (var building in abuildings)
				{
					var count = buildings.Count(_building => _building.GetType()==building.GetType());
					if(count<building.MaxCountInCity)
					{
						buildings.Add((Building)Activator.CreateInstance(building.GetType(), new object[] { this }));
					}
				}
			}

			buildings = buildings.OrderBy(_building => _rnd.Next()).ToList();

			foreach (var building in buildings)
			{
				var pair = allRooms.First(_pair => building.IsFit(_pair.Key));
				allRooms.Remove(pair.Key);
				building.SetRoom(pair.Key);
				m_buildings.Add(building, pair.Value);
			}

			Debug.WriteLine(m_buildings.Count);
		}

		public Surface Surface { get; private set; }

		public void GenerateCityBlock(MapBlock _block, Random _rnd)
		{
			foreach (var pair in m_buildings.Where(_pair => _pair.Value == _block.BlockId))
			{
				_block.AddRoom(pair.Key.Room);
				pair.Key.Fill(_block);

				_block.AddCreature(new Citizen(Surface, _rnd), pair.Key.Room.RoomRectangle.Center);
			}
		}
	}
}
