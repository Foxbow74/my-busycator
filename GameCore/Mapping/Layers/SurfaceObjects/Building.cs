using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Materials;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture.LightSources;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	class Building:Room
	{
		public Building(Rct _roomRect, Rct _areaRect, MapBlock _mapBlock, WorldLayer _layer, Random _rnd)
			: base(_roomRect, _areaRect, _mapBlock, _layer)
		{
			var wall = Walls.ToArray().RandomItem(_rnd);
			var floor = Floors.ToArray().RandomItem(_rnd);

			foreach (var point in RoomRectangle.AllPoints)
			{
				_mapBlock.Map[point.X, point.Y] = floor;
			}

			var borderPoints = RoomRectangle.BorderPoints;
			var cornerPoints = RoomRectangle.CornerPoints;
			var i = 0;
			foreach (var point in borderPoints)
			{
				if(cornerPoints.Contains(point))
				{
					_mapBlock.Map[point.X, point.Y] = wall.Item1;
				}
				else
				{
					if(i++%3==0)
					{
						_mapBlock.Map[point.X, point.Y] = wall.Item2;
					}
					else
					{
						_mapBlock.Map[point.X, point.Y] = wall.Item1;
					}
				}
			}

			var doorCoords = RoomRectangle.Center;
			var delta = _rnd.GetRandomDirection().GetDelta();
			while (!borderPoints.Contains(doorCoords))
			{
				doorCoords += delta;
			}

			_mapBlock.Map[doorCoords.X, doorCoords.Y] = floor;
			var fakedFurniture = ETiles.DOOR.GetThing(ThingHelper.GetMaterial<OakMaterial>());
			var door = fakedFurniture.ResolveFake(World.TheWorld.Avatar);
			_mapBlock.AddObject(door, doorCoords);

			_mapBlock.AddObject(new IndoorLight(new LightSource(10, new FColor(3f, 1f, 1f, 0.5f)), ThingHelper.GetMaterial<BrassMaterial>()), RoomRectangle.Center);
		}

		private static IEnumerable<Tuple<ETerrains,ETerrains>> Walls
		{
			get
			{
				yield return new Tuple<ETerrains, ETerrains>(ETerrains.GRAY_BRICK_WALL, ETerrains.GRAY_BRICK_WINDOW);
				yield return new Tuple<ETerrains, ETerrains>(ETerrains.RED_BRICK_WALL,ETerrains.RED_BRICK_WINDOW);
				yield return new Tuple<ETerrains, ETerrains>(ETerrains.YELLOW_BRICK_WALL, ETerrains.YELLOW_BRICK_WINDOW);
			}
		}

		private static IEnumerable<ETerrains> Floors
		{
			get
			{
				yield return ETerrains.STONE_FLOOR;
			}
		}
	}
}
