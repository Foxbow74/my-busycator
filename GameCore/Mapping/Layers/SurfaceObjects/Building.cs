using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Materials;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture.LightSources;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	abstract class Building
	{
		private readonly City m_city;

		protected Building(City _city)
		{
			m_city = _city;
			Debug.WriteLine(BuildingType + " построен");
		}
		
		public virtual void Fill(MapBlock _block)
		{
			var mapBlock = _block;
			var rnd = new Random(mapBlock.RandomSeed);
			var roomRectangle = Room.RoomRectangle;

			var wall = Walls.ToArray().RandomItem(rnd);
			var floor = Floors.ToArray().RandomItem(rnd);

			foreach (var point in roomRectangle.AllPoints)
			{
				mapBlock.Map[point.X, point.Y] = floor;
			}

			var borderPoints = roomRectangle.BorderPoints;
			var cornerPoints = roomRectangle.CornerPoints;
			var i = 0;
			foreach (var point in borderPoints)
			{
				if (cornerPoints.Contains(point))
				{
					mapBlock.Map[point.X, point.Y] = wall.Item1;
				}
				else
				{
					if (i++ % 3 == 0)
					{
						mapBlock.Map[point.X, point.Y] = wall.Item2;
					}
					else
					{
						mapBlock.Map[point.X, point.Y] = wall.Item1;
					}
				}
			}

			var doorCoords = roomRectangle.Center;
			var delta = rnd.GetRandomDirection().GetDelta();
			while (!borderPoints.Contains(doorCoords))
			{
				doorCoords += delta;
			}

			mapBlock.Map[doorCoords.X, doorCoords.Y] = floor;
			var fakedFurniture = ETiles.DOOR.GetThing(ThingHelper.GetMaterial<OakMaterial>());
			var door = fakedFurniture.ResolveFake(World.TheWorld.Avatar);
			mapBlock.AddObject(door, doorCoords);

			mapBlock.AddObject(new IndoorLight(new LightSource(10, new FColor(3f, 1f, 1f, 0.5f)), ThingHelper.GetMaterial<BrassMaterial>()), roomRectangle.Center);
		}

		public virtual bool IsFit(Room _room)
		{
			return true;
		}

		public virtual uint MinCountInCity { get { return 1; } }
		public virtual uint MaxCountInCity { get { return uint.MaxValue; } }

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
				yield return ETerrains.WOOD_FLOOR_MAPPLE;
				yield return ETerrains.WOOD_FLOOR_OAK;
			}
		}

		public abstract EBuilding BuildingType { get; }

		public Room Room { get; private set; }

		public void SetRoom(Room _room)
		{
			Room = _room;
		}
	}
}
