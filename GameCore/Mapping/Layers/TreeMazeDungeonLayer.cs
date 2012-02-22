using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using GameCore.Misc;
using GameCore.Objects.Furniture;
using Point = GameCore.Misc.Point;

namespace GameCore.Mapping.Layers
{
	class TreeMazeDungeonLayer:DungeonLayer
	{
		private readonly Dictionary<Room, MapBlock> m_notConnectedRooms = new Dictionary<Room, MapBlock>();
		private readonly Dictionary<Point, Room> m_connectionPoints = new Dictionary<Point, Room>();

		private const int MIN_SIZE = 5;
		private const int MIN_ROOM_SIZE = 3;

		public TreeMazeDungeonLayer(WorldLayer _enterFromLayer, Point _enterCoords, Stair _stair) : base(_enterFromLayer, _enterCoords, _stair)
		{
		}

		internal override IEnumerable<ETerrains> DefaultWalls
		{
			get
			{
				yield return ETerrains.GRASS;
			}
		}

		internal override IEnumerable<ETerrains> DefaultEmptySpaces
		{
			get
			{
				yield return ETerrains.STONE_FLOOR;
			}
		}

		public override FColor Ambient
		{
			get
			{
				return FColor.White;
			}
		}

		protected override MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			
			if (Blocks.Count == 0)
			{
				GenerateInternal(block, new[] { MapBlock.GetInBlockCoords(EnterCoords) });

				//Первая комната всегда присоединена
				block.Rooms.Add(m_notConnectedRooms.Keys.Single(_room => _room.RoomRectangle.ContainsEx(MapBlock.GetInBlockCoords(EnterCoords))));
			}
			else
			{
				GenerateInternal(block);
			}

			ConnectRooms(block);

			return block;
		}

		private void GenerateInternal(MapBlock _block, params Point[] _objects)
		{
			var rnd = new Random(_block.RandomSeed);
			MapBlockHelper.Clear(_block, rnd, this, new[] { ETerrains.SWAMP, });
			MapBlockHelper.Fill(_block, rnd, this, DefaultWalls, new Rectangle(0, 0, MapBlock.SIZE - 1, MapBlock.SIZE - 1));
			var objects = new List<Point>(_objects);

			Generate(_block, rnd, new Rectangle(0, 0, MapBlock.SIZE - 1, MapBlock.SIZE - 1), objects);

			if(objects.Any())
			{
				
			}
		}

		private void Generate(MapBlock _block, Random _random, Rectangle _rectangle, List<Point> _objects)
		{
			var ableVert = _rectangle.Width - MIN_SIZE * 2;
			var ableHor = _rectangle.Height - MIN_SIZE * 2;

			if ((ableHor > 1 || ableVert > 1) && _random.Next(_rectangle.Width * _rectangle.Height) > MIN_SIZE)
			{
				var divVert = 0;
				var divHor = 0;
				while (divVert == divHor)
				{
					divVert = ableVert > 0 ? _random.Next(ableVert + 1) : 0;
					divHor = ableHor > 0 ? _random.Next(ableHor + 1) : 0;
				}
				var rects = new List<Rectangle>();
				if (divVert > divHor)
				{
					int vert;
					do
					{
						vert = MIN_SIZE + _random.Next(ableVert);
						var val = vert;
						if (_objects.All(_point => _point.X != (_rectangle.Left + val))) break;
					} while (true);
					rects.Add(new Rectangle(_rectangle.Left, _rectangle.Top, vert, _rectangle.Height));
					rects.Add(new Rectangle(_rectangle.Left + vert + 1, _rectangle.Top, _rectangle.Width - (vert + 1),_rectangle.Height));
				}
				else
				{
					int hor;
					do
					{
						hor = MIN_SIZE + _random.Next(ableHor);
						var val = hor;
						if (_objects.All(_point => _point.Y != (_rectangle.Top + val))) break;
					} while (true);
					rects.Add(new Rectangle(_rectangle.Left, _rectangle.Top, _rectangle.Width, hor));
					rects.Add(new Rectangle(_rectangle.Left, _rectangle.Top + hor + 1, _rectangle.Width, _rectangle.Height - (hor + 1)));
				}
				foreach (var rectangle in rects)
				{
					if(rectangle.Width>_rectangle.Width || rectangle.Height>_rectangle.Height)
					{
						throw new ApplicationException("Доля больше чем место под нее");
					}
					Generate(_block, _random, rectangle, _objects);
				}
				return;
			}
			MakeRoom(_block, _rectangle, _random, _objects);
		}

