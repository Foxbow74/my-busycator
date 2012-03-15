using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.CreatureRoles;
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

			var availableBuildings = Util.GetAllTypesOf<Building>().Select(_type => (Building)Activator.CreateInstance(_type, new object[] { this })).ToArray();
			var availableRoles = Util.GetAllTypesOf<AbstractCitizenRole>().Select(_type => (AbstractCitizenRole)Activator.CreateInstance(_type)).ToArray();

			var buildings = new List<Building>();

			foreach (var abuilding in availableBuildings)
			{
				for (var i = 0; i < abuilding.MinCountInCity; i++)
				{
					var building = (Building)Activator.CreateInstance(abuilding.GetType(), new object[] { this });

					var role = availableRoles.FirstOrDefault(_role => _role.BuildingType == building.BuildingType);
					if(role!=null)
					{
						var citizen = new Citizen(Surface, _rnd);
						var citizenRole = (AbstractCitizenRole)Activator.CreateInstance(role.GetType());
						citizenRole.SetBuilding(citizen, building);
						citizen.AddRole(citizenRole);
						m_citizens.Add(citizen);
					}

					buildings.Add(building);
				}
			}

			while (buildings.Count<allRooms.Count)
			{
				foreach (var abuilding in availableBuildings)
				{
					var count = buildings.Count(_building => _building.GetType()==abuilding.GetType());
					if (count >= abuilding.MaxCountInCity) continue;

					var building = (Building)Activator.CreateInstance(abuilding.GetType(), new object[] { this });
					var role = availableRoles.FirstOrDefault(_role => _role.BuildingType == building.BuildingType);
					if (role != null)
					{
						Citizen citizen = null;
						if(building.BuildingType==EBuilding.HOUSE)
						{
							citizen = m_citizens.FirstOrDefault(_citizen => _citizen.Roles.OfType<AbstractCitizenRole>().All(_role => _role.BuildingType != EBuilding.HOUSE));
						}
						if(citizen==null)
						{
							citizen = new Citizen(Surface, _rnd);
							m_citizens.Add(citizen);
						}
						var citizenRole = (AbstractCitizenRole)Activator.CreateInstance(role.GetType());
						citizenRole.SetBuilding(citizen, building);
						citizen.AddRole(citizenRole);
					}
					buildings.Add(building);
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
		}

		public Surface Surface { get; private set; }

		readonly List<Citizen> m_already = new List<Citizen>();

		public void GenerateCityBlock(MapBlock _block, Random _rnd)
		{
			foreach (var pair in m_buildings.Where(_pair => _pair.Value == _block.BlockId))
			{
				_block.AddRoom(pair.Key.Room);
				pair.Key.Fill(_block);
				var citizens = m_citizens.Where(_citizen => _citizen.Roles.OfType<AbstractCitizenRole>().First().Building == pair.Key).ToArray();
				foreach (var citizen in citizens)
				{
					if (m_already.Contains(citizen))
					{
						
					}
					Debug.WriteLine(citizen);
					m_already.Add(citizen);
					_block.AddCreature(citizen, pair.Key.Room.RoomRectangle.Center);	
				}
			}
		}
	}
}
