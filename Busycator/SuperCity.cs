﻿using System;
using System.Collections.Generic;
using System.Linq;
using Busycator.Creatures;
using Busycator.Layers;
using GameCore;
using GameCore.CreatureRoles;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Mapping.Layers.SurfaceObjects;
using GameCore.Misc;

namespace Busycator
{
    public class SuperCity:City
    {
        private const int MAX_CITY_BUILDINGS_COUNT = 10;
        private readonly List<Citizen> m_already = new List<Citizen>();
        private readonly List<Citizen> m_citizens = new List<Citizen>();
        private readonly List<Tuple<ETileset, FColor>> m_conf = new List<Tuple<ETileset, FColor>>();

        public SuperCity(WorldLayer _layer, params Point[] _cityBlockIds):base(_layer, _cityBlockIds)
        {
            if (Constants.WORLD_MAP_SIZE >= 32)
            {
                AddBuildings();
            }
        }


        private void AddBuildings()
        {
            var allRooms = new Dictionary<Room, Point>();
            var needRooms = CityBlockIds.Length * 3;
            foreach (var blockId in CityBlockIds)
            {
                var rooms = LayerHelper.GenerateRooms(World.Rnd, BaseMapBlock.Rect.Inflate(-2, -2).Offset(2, 2), new Point[0], blockId).OrderByDescending(_room => _room.RoomRectangle.Size);
                foreach (var room in rooms)
                {
                    allRooms.Add(room, blockId);
                    //if (allRooms.Count >= needRooms)
                    //{
                    //    break;
                    //}
                }
            }

            var availableBuildings = Util.GetAllTypesOf<Building>().Select(_type => (Building)Activator.CreateInstance(_type, this)).ToArray();
            var availableRoles = Util.GetAllTypesOf<AbstractCitizenRole>().Select(_type => (AbstractCitizenRole)Activator.CreateInstance(_type)).ToArray();

            var buildings = new List<Building>();

            var surface = Layer as Surface;
            if (surface == null)
            {
                throw new NotImplementedException();
            }

            foreach (var abuilding in availableBuildings)
            {
                for (var i = 0; i < abuilding.MinCountInCity; i++)
                {
                    var building = (Building)Activator.CreateInstance(abuilding.GetType(), this);

                    var role = availableRoles.FirstOrDefault(_role => _role.BuildingType == building.BuildingType);
                    if (role != null)
                    {
                        var citizen = new Citizen(Layer, World.Rnd, surface.GetNextCitizenName(new[] { ESex.MALE, ESex.FEMALE, }.RandomItem(World.Rnd)));
                        var citizenRole = (AbstractCitizenRole)Activator.CreateInstance(role.GetType());
                        citizenRole.SetBuilding(citizen, building);
                        citizen.AddRole(citizenRole);
                        m_citizens.Add(citizen);
                    }

                    buildings.Add(building);
                }
            }

            while (buildings.Count < allRooms.Count)
            {
                foreach (var abuilding in availableBuildings)
                {
                    var count = buildings.Count(_building => _building.GetType() == abuilding.GetType());
                    if (count >= abuilding.MaxCountInCity) continue;

                    var building = (Building)Activator.CreateInstance(abuilding.GetType(), this);
                    var role = availableRoles.FirstOrDefault(_role => _role.BuildingType == building.BuildingType);
                    if (role != null)
                    {
                        Citizen citizen = null;
                        if (building.BuildingType == EBuilding.HOUSE)
                        {
                            citizen = m_citizens.FirstOrDefault(_citizen => _citizen.Roles.OfType<AbstractCitizenRole>().All(_role => _role.BuildingType != EBuilding.HOUSE));
                        }
                        if (citizen == null)
                        {
                            citizen = new Citizen(Layer, World.Rnd, surface.GetNextCitizenName(new[] { ESex.MALE, ESex.FEMALE, }.RandomItem(World.Rnd)));
                            m_citizens.Add(citizen);
                        }
                        var citizenRole = (AbstractCitizenRole)Activator.CreateInstance(role.GetType());
                        citizenRole.SetBuilding(citizen, building);
                        citizen.AddRole(citizenRole);
                    }
                    buildings.Add(building);
                }
            }

            buildings = buildings.OrderBy(_building => World.Rnd.Next()).ToList();

            foreach (var building in buildings)
            {
                var pair = allRooms.First(_pair => building.IsFit(_pair.Key));
                allRooms.Remove(pair.Key);
                building.SetRoom(pair.Key);
                m_buildings.Add(building);
            }
        }

        private readonly List<Building> m_buildings = new List<Building>();

        public IEnumerable<Building> Buildings { get { return m_buildings; } }


        public override void GenerateCityBlock(MapBlock _block, Random _rnd, WorldLayer _layer)
        {
            var roadPoints = new List<Point>();
            for (var i = 0; i < Constants.MAP_BLOCK_SIZE; ++i)
            {
                if (_rnd.Next(2) == 0) roadPoints.Add(new Point(0, i));
                if (_rnd.Next(2) == 0) roadPoints.Add(new Point(1, i));
                if (_rnd.Next(2) == 0) roadPoints.Add(new Point(i, 0));
                if (_rnd.Next(2) == 0) roadPoints.Add(new Point(i, 1));
            }

            if (roadPoints.All(point => TerrainAttribute.GetAttribute(_block.Map[point.X, point.Y]).IsPassable))
            {
                foreach (var point in roadPoints)
                {
                    _block.Map[point.X, point.Y] = ETerrains.ROAD;
                }
            }

            var terrains = m_buildings.Where(_building => _building.BlockId == _block.BlockId).ToDictionary(_b => _b, _building => _building.Room.AreaRectangle.AllPoints.Select(_point => _block.Map[_point.X, _point.Y]).Distinct().ToArray());
            foreach (var pair in terrains)
            {
                if (pair.Value.Any(_terrains => TerrainAttribute.GetAttribute(_terrains).IsNotPassable))
                {
                    m_buildings.Remove(pair.Key);
                }
            }

            var buildings = m_buildings.Where(_pair => _pair.BlockId == _block.BlockId).ToArray();
            foreach (var building in buildings)
            {
                _block.AddRoom(building.Room);
                building.Fill(_block, _layer);
                var citizens = m_citizens.Where(_citizen => _citizen.Roles.OfType<AbstractCitizenRole>().First().Building == building).ToArray();
                foreach (var citizen in citizens)
                {
                    if (m_already.Contains(citizen))
                    {
                        throw new ApplicationException();
                    }
                    m_already.Add(citizen);

                    Tuple<ETileset, FColor> tuple = null;
                    foreach (var color in citizen.Roles.First().Colors)
                    {
                        tuple = Tuple.Create(citizen.Tileset, color);
                        if (!m_conf.Contains(tuple))
                        {
                            break;
                        }
                    }
                    if (tuple == null)
                    {
                        throw new ApplicationException();
                    }

                    m_conf.Add(tuple);
                    citizen.SetLerpColor(tuple.Item2);
                    _block.CreaturesAdd(citizen, building.Room.RoomRectangle.Center);
                }
            }
        }
    }
}