﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Drawing;
using GameCore.Misc;
using GameCore.Objects.Furniture;
using Point = GameCore.Misc.Point;

namespace GameCore.Mapping.Layers
{
	class TreeMazeDungeonLayer : DungeonLayer
	{
		//private readonly Dictionary<Room, MapBlock> m_notConnectedRooms = new Dictionary<Room, MapBlock>();
		private readonly List<ConnectionPoint> m_connectionPoints = new List<ConnectionPoint>();

		private const int MIN_SIZE = 5;
		private const int MIN_ROOM_SIZE = 3;

		public TreeMazeDungeonLayer(WorldLayer _enterFromLayer, Point _enterCoords, Stair _stair)
			: base(_enterFromLayer, _enterCoords, _stair)
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
				//block.Rooms.Add(m_notConnectedRooms.Keys.Single(_room => _room.RoomRectangle.ContainsEx(MapBlock.GetInBlockCoords(EnterCoords))));
			}
			else
			{
				GenerateInternal(block);
			}

			Blocks[block.BlockId] = block;
			AddConnectionPoints(block);
			return block;
		}

		private void GenerateInternal(MapBlock _block, params Point[] _objects)
		{
			var rnd = new Random(_block.RandomSeed);
			MapBlockHelper.Clear(_block, rnd, this, new[] { ETerrains.GROUND, });
			MapBlockHelper.Fill(_block, rnd, this, DefaultWalls, new Rectangle(0, 0, MapBlock.SIZE - 1, MapBlock.SIZE - 1));
			var objects = new List<Point>(_objects);

			Generate(_block, rnd, new Rectangle(0, 0, MapBlock.SIZE - 1, MapBlock.SIZE - 1), objects);

			if (objects.Any())
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
					rects.Add(new Rectangle(_rectangle.Left + vert + 1, _rectangle.Top, _rectangle.Width - (vert + 1), _rectangle.Height));
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
					if (rectangle.Width > _rectangle.Width || rectangle.Height > _rectangle.Height)
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
				if (!contains.Any()) break;
				if (contains.All(_point => rect.ContainsEx(_point)))
				{
					break;
				}
			}
			MapBlockHelper.Fill(_block, _random, this, DefaultEmptySpaces, rect);
			_block.Rooms.Add(new Room(rect, _rectangle));
			foreach (var contain in contains)
			{
				_objects.Remove(contain);
			}
		}

		/// <summary>
		/// каждая несоединенная комната должна быть либо присоединена к присоединенной комнате, либо иметь точку связи на границе блока
		/// </summary>
		private void AddConnectionPoints(MapBlock _block)
		{
			var rnd = new Random(_block.RandomSeed);
			foreach (var room in _block.Rooms)
			{
				if (_block.BlockId == MapBlock.GetBlockCoords(EnterCoords) && room.RoomRectangle.ContainsEx(MapBlock.GetInBlockCoords(EnterCoords)))
				{
					room.IsConnected = true;
				}


				EDirections dirs;
				do
				{
					dirs = (EDirections)rnd.Next(16);
				} while (dirs == EDirections.NONE);

				foreach (EDirections dir in Enum.GetValues(typeof(EDirections)))
				{
					if (dir == EDirections.NONE || !dirs.HasFlag(dir)) continue;

					int val;
					var delta = Point.Zero;
					var begin = Point.Zero;
					switch (dir)
					{
						case EDirections.UP:
							val = room.RoomRectangle.Left + rnd.Next(room.RoomRectangle.Width);
							delta = new Point(0, -1);
							begin = new Point(val, room.RoomRectangle.Top-1);
							break;
						case EDirections.DOWN:
							val = room.RoomRectangle.Left + rnd.Next(room.RoomRectangle.Width);
							delta = new Point(0, 1);
							begin = new Point(val, room.RoomRectangle.Bottom);
							break;
						case EDirections.LEFT:
							val = room.RoomRectangle.Top + rnd.Next(room.RoomRectangle.Height);
							delta = new Point(-1, 0);
							begin = new Point(room.RoomRectangle.Left-1, val);
							break;
						case EDirections.RIGHT:
							val = room.RoomRectangle.Top + rnd.Next(room.RoomRectangle.Height);
							delta = new Point(1, 0);
							begin = new Point(room.RoomRectangle.Right, val);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					var end = begin.Clone();

					do
					{
						end += delta;
						if (!room.AreaRectangle.ContainsEx(end)) break;
					} while (true);

					m_connectionPoints.Add(new ConnectionPoint(begin + _block.BlockId * MapBlock.SIZE, end + _block.BlockId * MapBlock.SIZE, room, dir));
				}
			}
			//m_notConnectedRooms.Clear();

			//var points = m_connectionPoints.Keys.ToDictionary(_pnt => _pnt, _pnt => m_connectionPoints.Where(_pair => _pair.Key!=_pnt && (_pair.Key - _pnt).QLenght < 10).OrderBy(_pair => (_pair.Key - _pnt).QLenght).ToList());
			//var keys = points.Keys.ToList();
			//foreach (var pair in points)
			//{
			//    var pnt = pair.Key;
			//    foreach (var npair in pair.Value)
			//    {
			//        if (m_connectionPoints[pnt].ConnectedTo.ContainsKey(npair.Value)) continue;

			//        var npnt = npair.Key;

			//        if (!Blocks.ContainsKey(MapBlock.GetBlockCoords(npnt)) || !Blocks.ContainsKey(MapBlock.GetBlockCoords(pnt))) continue;

			//        var nblock = Blocks[MapBlock.GetBlockCoords(npnt)];
			//        var block = Blocks[MapBlock.GetBlockCoords(pnt)];

			//        var near = pnt.X == npnt.X && Math.Abs(pnt.Y - npnt.Y) == 1;
			//        near = near || (pnt.Y == npnt.Y && Math.Abs(pnt.X - npnt.X) == 1);
			//        if (near)
			//        {
			//            npair.Value.Connect(m_connectionPoints[pnt], pnt);
			//            var innpnt = MapBlock.GetInBlockCoords(npnt);
			//            var inpnt = MapBlock.GetInBlockCoords(pnt);

			//            nblock.Map[innpnt.X, innpnt.Y] = ETerrains.WATER;
			//            block.Map[inpnt.X, inpnt.Y] = ETerrains.WATER;
			//        }
			//    }

			//}
		}

		internal override void CompleteBlock(MapBlock _mapBlock)
		{
			var rect = _mapBlock.Rectangle();
			rect.Inflate(1, 1);

			var offset = _mapBlock.BlockId * MapBlock.SIZE;

			var connectionPoints = m_connectionPoints.Where(_pair => rect.ContainsEx(_pair.End)).ToList();

			var forbid = new List<Point>();

			{
				var toRemove = new List<ConnectionPoint>();

				foreach (var room in _mapBlock.Rooms)
				{
					var rrect = room.RoomRectangle;
					rrect.Offset(offset.X, offset.Y);
					rrect.Inflate(1, 1);
					forbid.AddRange(rrect.AllPoints().Except(forbid));
				}
				foreach (var connectionPoint in connectionPoints)
				{
					//if (forbid.Contains(connectionPoint.End))
					//{
					//    toRemove.Add(connectionPoint);
					//}
					//else
					{
						foreach (var point in connectionPoint.Begin.GetLineToPoints(connectionPoint.End))
						{
							forbid.Remove(point);
						}
					}
				}

				foreach (var point in forbid)
				{
					var inBlock = MapBlock.GetInBlockCoords(point);
					if (Blocks[MapBlock.GetBlockCoords(point)].Map[inBlock.X, inBlock.Y] != ETerrains.STONE_FLOOR)
					{
						Blocks[MapBlock.GetBlockCoords(point)].Map[inBlock.X, inBlock.Y] = ETerrains.LAVA;
					}
				}
			}

			#region концевая точка касается другой комнаты

			{
				var toRemove = new List<ConnectionPoint>();

				foreach (var connectionPoint in connectionPoints)
				{
					if (toRemove.Contains(connectionPoint)) continue;
					foreach (var room in _mapBlock.Rooms)
					{
						if (room == connectionPoint.Room) continue;
						var rrect = room.RoomRectangle;
						rrect.Offset(offset.X, offset.Y);
						rrect.Inflate(1, 1);
						if (rrect.ContainsEx(connectionPoint.End))
						{
							if (rrect.AllPointsExceptCorners().Contains(connectionPoint.End))
							{
								ConnectRooms(room, connectionPoint.Room, connectionPoint.End, connectionPoint.Begin);
								toRemove.Add(connectionPoint);
								var revert = connectionPoints.Where(_point => _point.Dir == connectionPoint.Dir.Opposite() && _point.Room == room);
								toRemove.AddRange(revert);
								foreach (var point in revert)
								{
									Debug.WriteLine("MUSHROOM: " + point);
									var inBlock = MapBlock.GetInBlockCoords(point.End);
									Blocks[MapBlock.GetBlockCoords(point.End)].Map[inBlock.X, inBlock.Y] = ETerrains.MUSHROOM;

									inBlock = MapBlock.GetInBlockCoords(point.Begin);
									Blocks[MapBlock.GetBlockCoords(point.Begin)].Map[inBlock.X, inBlock.Y] = ETerrains.MUSHROOM;
								}
							}
							else
							{
								//концевая точка примыкает к углу комнаты
								//toRemove.Add(connectionPoint);
							}
						}
						Debug.WriteLine("--------------");
					}
				}

				foreach (var connectionPoint in toRemove)
				{
					connectionPoints.Remove(connectionPoint);
					m_connectionPoints.Remove(connectionPoint);
					Debug.WriteLine(connectionPoint + " deleted");
				}
			}

			#endregion

			#region конечные точки совпадают

			var sameEndPoints = connectionPoints.GroupBy(_point => _point.End).Where(_points => _points.Count() > 1).ToArray();
			foreach (var grouping in sameEndPoints)
			{
				var points = grouping.ToArray();
				if (points.Length > 2)
				{
					throw new NotImplementedException("Как может сойтись в одной точке более двух комнат????");
				}
				ConnectRooms(points[0].Room, points[1].Room, points[0].Begin, points[0].End, points[1].Begin);
				foreach (var point in grouping)
				{
					m_connectionPoints.Remove(point);
					connectionPoints.Remove(point);
					Debug.WriteLine(point + " deleted");
				}
			}

			#endregion

			#region Конечные точки на одной строке

			//var sameRow = connectionPoints.GroupBy(_point => _point.End.Y).Where(_points => _points.Count() > 1).ToArray();
			//foreach (var grouping in sameRow)
			//{
			//    var points = grouping.ToArray();
			//    if (points.Length > 2)
			//    {
			//        //throw new NotImplementedException("Как может сойтись в одной точке более двух комнат????");
			//    }
			//    ConnectRooms(points[0].Room, points[1].Room, points[0].Begin, points[0].End, points[1].End, points[1].End);
			//    foreach (var point in grouping)
			//    {
			//        m_connectionPoints.Remove(point);
			//        connectionPoints.Remove(point);
			//        Debug.WriteLine(point + " deleted");
			//    }
			//}

			#endregion

			foreach (var connectionPoint in connectionPoints)
			{
				foreach (var point in connectionPoint.Begin.GetLineToPoints(connectionPoint.End))
				{
					var inBlock = MapBlock.GetInBlockCoords(point);
					Blocks[MapBlock.GetBlockCoords(point)].Map[inBlock.X, inBlock.Y] = ETerrains.SWAMP;
				}
			}

			base.CompleteBlock(_mapBlock);
		}

		private void ConnectRooms(Room _room1, Room _room2, params Point[] _points)
		{
			var pnt = _points[0];
			for(var i=1;i<_points.Length;++i)
			{
				foreach (var point in pnt.GetLineToPoints(_points[i]))
				{
					var inBlock = MapBlock.GetInBlockCoords(point);
					Blocks[MapBlock.GetBlockCoords(point)].Map[inBlock.X, inBlock.Y] = ETerrains.WATER;
				}
				pnt = _points[i];
			}
			_room1.Connect(_room2, _points);

			Debug.WriteLine("CONNECTED:" + _room1.RoomRectangle + " to " + _room2.RoomRectangle + " by " + string.Join("|",  _points.Select(_point => _point.ToString())));
		}
	}
}
