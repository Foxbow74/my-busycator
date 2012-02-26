using System;
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
			_block.Rooms.Add(new Room(rect, _rectangle, _block.BlockId));
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
					var begin = Point.Zero;
					switch (dir)
					{
						case EDirections.UP:
							val = room.RoomRectangle.Left + rnd.Next(room.RoomRectangle.Width);
							begin = new Point(val, room.RoomRectangle.Top-1);
							break;
						case EDirections.DOWN:
							val = room.RoomRectangle.Left + rnd.Next(room.RoomRectangle.Width);
							begin = new Point(val, room.RoomRectangle.Bottom);
							break;
						case EDirections.LEFT:
							val = room.RoomRectangle.Top + rnd.Next(room.RoomRectangle.Height);
							begin = new Point(room.RoomRectangle.Left-1, val);
							break;
						case EDirections.RIGHT:
							val = room.RoomRectangle.Top + rnd.Next(room.RoomRectangle.Height);
							begin = new Point(room.RoomRectangle.Right, val);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					var end = begin.Clone();
					var delta = dir.GetDelta();
					do
					{
						end += delta;
						if (!room.AreaRectangle.ContainsEx(end)) break;
					} while (true);

					m_connectionPoints.Add(new ConnectionPoint(begin + _block.BlockId * MapBlock.SIZE, end + _block.BlockId * MapBlock.SIZE, room, dir));
				}
			}
		}

		internal override void CompleteBlock(MapBlock _mapBlock)
		{
			var rect = _mapBlock.Rectangle();
			rect.Inflate(MapBlock.SIZE, MapBlock.SIZE);

			var rooms = _mapBlock.BlockId.NearestPoints.Select(_point => Blocks[_point]).SelectMany(_block => _block.Rooms).ToArray();

			var connectionPoints = m_connectionPoints.Where(_pair => rect.ContainsEx(_pair.End)).ToList();

			Action<IEnumerable<ConnectionPoint>> actRemove = delegate(IEnumerable<ConnectionPoint> _points)
			{
				foreach (var connectionPoint in _points)
				{
					Debug.WriteLine(connectionPoint + " deleted");
					connectionPoints.Remove(connectionPoint);
					//m_connectionPoints.Remove(connectionPoint);
				}
			};

			actRemove(connectionPoints.Where(_point => _point.Room.ConnectedTo.Count > 2).ToArray());

			var forbid = new List<Point>();

			{
				foreach (var room in rooms)
				{
					var rrect = room.RoomRectangle;

					var blockId = room.BlockId;

					rrect.Offset(blockId.X * MapBlock.SIZE, blockId.Y * MapBlock.SIZE);
					rrect.Inflate(1, 1);
					forbid.AddRange(rrect.AllPoints().Except(forbid));
				}
			}

			rect = _mapBlock.Rectangle();
			rect.Inflate(MapBlock.SIZE / 2, MapBlock.SIZE / 2);

			actRemove(connectionPoints.Where(_point => _point.Room.ConnectedTo.Count > 2).ToArray());

			if (true)
			{
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

				actRemove(connectionPoints.Where(_point => _point.Room.ConnectedTo.Count > 1).ToArray());

				#endregion
			}

			if (true)
			{
				#region концевая точка касается другой комнаты

				{
					var toRemove = new List<ConnectionPoint>();

					foreach (var connectionPoint in connectionPoints)
					{
						if(connectionPoint.End==new Point(15,18))
						{
							
						}

						if (toRemove.Contains(connectionPoint)) continue;
						foreach (var room in rooms)
						{
							if (room == connectionPoint.Room) continue;
							var rrect = room.RoomRectangle;
							rrect.Offset(room.BlockId.X * MapBlock.SIZE, room.BlockId.Y * MapBlock.SIZE);
							var frect = rrect;
							frect.Inflate(1, 1);
							var end = connectionPoint.End;
							if (frect.ContainsEx(end))
							{
								while (rrect.ContainsEx(end))
								{
									end += connectionPoint.Dir.Opposite().GetDelta();
								}

								if (frect.AllPointsExceptCorners().Contains(end))
								{
									ConnectRooms(room, connectionPoint.Room, end, connectionPoint.Begin);

									toRemove.Add(connectionPoint);
									var revert = m_connectionPoints.Where(_point => _point.Dir == connectionPoint.Dir.Opposite() && _point.Room == room).ToArray();
									toRemove.AddRange(revert);
									m_connectionPoints.Remove(connectionPoint);
									foreach (var point in revert)
									{
										m_connectionPoints.Remove(point);
									}
								}
								else
								{
									//концевая точка примыкает к углу комнаты
									//toRemove.Add(connectionPoint);
								}
							}
						}
					}
					actRemove(toRemove);
				}

				#endregion
			}

			if (true)
			{
				#region Конечные точки в одном столбце

				{
					var toRemove = new List<ConnectionPoint>();
					var sameCol = connectionPoints.GroupBy(_point => _point.End.X).Where(_points => _points.Count() > 1).ToArray();
					foreach (var grouping in sameCol)
					{
						var points = grouping.OrderBy(_point => _point.End.Y).ToArray();

						for (var i = 0; i < points.Length - 1; ++i)
						{
							var flag = true;
							foreach (var point in points[i].End.GetLineToPoints(points[i + 1].End))
							{
								if (forbid.Contains(point))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								ConnectRooms(points[i].Room, points[i + 1].Room, points[i].Begin, points[i].End, points[i + 1].End,
								             points[i + 1].Begin);
								toRemove.Add(points[i]);
								toRemove.Add(points[i + 1]);
								m_connectionPoints.Remove(points[i]);
								m_connectionPoints.Remove(points[i + 1]);
							}
						}
					}
					actRemove(toRemove.Distinct());
				}
				actRemove(connectionPoints.Where(_point => _point.Room.ConnectedTo.Count > 2).ToArray());

				#endregion
			}
			if (true)
			{
				#region Конечные точки на одной строке

				{
					var toRemove = new List<ConnectionPoint>();
					var sameRow = connectionPoints.GroupBy(_point => _point.End.Y).Where(_points => _points.Count() > 1).ToArray();
					foreach (var grouping in sameRow)
					{
						var points = grouping.OrderBy(_point => _point.End.X).ToArray();

						for (var i = 0; i < points.Length - 1; ++i)
						{
							var flag = true;
							foreach (var point in points[i].End.GetLineToPoints(points[i + 1].End))
							{
								if (forbid.Contains(point))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								ConnectRooms(points[i].Room, points[i + 1].Room, points[i].Begin, points[i].End, points[i + 1].End,
								             points[i + 1].Begin);
								toRemove.Add(points[i]);
								toRemove.Add(points[i + 1]);

								m_connectionPoints.Remove(points[i]);
								m_connectionPoints.Remove(points[i + 1]);
							}
						}
					}
					actRemove(toRemove.Distinct());
				}
				actRemove(connectionPoints.Where(_point => _point.Room.ConnectedTo.Count > 2).ToArray());

				#endregion
			}

			foreach (var point in forbid)
			{
				if (!_mapBlock.Rectangle().ContainsEx(point)) continue;
				var inBlock = MapBlock.GetInBlockCoords(point);
				var block = Blocks[MapBlock.GetBlockCoords(point)];

				if (block.Map[inBlock.X, inBlock.Y] == ETerrains.GRASS || block.Map[inBlock.X, inBlock.Y] == ETerrains.GROUND)
				{
					block.Map[inBlock.X, inBlock.Y] = ETerrains.LAVA;
				}
			}

			foreach (var room in _mapBlock.Rooms)
			{
				if (room.ConnectedTo.Any())
				{
					MapBlockHelper.Fill(_mapBlock, new Random(_mapBlock.RandomSeed), this, new[] { ETerrains.STONE_FLOOR, }, room.RoomRectangle);
				}
				else
				{
					MapBlockHelper.Fill(_mapBlock, new Random(_mapBlock.RandomSeed), this, new[] { ETerrains.SWAMP, }, room.RoomRectangle);
				}
			}

			actRemove(connectionPoints.Where(_point => _point.Room.IsConnected || _point.Room.ConnectedTo.Count > 2).ToArray()); 
			foreach (var connectionPoint in connectionPoints)
			{
				foreach (var point in connectionPoint.Begin.GetLineToPoints(connectionPoint.End))
				{
					var inBlock = MapBlock.GetInBlockCoords(point);
					Blocks[MapBlock.GetBlockCoords(point)].Map[inBlock.X, inBlock.Y] = connectionPoint.Dir.GetTerrain();
				}
			}

			{
				var toRemove = m_connectionPoints.Where(_point => _point.Room.IsConnected || _point.Room.ConnectedTo.Count > 2).ToArray();
				foreach (var connectionPoint in toRemove)
				{
					m_connectionPoints.Remove(connectionPoint);
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