		private void MakeRoom(MapBlock _block, Rectangle _rectangle, Random _random, ICollection<Point> _objects)
		{
			MapBlockHelper.Fill(_block, _random, this, new[] { ETerrains.GROUND, }, _rectangle);
			var contains = _objects.Where(_point => _rectangle.ContainsEx(_point)).ToArray();
			var size = new Point(MIN_ROOM_SIZE + _random.Next(_rectangle.Width - MIN_ROOM_SIZE), MIN_ROOM_SIZE + _random.Next(_rectangle.Height - MIN_ROOM_SIZE));
			Point xy;
			Rectangle rect;
			for (; ; )
			{
				xy = new Point(_random.Next(_rectangle.Width - size.X + 1), _random.Next(_rectangle.Height - size.Y + 1));
				rect = new Rectangle(_rectangle.X + xy.X, _rectangle.Y + xy.Y, size.X, size.Y);
				if(!contains.Any()) break;
				if (contains.All(_point => rect.ContainsEx(_point)))
				{
					break;
				}
			}
			MapBlockHelper.Fill(_block, _random, this, DefaultEmptySpaces, rect);
			m_notConnectedRooms.Add(new Room(rect, _rectangle), _block);
			foreach (var contain in contains)
			{
				_objects.Remove(contain);
			}
		}

		/// <summary>
		/// каждая несоединенная комната должна быть либо присоединена к присоединенной комнате, либо иметь точку связи на границе блока
		/// </summary>
		private void ConnectRooms(MapBlock _block)
		{
			var rnd = new Random(_block.RandomSeed);
			foreach (var pair in m_notConnectedRooms)
			{
				var room = pair.Key;
				var block = pair.Value;

				EDirections dirs;
				do
				{
					dirs = (EDirections)rnd.Next(16);
				} while (dirs==EDirections.NONE);

				foreach (EDirections dir in Enum.GetValues(typeof(EDirections)))
				{
					if(dir==EDirections.NONE || !dirs.HasFlag(dir)) continue;

					int val;
					var delta = Point.Zero;
					var begin = Point.Zero;
					switch (dir)
					{
						case EDirections.UP:
							val = room.RoomRectangle.Left + rnd.Next(room.RoomRectangle.Width);
							delta = new Point(0,-1);
							begin = new Point(val, room.RoomRectangle.Top);
							break;
						case EDirections.DOWN:
							val = room.RoomRectangle.Left + rnd.Next(room.RoomRectangle.Width);
							delta = new Point(0,1);
							begin = new Point(val, room.RoomRectangle.Bottom - 1);
							break;
						case EDirections.LEFT:
							val = room.RoomRectangle.Top + rnd.Next(room.RoomRectangle.Height);
							delta = new Point(-1,0);
							begin = new Point(room.RoomRectangle.Left, val);
							break;
						case EDirections.RIGHT:
							val = room.RoomRectangle.Top + rnd.Next(room.RoomRectangle.Height);
							delta = new Point(1,0);
							begin = new Point(room.RoomRectangle.Right-1, val);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					while (room.AreaRectangle.ContainsEx(begin + delta))
					{
						if (!MapBlock.Rect.ContainsEx(begin + delta))
						{
							break;
						}
						begin += delta;
						if(room.RoomRectangle.ContainsEx(begin))
						{
							continue;
						}
						block.Map[begin.X, begin.Y] = ETerrains.WATER;
					}
					if (!room.RoomRectangle.ContainsEx(begin))
					{
						block.Map[begin.X, begin.Y] = ETerrains.SWAMP;
					}
					else
					{
						
					}

					var pnt = block.BlockId * MapBlock.SIZE + begin;
					if (m_connectionPoints.ContainsKey(pnt))
					{
						m_connectionPoints.Remove(pnt);
						block.Map[begin.X, begin.Y] = ETerrains.WATER;
					}
					else
					{
						m_connectionPoints.Add(pnt, room);
					}
				}
			}
			m_notConnectedRooms.Clear();
		}

	}
}
