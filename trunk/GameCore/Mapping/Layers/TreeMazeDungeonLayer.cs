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
		const int MAX_PATH_LEN = 20;

		private readonly Random m_rnd;

		public TreeMazeDungeonLayer(WorldLayer _enterFromLayer, Point _enterCoords, Stair _stair, int _rndSeed)
			: base(_enterFromLayer, _enterCoords, _stair)
		{
			m_rnd = new Random(_rndSeed);
			var size = m_rnd.Next(15) + m_rnd.Next(15) + 5;
			var map = new EMapBlockTypes[size, size];
			var center = new Point(size / 2, size / 2);
			var list = new List<Point> { center };
			do
			{
				var point = list[m_rnd.Next(list.Count)];
				list.AddRange(AddBlocks(point, map, ref size));
			} while (size > 0);

			var blockIds = list.Select(_point => _point - center).Distinct().ToArray();

			foreach (var blockId in blockIds)
			{
				MapBlock block;
				if(!Blocks.TryGetValue(blockId, out block))
				{
					block = new MapBlock(blockId);	
				}
				
				if (MapBlock.GetBlockCoords(EnterCoords) == blockId)
				{
					GenerateInternal(block, new[] { MapBlock.GetInBlockCoords(EnterCoords) });
				}
				else
				{
					GenerateInternal(block);
				}
				Blocks[block.BlockId] = block;
			}
			foreach (var block in blockIds.Select(_blockId => Blocks[_blockId]))
			{
				AddConnectionPoints(block);
			}

			LinkRooms();
			//foreach (var block in blockIds.Select(_blockId => Blocks[_blockId]))
			//{
			//    LinkRooms(block);
			//}
		}

		private IEnumerable<Point> AddBlocks(Point _from, EMapBlockTypes[,] _map, ref int _size)
		{
			var list = new List<Point>();
			if (_size == 0 )
			{
				return list;
			}
			if (_map[_from.X, _from.Y] != EMapBlockTypes.GROUND)
			{
				list.Add(_from);
				_map[_from.X, _from.Y] = EMapBlockTypes.GROUND;
				_size--;
				if(m_rnd.NextDouble() < 0.1)
				{
					return list;
				}
			}
			EDirections dirs;
			do
			{
				dirs = (EDirections)m_rnd.Next(16);
			} while (dirs == EDirections.NONE);
			foreach (EDirections dir in Enum.GetValues(typeof(EDirections)))
			{
				if (dir == EDirections.NONE || !dirs.HasFlag(dir)) continue;
				var xy = _from + dir.GetDelta();
				if (_map.GetLength(0) <= xy.X || xy.X < 0) continue;
				if (_map.GetLength(1) <= xy.Y || xy.Y < 0) continue;

				if (_map[xy.X, xy.Y] != EMapBlockTypes.GROUND)
				{
					list.AddRange(AddBlocks(xy, _map, ref _size));
				}
			}
			return list;
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
			var rnd = new Random(block.RandomSeed);
			MapBlockHelper.Clear(block, rnd, this, DefaultWalls);
			return block;
		}

		private void GenerateInternal(MapBlock _block, params Point[] _objects)
		{
			var rnd = new Random(_block.RandomSeed);
			MapBlockHelper.Clear(_block, rnd, this, new[] { ETerrains.GROUND, });
			MapBlockHelper.Fill(_block, rnd, this, DefaultWalls, new Rectangle(0, 0, MapBlock.SIZE - 1, MapBlock.SIZE - 1));
			var objects = new List<Point>(_objects);

			Generate(_block, rnd, new Rectangle(0, 0, MapBlock.SIZE - 1, MapBlock.SIZE - 1), objects);
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
		/// добавление коридоров, идущих из комнат
		/// </summary>
		private void AddConnectionPoints(MapBlock _block)
		{
			var rnd = new Random(_block.RandomSeed);
			foreach (var room in _block.Rooms)
			{
				if (_block.BlockId == MapBlock.GetBlockCoords(EnterCoords) &&
				    room.RoomRectangle.ContainsEx(MapBlock.GetInBlockCoords(EnterCoords)))
				{
					room.IsConnected = true;
				}

				var trys = 0;

				do
				{
					trys++;
					var cps = new List<ConnectionPoint>();

					EDirections dirs;
					do
					{
						dirs = (EDirections) rnd.Next(16);
					} while (dirs == EDirections.NONE);

					foreach (EDirections dir in Enum.GetValues(typeof (EDirections)))
					{
						if (dir == EDirections.NONE || !dirs.HasFlag(dir)) continue;

						int val;
						var begin = Point.Zero;
						switch (dir)
						{
							case EDirections.UP:
								val = room.RoomRectangle.Left + rnd.Next(room.RoomRectangle.Width);
								begin = new Point(val, room.RoomRectangle.Top - 1);
								break;
							case EDirections.DOWN:
								val = room.RoomRectangle.Left + rnd.Next(room.RoomRectangle.Width);
								begin = new Point(val, room.RoomRectangle.Bottom);
								break;
							case EDirections.LEFT:
								val = room.RoomRectangle.Top + rnd.Next(room.RoomRectangle.Height);
								begin = new Point(room.RoomRectangle.Left - 1, val);
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

						if (!Blocks.ContainsKey(MapBlock.GetBlockCoords(begin + _block.BlockId * MapBlock.SIZE + delta * MapBlock.SIZE)))
						{
							continue;
						}

						do
						{
							end += delta;
							if (!room.AreaRectangle.ContainsEx(end)) break;
						} while (true);

						cps.Add(new ConnectionPoint(begin + _block.BlockId * MapBlock.SIZE, end + _block.BlockId * MapBlock.SIZE, room, dir));
					}

					if (cps.Count > 1 || (trys>5 && cps.Count>0))
					{
						m_connectionPoints.AddRange(cps);
						break;
					}
				} while (true);
			}
		}

		private void LinkRooms()
		{
			var rooms = Blocks.Values.SelectMany(_mapBlock => _mapBlock.Rooms).ToArray();

			if (rooms.Length == 0) return;

			var forbid = new List<Point>();

			{
				foreach (var room in rooms)
				{
					var rrect = room.RoomRectangle;

					var blockId = room.BlockId;

					rrect.Offset(blockId.X*MapBlock.SIZE, blockId.Y*MapBlock.SIZE);
					rrect.Inflate(1, 1);
					forbid.AddRange(rrect.AllPoints().Except(forbid));
				}
			}

			var forbidMin = new Point(forbid.Min(_point1 => _point1.X), forbid.Min(_point1 => _point1.Y));
			var forbidMax = new Point(forbid.Max(_point1 => _point1.X), forbid.Max(_point1 => _point1.Y));

			Debug.WriteLine("FORBID " + forbidMin + " - " + forbidMax);

			var forbidRect = new Rectangle(forbidMin.X, forbidMin.Y, forbidMax.X - forbidMin.X, forbidMax.Y - forbidMin.Y);
			forbidRect.Inflate(-1, -1);

			var connectionPoints = m_connectionPoints.ToList();

			Action<IEnumerable<ConnectionPoint>> actRemove = delegate(IEnumerable<ConnectionPoint> _points)
			                                                 	{
			                                                 		foreach (var connectionPoint in _points)
			                                                 		{
			                                                 			connectionPoints.Remove(connectionPoint);
			                                                 		}
			                                                 	};

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
					ConnectRooms(points[0].Room, points[1].Room, forbid,points[0].Begin, points[0].End, points[1].Begin);
					foreach (var point in grouping)
					{
						connectionPoints.Remove(point);
					}
				}

				//actRemove(connectionPoints.Where(_point => _point.Room.ConnectedTo.Count > 1).ToArray());

				#endregion
			}

			if (true)
			{
				#region концевая точка касается другой комнаты

				{
					var toRemove = new List<ConnectionPoint>();

					foreach (var connectionPoint in connectionPoints)
					{
						if (connectionPoint.End == new Point(18, 64))
						{

						}

						if (toRemove.Contains(connectionPoint)) continue;
						foreach (var room in rooms)
						{
							if (room == connectionPoint.Room) continue;
							var rrect = room.RoomRectangle;
							rrect.Offset(room.BlockId.X*MapBlock.SIZE, room.BlockId.Y*MapBlock.SIZE);
							var frect = rrect;
							frect.Inflate(1, 1);
							var end = connectionPoint.End;
							if (frect.ContainsEx(end))
							{
								while (rrect.ContainsEx(end))
								{
									end += connectionPoint.Dir.Opposite().GetDelta();
								}

								//if (frect.AllPointsExceptCorners().Contains(end))
								{
									ConnectRooms(room, connectionPoint.Room, forbid, end, connectionPoint.Begin);

									toRemove.Add(connectionPoint);
									var revert = connectionPoints.Where(_point => _point.Dir == connectionPoint.Dir.Opposite() && _point.Room == room).ToArray();
									toRemove.AddRange(revert);
								}
								//else
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
							if ((points[i].End - points[i + 1].End).QLenght > MAX_PATH_LEN) continue;
							var flag = points[i].End.GetLineToPoints(points[i + 1].End).All(_point => !forbid.Contains(_point));
							if (flag)
							{
								ConnectRooms(points[i].Room, points[i + 1].Room, forbid, points[i].Begin, points[i].End, points[i + 1].End,
								             points[i + 1].Begin);
								toRemove.Add(points[i]);
								toRemove.Add(points[i + 1]);
							}
						}
					}
					actRemove(toRemove.Distinct());
				}
				//actRemove(connectionPoints.Where(_point => _point.Room.ConnectedTo.Count > 2).ToArray());

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
							if ((points[i].End - points[i + 1].End).QLenght > MAX_PATH_LEN) continue;
							var flag = points[i].End.GetLineToPoints(points[i + 1].End).All(_point => !forbid.Contains(_point));
							if (flag)
							{
								ConnectRooms(points[i].Room, points[i + 1].Room, forbid, points[i].Begin, points[i].End, points[i + 1].End, points[i + 1].Begin);
								toRemove.Add(points[i]);
								toRemove.Add(points[i + 1]);
							}
						}
					}
					actRemove(toRemove.Distinct());
				}
				//actRemove(connectionPoints.Where(_point => _point.Room.ConnectedTo.Count > 2).ToArray());

				#endregion
			}

			if(true)
			{
				#region можно проложить путь между двумя концами не затрагивая forbid

				var toRemove = new List<ConnectionPoint>();
				
				foreach (var cp in connectionPoints)
				{
					if (cp.End == new Point(21, -51))
					{

					}

					if(toRemove.Contains(cp) || forbid.Contains(cp.End)) continue;

					var candidates = connectionPoints.Where(_point => _point.Room != cp.Room  && cp.Dir<=_point.Dir  && !_point.Room.ConnectedTo.ContainsKey(cp.Room) && !forbid.Contains(_point.End) && (_point.End - cp.End).QLenght < MAX_PATH_LEN).ToArray();

					foreach (var candidate in candidates)
					{
						if (toRemove.Contains(cp) || toRemove.Contains(candidate)) continue;

						var points = new[] {cp, candidate};//.OrderBy(_point => _point.Dir).ToArray();

						var minx = cp.End.X < candidate.End.X ? cp : candidate;
						var miny = cp.End.Y < candidate.End.Y ? cp : candidate;
						var maxx = cp.End.X > candidate.End.X ? cp : candidate;
						var maxy = cp.End.Y > candidate.End.Y ? cp : candidate;

						var dx = maxx.End.X - minx.End.X;
						var dy = maxy.End.Y - miny.End.Y;

						var way1 = new List<Point>();
						var way2 = new List<Point>();

						for (var i = 0; i < 20;++i )
						{
							way1.Clear();
							way2.Clear();
							switch (points[0].Dir)
							{
								case EDirections.UP:
									switch (points[1].Dir)
									{
										case EDirections.UP:
											if (dx >= dy)
											{
												way1.AddRange(new[]
												              	{
												              		minx.Begin, minx.End,

												              		new Point(minx.End.X, miny.End.Y - i),
												              		new Point(maxx.End.X, miny.End.Y - i),

												              		maxx.End, maxx.Begin
												              	});
											}
											else
											{
												way1.AddRange(new[]
												              	{
												              		miny.Begin, miny.End,

												              		new Point(miny.End.X - i, miny.End.Y),
												              		new Point(miny.End.X - i, maxy.End.Y),

												              		maxx.End, maxx.Begin
												              	});
												way2.AddRange(new[]
												              	{
												              		minx.Begin, minx.End,

												              		new Point(miny.End.X + i, miny.End.Y),
												              		new Point(miny.End.X + i, maxy.End.Y),

												              		maxx.End, maxx.Begin
												              	});
											}
											break;
										case EDirections.DOWN:
											if (maxx.End.X - minx.End.X < 2) continue;
											way1.AddRange(new[] 
											{ 
												miny.Begin, miny.End, 

												new Point(miny.End.X+i, miny.End.Y),
												new Point(miny.End.X+i, maxy.End.Y),

												maxy.End, maxy.Begin 
											});
											way2.AddRange(new[] 
											{ 
												miny.Begin, miny.End, 

												new Point(miny.End.X-i, miny.End.Y),
												new Point(miny.End.X-i, maxy.End.Y),

												maxy.End, maxy.Begin 
											});
											break;
										case EDirections.LEFT:
											way1.AddRange(new[] 
											{ 
												minx.Begin, minx.End, 
												new Point(minx.End.X-i, minx.End.Y),
												new Point(minx.End.X-i, maxx.End.Y),
												maxx.End, maxx.Begin 
											});
											way2.AddRange(new[] 
											{ 
												minx.Begin, minx.End, 
												new Point(minx.End.X, minx.End.Y-i),
												new Point(minx.End.X, maxx.End.Y-i),
												maxx.End, maxx.Begin 
											});
											break;
										case EDirections.RIGHT:
											way1.AddRange(new[] 
											{ 
												minx.Begin, minx.End, 
												new Point(minx.End.X+i, minx.End.Y),
												new Point(minx.End.X+i, maxx.End.Y),
												maxx.End, maxx.Begin 
											});
											way2.AddRange(new[] 
											{ 
												minx.Begin, minx.End, 
												new Point(minx.End.X, minx.End.Y+i),
												new Point(minx.End.X, maxx.End.Y+i),
												maxx.End, maxx.Begin 
											});
											break;
										default:
											throw new ArgumentOutOfRangeException();
									}
									break;
								case EDirections.DOWN:
									switch (points[1].Dir)
									{
										case EDirections.DOWN:
											if (dx >= dy)
											{
												way1.AddRange(new[]
												              	{
												              		minx.Begin, minx.End,

												              		new Point(minx.End.X, miny.End.Y + i),
												              		new Point(maxx.End.X, miny.End.Y + i),

												              		maxx.End, maxx.Begin
												              	});
											}
											else
											{
												way1.AddRange(new[]
												              	{
												              		miny.Begin, miny.End,

												              		new Point(miny.End.X - i, miny.End.Y),
												              		new Point(miny.End.X - i, maxy.End.Y),

												              		maxx.End, maxx.Begin
												              	});
												way2.AddRange(new[]
												              	{
												              		minx.Begin, minx.End,

												              		new Point(miny.End.X + i, miny.End.Y),
												              		new Point(miny.End.X + i, maxy.End.Y),

												              		maxx.End, maxx.Begin
												              	});
											}
											break;
										case EDirections.LEFT:
											way1.AddRange(new[] 
											{ 
												minx.Begin, minx.End, 
												new Point(minx.End.X, minx.End.Y + i),
												new Point(maxx.End.X, minx.End.Y + i),
												maxx.End, maxx.Begin 
											});
											way2.AddRange(new[] 
											{ 
												minx.Begin, minx.End, 
												new Point(maxx.End.X - i, minx.End.Y),
												new Point(maxx.End.X - i, maxx.End.Y),
												maxx.End, maxx.Begin 
											});
											break;
										case EDirections.RIGHT:
											way1.AddRange(new[] 
											{ 
												minx.Begin, minx.End, 
												new Point(minx.End.X+i, minx.End.Y),
												new Point(minx.End.X+i, maxx.End.Y),
												maxx.End, maxx.Begin 
											});
											way2.AddRange(new[] 
											{ 
												minx.Begin, minx.End, 
												new Point(minx.End.X, minx.End.Y+i),
												new Point(minx.End.X, maxx.End.Y+i),
												maxx.End, maxx.Begin 
											});
											break;
										default:
											throw new ArgumentOutOfRangeException();
									}
									break;
								case EDirections.LEFT:
									switch (points[1].Dir)
									{
										case EDirections.LEFT:
											if (maxy.End.Y - miny.End.Y < 2) continue;
											way1.AddRange(new[] 
											{ 
												miny.Begin, miny.End, 

												new Point(miny.End.X - i, miny.End.Y),
												new Point(miny.End.X - i, maxy.End.Y),

												maxy.End, maxy.Begin 
											});
											break;
										case EDirections.RIGHT:
											if (i>0) continue;
											way1.AddRange(new[] 
											{ 
												miny.Begin, miny.End, 
												new Point(maxy.End.X, miny.End.Y), 
												maxy.End, maxy.Begin 
											});
											way2.AddRange(new[] 
											{ 
												miny.Begin, miny.End, 
												new Point(miny.End.X, maxy.End.Y), 
												maxy.End, maxy.Begin 
											});
											break;
										default:
											throw new ArgumentOutOfRangeException();
									}
									break;
								case EDirections.RIGHT:
									switch (points[1].Dir)
									{
										case EDirections.RIGHT:
											if (maxy.End.Y - miny.End.Y < 2) continue;
											way1.AddRange(new[] 
											{ 
												miny.Begin, miny.End, 

												new Point(maxx.End.X + i, miny.End.Y),
												new Point(maxx.End.X + i, maxy.End.Y),

												maxy.End, maxy.Begin 
											});
											break;
										default:
											throw new ArgumentOutOfRangeException();
									}
									break;
								default:
									throw new ArgumentOutOfRangeException();
							}
							bool flag = false;
							foreach (var way in new[] { way1, way2 })
							{
								flag = true;
								if (way.Count == 0) continue;
								var pnt = way[1];
								
								for (var index = 2; index < way.Count - 1 && flag; ++index)
								{
									flag = !pnt.GetLineToPoints(way[index]).Any(forbid.Contains);
									pnt = way[index];
								}
								if (!flag) continue;
								if (way[0] != cp.Begin) way.Reverse();

								Debug.WriteLine(points[0].Dir + " => " + points[1].Dir);

								ConnectRooms(cp.Room, candidate.Room, forbid, way1.ToArray());
								toRemove.Add(candidate);
								break;
							}
							if(flag)
							{
								break;
							}
						}
					}
					toRemove.Add(cp);
				}

				actRemove(toRemove.Distinct());

				#endregion
			}

			if(true)
			{
				#region коннектим оставшиеся комнаты

				//var toRemove = new List<ConnectionPoint>();

				//while (connectionPoints.Count > 0)
				//{
				//    toRemove.Clear();
				//    toRemove.AddRange(connectionPoints.Where(_point => _point.Room.IsConnected));
				//    foreach (var connectionPoint in connectionPoints)
				//    {
				//        if(toRemove.Contains(connectionPoint)) continue;



				//    }
				//    actRemove(toRemove);
				//} 

				#endregion

			}

			foreach (var point in forbid)
			{
				var inBlock = MapBlock.GetInBlockCoords(point);
				var block = this[MapBlock.GetBlockCoords(point)];

				if (block.Map[inBlock.X, inBlock.Y] == ETerrains.GRASS || block.Map[inBlock.X, inBlock.Y] == ETerrains.GROUND)
				{
					block.Map[inBlock.X, inBlock.Y] = ETerrains.LAVA;
				}
			}

			foreach (var room in rooms)
			{
				var block = this[room.BlockId];
				if (room.IsConnected)
				{
					MapBlockHelper.Fill(block, new Random(block.RandomSeed), this, new[] { ETerrains.STONE_FLOOR, }, room.RoomRectangle);
				}
				else
				{
					if (room.ConnectedTo.Any())
					{
						MapBlockHelper.Fill(block, new Random(block.RandomSeed), this, new[] { ETerrains.WATER, }, room.RoomRectangle);
					}
					else
					{
						MapBlockHelper.Fill(block, new Random(block.RandomSeed), this, new[] { ETerrains.SWAMP, }, room.RoomRectangle);
					}
				}
			}

			//actRemove(connectionPoints.Where(_point => _point.Room.IsConnected || _point.Room.ConnectedTo.Count > 2).ToArray());
			foreach (var connectionPoint in connectionPoints)
			{
				foreach (var point in connectionPoint.Begin.GetLineToPoints(connectionPoint.End))
				{
					var inBlock = MapBlock.GetInBlockCoords(point);
					Blocks[MapBlock.GetBlockCoords(point)].Map[inBlock.X, inBlock.Y] = connectionPoint.Dir.GetTerrain();
				}
			}
		}

		private void ConnectRooms(Room _room1, Room _room2, ICollection<Point> _forbid, params Point[] _points)
		{
			var pnt = _points[0];
			for (var i = 1; i < _points.Length; ++i)
			{
				//if (pnt == _points[i]) continue;

				var line = pnt.GetLineToPoints(_points[i]).ToArray();
				var border = Util.GetDirection(pnt, _points[i]).GetBorders().ToArray();
				for (var index = 0; index < line.Length; index++)
				{
					var point = line[index];
					var inBlock = MapBlock.GetInBlockCoords(point);
					this[MapBlock.GetBlockCoords(point)].Map[inBlock.X, inBlock.Y] = ETerrains.WATER;

					if (index != 0 && index < line.Length - 1)
					{
						foreach (var delta in border)
						{
							var borderPnt = point + delta;
							_forbid.Add(borderPnt);
						}
					}
				}
				pnt = _points[i];
			}
			_room1.Connect(_room2, _points);

			Debug.WriteLine("CONNECTED:" + _room1.RoomRectangle + " to " + _room2.RoomRectangle + " by " + string.Join("|", _points.Select(_point => _point.ToString())));
		}
	}
}
