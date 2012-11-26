using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Essences.Things;
using GameCore.Essences.Things.LightSources;
using GameCore.Materials;
using GameCore.Misc;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	public abstract class Building
	{
		private readonly City m_city;

		protected Building(City _city) { m_city = _city; }

		public ESex Sex { get; protected set; }

		public Point BlockId { get { return Room.BlockId; } }

		public Point DoorWorldCoords { get; private set; }

		/// <summary>
		/// 	координаты внешних точек, откуда можно открыть дверь, крыльцо
		/// </summary>
		public Point[] OutDoorWorldCoords { get; protected set; }

		/// <summary>
		/// 	координаты внутренних точек, откуда можно открыть дверь, прихожая
		/// </summary>
		public Point[] InDoorWorldCoords { get; protected set; }

		public virtual uint MinCountInCity { get { return 1; } }
		public virtual uint MaxCountInCity { get { return uint.MaxValue; } }

		private static IEnumerable<Tuple<ETerrains, ETerrains>> Walls
		{
			get
			{
				yield return new Tuple<ETerrains, ETerrains>(ETerrains.GRAY_BRICK_WALL, ETerrains.GRAY_BRICK_WINDOW);
				yield return new Tuple<ETerrains, ETerrains>(ETerrains.RED_BRICK_WALL, ETerrains.RED_BRICK_WINDOW);
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

		public Intelligent Owner { get; private set; }

		//public string this[EPadej _padej, bool _withOwner] { get { return Sklonenia.ToPadej(_padej, BuildingName, false, Sex) + (_withOwner ? (" " + Sklonenia.ToPadej(EPadej.ROD, Owner.IntelligentName, true, Owner.Sex)) : ""); } }

		protected abstract string BuildingName { get; }

		public virtual void Fill(MapBlock _block, WorldLayer _layer)
		{
			var mapBlock = _block;
			var roomRectangle = Room.RoomRectangle;

			var wall = Walls.ToArray().RandomItem(World.Rnd);
			var floor = Floors.ToArray().RandomItem(World.Rnd);

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
					if (i++%3 == 0)
					{
						mapBlock.Map[point.X, point.Y] = wall.Item2;
					}
					else
					{
						mapBlock.Map[point.X, point.Y] = wall.Item1;
					}
				}
			}

			CreateDoor(mapBlock, floor, borderPoints);

			mapBlock.AddEssence(new IndoorLight(new LightSource(10, new FColor(3f, 1f, 1f, 0.5f)), EssenceHelper.GetFirstFoundedMaterial<MetalMaterial>()), roomRectangle.Inflate(-1, -1).Random(World.Rnd));
		}

		private void CreateDoor(MapBlock _mapBlock, ETerrains _floor, IEnumerable<Point> _borderPoints)
		{
			var doorCoords = Room.RoomRectangle.Center;
			var prevPoint = doorCoords;
			var direction = World.Rnd.GetRandomDirection();
			var delta = direction.GetDelta();
			while (!_borderPoints.Contains(doorCoords))
			{
				prevPoint = doorCoords;
				doorCoords += delta;
			}
			var nextPoint = doorCoords + delta;

			var borders = direction.GetBorders().ToArray();

			DoorWorldCoords = _mapBlock.ToWorldCoords(doorCoords);

			OutDoorWorldCoords = new[] {nextPoint + borders[0].Key, nextPoint, nextPoint + borders[1].Key}.Select(_mapBlock.ToWorldCoords).ToArray();
			InDoorWorldCoords = new[] {prevPoint + borders[0].Key, prevPoint, prevPoint + borders[1].Key}.Select(_mapBlock.ToWorldCoords).ToArray();

			_mapBlock.Map[doorCoords.X, doorCoords.Y] = _floor;
            var doors = EssenceHelper.GetAllThings<ClosedDoor>().ToArray();
			_mapBlock.AddEssence(doors[0], doorCoords);
		}

		public virtual bool IsFit(Room _room) { return true; }
		public void SetRoom(Room _room) { Room = _room; }

		public void SetOwner(Intelligent _intelligent) { Owner = _intelligent; }
	}
}