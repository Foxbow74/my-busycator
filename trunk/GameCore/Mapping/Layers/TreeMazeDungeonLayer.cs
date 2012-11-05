using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Materials;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture;
using GameCore.Objects.Furniture.LightSources;

namespace GameCore.Mapping.Layers
{
	internal class TreeMazeDungeonLayer : DungeonLayer
	{
		private const int MAX_PATH_LEN = 20;

		private readonly Dictionary<Point, BaseMapBlock> m_mazeBlocks = new Dictionary<Point, BaseMapBlock>();

		public TreeMazeDungeonLayer(Point _enterCoords, Random _rnd)
			: base(_enterCoords)
		{
			var enterBlock = BaseMapBlock.GetBlockId(_enterCoords);

			var size = _rnd.Next(5) + _rnd.Next(5) + 5;
			var center = new Point(size, size)/2;
			var map = new EMapBlockTypes[size,size];

			var list = LayerHelper.GetRandomPoints(center, _rnd, map, size, EMapBlockTypes.GROUND, EMapBlockTypes.NONE);
			var blockIds = list.Distinct().Select(_point => _point - center + enterBlock).ToArray();

			foreach (var blockId in blockIds)
			{
				BaseMapBlock block;
				if (!m_mazeBlocks.TryGetValue(blockId, out block))
				{
					block = new BaseMapBlock(blockId);
				}

				if (BaseMapBlock.GetBlockId(EnterCoords) == blockId)
				{
					GenerateInternal(block, new[] {BaseMapBlock.GetInBlockCoords(EnterCoords)});
				}
				else
				{
					GenerateInternal(block);
				}
				m_mazeBlocks[block.BlockId] = block;
			}

			var connectionPoints = new List<ConnectionPoint>();

			foreach (var block in blockIds.Select(_blockId => m_mazeBlocks[_blockId]))
			{
				var rnd = new Random(block.RandomSeed);
				foreach (var room in block.Rooms)
				{
					connectionPoints.AddRange(AddConnectionPoints(block, room, rnd));
				}
			}

			LinkRooms(connectionPoints);

			foreach (var mapBlock in m_mazeBlocks.Values)
			{
				foreach (var room in mapBlock.Rooms.Where(_room => _room.IsConnected))
				{
					var border = room.RoomRectangle.BorderPoints.ToArray();
					foreach (var point in border)
					{
						if (_rnd.NextDouble() > 0.7)
						{
							var dir = EDirections.NONE;
							if (point.X > 0 && TerrainAttribute.GetAttribute(mapBlock.Map[point.X - 1, point.Y]).IsPassable == 0)
							{
								dir = EDirections.RIGHT;
							}
							else if (point.X < Constants.MAP_BLOCK_SIZE - 1 && TerrainAttribute.GetAttribute(mapBlock.Map[point.X + 1, point.Y]).IsPassable == 0)
							{
								dir = EDirections.LEFT;
							}
							else if (point.Y > 0 && TerrainAttribute.GetAttribute(mapBlock.Map[point.X, point.Y - 1]).IsPassable == 0)
							{
								dir = EDirections.DOWN;
							}
							else if (point.Y < Constants.MAP_BLOCK_SIZE - 1 && TerrainAttribute.GetAttribute(mapBlock.Map[point.X, point.Y + 1]).IsPassable == 0)
							{
								dir = EDirections.UP;
							}
							if (dir == EDirections.NONE) continue;
							var fColor = new FColor(3f, (float) _rnd.NextDouble(), (float) _rnd.NextDouble(), (float) _rnd.NextDouble());
							mapBlock.AddObject(new OnWallTorch(new LightSource(_rnd.Next(4) + 3, fColor), dir, ThingHelper.GetMaterial<OakMaterial>()), point);
							break;
						}
					}
				}
			}
		}

		internal override IEnumerable<ETile> DefaultWalls { get { yield return ETile.STONE_WALL; } }

		internal override IEnumerable<ETile> DefaultEmptySpaces { get { yield return ETile.STONE_FLOOR; } }

		public override FColor Ambient { get { return new FColor(1f, 0.1f, 0f, 0f); } }

		protected override MapBlock GenerateBlock(Point _blockId)
		{
			BaseMapBlock baseMapBlock;
			if (!m_mazeBlocks.TryGetValue(_blockId, out baseMapBlock))
			{
				var eblock = new MapBlock(_blockId);
				var ernd = new Random(eblock.RandomSeed);
				MapBlockHelper.Clear(eblock, ernd, this, DefaultWalls);
				return eblock;
			}
			var block = new MapBlock(_blockId, baseMapBlock);
			var rnd = new Random(block.RandomSeed);
			AddItems(block, rnd);
			AddCreatures(block, rnd);

			return block;
		}

		private static void AddCreatures(MapBlock _block, Random _rnd)
		{
			var itmcnt = 2 + _rnd.Next(_rnd.Next(2));
			for (var i = 0; i < itmcnt; ++i)
			{
				var x = _rnd.Next(Constants.MAP_BLOCK_SIZE);
				var y = _rnd.Next(Constants.MAP_BLOCK_SIZE);

				var attr = TerrainAttribute.GetAttribute(_block.Map[x, y]);
				if (attr.IsPassable > 0)
				{
					var point = new Point(x, y);
					var any = _block.Creatures.Where(_tuple => _tuple.Item2 == point).Select(_tuple => _tuple.Item1);
					var creature = ThingHelper.GetFakedCreature(_block);

					if (creature.Is<Stair>() && (x == Constants.MAP_BLOCK_SIZE - 1 || y == Constants.MAP_BLOCK_SIZE - 1))
					{
						continue;
					}
					if (any.Any())
					{
						continue;
					}

					if (!_block.Rooms.Any(_room => _room.RoomRectangle.Contains(point) && _room.IsConnected))
					{
						continue;
					}

					_block.AddCreature(creature, point);
				}
			}
		}

		private static void AddItems(MapBlock _block, Random _rnd)
		{
			var itmcnt = 20 + _rnd.Next(_rnd.Next(20));
			for (var i = 0; i < itmcnt; ++i)
			{
				var x = _rnd.Next(Constants.MAP_BLOCK_SIZE);
				var y = _rnd.Next(Constants.MAP_BLOCK_SIZE);

				var attr = TerrainAttribute.GetAttribute(_block.Map[x, y]);
				if (attr.IsPassable > 0)
				{
					var point = new Point(x, y);
					var any = _block.Objects.Where(_tuple => _tuple.Item2 == point).Select(_tuple => _tuple.Item1);
					var thing = ThingHelper.GetFakedItem(_block.RandomSeed);

					if (thing.Is<Stair>() && (x == Constants.MAP_BLOCK_SIZE - 1 || y == Constants.MAP_BLOCK_SIZE - 1))
					{
						continue;
					}

					if (thing is Item)
					{
						if (any.Any(_thing => !(_thing is Item)))
						{
							continue;
						}
					}
					else if (any.Any())
					{
						continue;
					}
					_block.AddObject(thing, point);
				}
			}
		}

		private void GenerateInternal(BaseMapBlock _block, params Point[] _objects)
		{
			var rnd = new Random(_block.RandomSeed);
			MapBlockHelper.Clear(_block, rnd, this, DefaultWalls);
			var rooms = LayerHelper.GenerateRooms(rnd, new Rct(0, 0, Constants.MAP_BLOCK_SIZE - 1, Constants.MAP_BLOCK_SIZE - 1), new List<Point>(_objects), _block.BlockId);
			foreach (var room in rooms)
			{
				MapBlockHelper.Fill(_block, rnd, this, DefaultEmptySpaces, room.RoomRectangle);
				_block.AddRoom(room);
			}
		}

		/// <summary>
		/// 	добавление коридоров, идущих из комнат
		/// </summary>
		private IEnumerable<ConnectionPoint> AddConnectionPoints(BaseMapBlock _block, Room _room, Random _rnd)
		{
			if (_block.BlockId == BaseMapBlock.GetBlockId(EnterCoords) &&
			    _room.RoomRectangle.Contains(BaseMapBlock.GetInBlockCoords(EnterCoords)))
			{
				_room.IsConnected = true;
			}

			var trys = 0;

			do
			{
				trys++;
				var cps = new List<ConnectionPoint>();

				var dirs = _rnd.GetRandomDirections();

				foreach (var dir in dirs.AllDirectionsIn())
				{
					int val;
					Point begin;
					switch (dir)
					{
						case EDirections.UP:
							val = _room.RoomRectangle.Left + _rnd.Next(_room.RoomRectangle.Width);
							begin = new Point(val, _room.RoomRectangle.Top - 1);
							break;
						case EDirections.DOWN:
							val = _room.RoomRectangle.Left + _rnd.Next(_room.RoomRectangle.Width);
							begin = new Point(val, _room.RoomRectangle.Bottom);
							break;
						case EDirections.LEFT:
							val = _room.RoomRectangle.Top + _rnd.Next(_room.RoomRectangle.Height);
							begin = new Point(_room.RoomRectangle.Left - 1, val);
							break;
						case EDirections.RIGHT:
							val = _room.RoomRectangle.Top + _rnd.Next(_room.RoomRectangle.Height);
							begin = new Point(_room.RoomRectangle.Right, val);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					var end = begin.Clone();

					var delta = dir.GetDelta();

					if (!m_mazeBlocks.ContainsKey(BaseMapBlock.GetBlockId(begin + _block.BlockId*Constants.MAP_BLOCK_SIZE + delta*Constants.MAP_BLOCK_SIZE)))
					{
						continue;
					}

					do
					{
						end += delta;
						if (!_room.AreaRectangle.Contains(end)) break;
					} while (true);

					cps.Add(new ConnectionPoint(begin + _block.BlockId*Constants.MAP_BLOCK_SIZE, end + _block.BlockId*Constants.MAP_BLOCK_SIZE, _room, dir));
				}

				if (cps.Count > 1 || (trys > 5 && cps.Count > 0) || trys > 20)
				{
					foreach (var connectionPoint in cps)
					{
						yield return connectionPoint;
					}
					break;
				}
			} while (true);
		}

		private void LinkRooms(ICollection<ConnectionPoint> _connectionPoints)
		{
			var connectors = new Dictionary<Point, Connector>();

			var rooms = m_mazeBlocks.Values.SelectMany(_mapBlock => _mapBlock.Rooms).ToArray();

			if (rooms.Length == 0) return;

			var forbid = new Dictionary<Point, EDirections>();

			{
				foreach (var room in rooms)
				{
					foreach (var pair in room.WorldRoomRectangle.AllForbidBorders())
					{
						EDirections dir;
						if (!forbid.TryGetValue(pair.Key, out dir))
						{
							dir = EDirections.DOWN | EDirections.UP | EDirections.LEFT | EDirections.RIGHT;
						}
						dir &= pair.Value;
						forbid[pair.Key] = dir;
					}
				}
			}

			Action<IEnumerable<ConnectionPoint>> actRemove = delegate(IEnumerable<ConnectionPoint> _points)
			                                                 	{
			                                                 		foreach (var connectionPoint in _points)
			                                                 		{
			                                                 			_connectionPoints.Remove(connectionPoint);
			                                                 		}
			                                                 	};

			if (true)
			{
				#region конечные точки совпадают

				var sameEndPoints = _connectionPoints.GroupBy(_point => _point.End).Where(_points => _points.Count() > 1).ToArray();
				foreach (var grouping in sameEndPoints)
				{
					var points = grouping.ToArray();
					if (points.Length > 2)
					{
						throw new NotImplementedException("Как может сойтись в одной точке более двух комнат????");
					}
					ConnectTwoRooms(points[0].Room, points[1].Room, forbid, connectors, points[0].Begin, points[0].End, points[1].Begin);
					foreach (var point in grouping)
					{
						_connectionPoints.Remove(point);
					}
				}

				#endregion
			}

			if (true)
			{
				#region концевая точка касается другой комнаты

				{
					var toRemove = new List<ConnectionPoint>();

					foreach (var cp in _connectionPoints)
					{
						if (toRemove.Contains(cp)) continue;

						var oppositeDelta = cp.Dir.Opposite().GetDelta();
						foreach (var room in rooms)
						{
							if (room == cp.Room) continue;
							var rrect = room.WorldRoomRectangle;
							var frect = rrect.Inflate(1, 1);
							var end = cp.End;
							if (frect.Contains(end))
							{
								while (rrect.Contains(end))
								{
									end += oppositeDelta;
								}

								if (!frect.AllPointsExceptCorners().Contains(end))
								{
									end += cp.Dir.GetDelta();
								}
								{
									ConnectTwoRooms(room, cp.Room, forbid, connectors, end, cp.Begin);

									toRemove.Add(cp);
									var revert = _connectionPoints.Where(_point => _point.Dir == cp.Dir.Opposite() && _point.Room == room).ToArray();
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
					var sameCol = _connectionPoints.GroupBy(_point => _point.End.X).Where(_points => _points.Count() > 1).ToArray();
					foreach (var grouping in sameCol)
					{
						var points = grouping.OrderBy(_point => _point.End.Y).ToArray();

						for (var i = 0; i < points.Length - 1; ++i)
						{
							if ((points[i].End - points[i + 1].End).QLenght > MAX_PATH_LEN) continue;
							if (!points[i].End.GetLineToPoints(points[i + 1].End).Any(forbid.ContainsKey))
							{
								ConnectTwoRooms(points[i].Room,
								                points[i + 1].Room,
								                forbid,
								                connectors,
								                points[i].Begin,
								                points[i].End,
								                points[i + 1].End,
								                points[i + 1].Begin);
								toRemove.Add(points[i]);
								toRemove.Add(points[i + 1]);
							}
						}
					}
					actRemove(toRemove.Distinct());
				}

				#endregion
			}

			if (true)
			{
				#region Конечные точки на одной строке

				{
					var toRemove = new List<ConnectionPoint>();
					var sameRow = _connectionPoints.GroupBy(_point => _point.End.Y).Where(_points => _points.Count() > 1).ToArray();
					foreach (var grouping in sameRow)
					{
						var points = grouping.OrderBy(_point => _point.End.X).ToArray();

						for (var i = 0; i < points.Length - 1; ++i)
						{
							if ((points[i].End - points[i + 1].End).QLenght > MAX_PATH_LEN) continue;
							if (!points[i].End.GetLineToPoints(points[i + 1].End).Any(forbid.ContainsKey))
							{
								ConnectTwoRooms(points[i].Room, points[i + 1].Room, forbid, connectors, points[i].Begin, points[i].End, points[i + 1].End, points[i + 1].Begin);
								toRemove.Add(points[i]);
								toRemove.Add(points[i + 1]);
							}
						}
					}
					actRemove(toRemove.Distinct());
				}

				#endregion
			}

			if (true)
			{
				#region можно проложить путь между двумя концами не затрагивая forbid

				var toRemove = new List<ConnectionPoint>();

				foreach (var cp in _connectionPoints)
				{
					if (toRemove.Contains(cp) || forbid.ContainsKey(cp.End)) continue;

					var candidates = _connectionPoints.Where(_point => _point.Room != cp.Room && cp.Dir <= _point.Dir && !_point.Room.ConnectedTo.Contains(cp.Room) && !forbid.ContainsKey(_point.End) && (_point.End - cp.End).QLenght < MAX_PATH_LEN).ToArray();

					foreach (var candidate in candidates)
					{
						if (toRemove.Contains(cp) || toRemove.Contains(candidate)) continue;

						var points = new[] {cp, candidate}; //.OrderBy(_point => _point.Dir).ToArray();

						var minx = cp.End.X < candidate.End.X ? cp : candidate;
						var miny = cp.End.Y < candidate.End.Y ? cp : candidate;
						var maxx = cp.End.X > candidate.End.X ? cp : candidate;
						var maxy = cp.End.Y > candidate.End.Y ? cp : candidate;

						var dx = maxx.End.X - minx.End.X;
						var dy = maxy.End.Y - miny.End.Y;

						var way1 = new List<Point>();
						var way2 = new List<Point>();

						for (var i = 0; i < 20; ++i)
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
											              		new Point(miny.End.X + i, miny.End.Y),
											              		new Point(miny.End.X + i, maxy.End.Y),
											              		maxy.End, maxy.Begin
											              	});
											way2.AddRange(new[]
											              	{
											              		miny.Begin, miny.End,
											              		new Point(miny.End.X - i, miny.End.Y),
											              		new Point(miny.End.X - i, maxy.End.Y),
											              		maxy.End, maxy.Begin
											              	});
											break;
										case EDirections.LEFT:
											way1.AddRange(new[]
											              	{
											              		minx.Begin, minx.End,
											              		new Point(minx.End.X - i, minx.End.Y),
											              		new Point(minx.End.X - i, maxx.End.Y),
											              		maxx.End, maxx.Begin
											              	});
											way2.AddRange(new[]
											              	{
											              		minx.Begin, minx.End,
											              		new Point(minx.End.X, minx.End.Y - i),
											              		new Point(minx.End.X, maxx.End.Y - i),
											              		maxx.End, maxx.Begin
											              	});
											break;
										case EDirections.RIGHT:
											way1.AddRange(new[]
											              	{
											              		minx.Begin, minx.End,
											              		new Point(minx.End.X + i, minx.End.Y),
											              		new Point(minx.End.X + i, maxx.End.Y),
											              		maxx.End, maxx.Begin
											              	});
											way2.AddRange(new[]
											              	{
											              		minx.Begin, minx.End,
											              		new Point(minx.End.X, minx.End.Y + i),
											              		new Point(minx.End.X, maxx.End.Y + i),
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
											              		new Point(minx.End.X + i, minx.End.Y),
											              		new Point(minx.End.X + i, maxx.End.Y),
											              		maxx.End, maxx.Begin
											              	});
											way2.AddRange(new[]
											              	{
											              		minx.Begin, minx.End,
											              		new Point(minx.End.X, minx.End.Y + i),
											              		new Point(minx.End.X, maxx.End.Y + i),
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
											if (i > 0) continue;
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
							var flag = false;
							foreach (var way in new[] {way1, way2})
							{
								flag = true;
								if (way.Count == 0) continue;
								var pnt = way[1];

								for (var index = 2; index < way.Count - 1 && flag; ++index)
								{
									flag = !pnt.GetLineToPoints(way[index]).Any(forbid.ContainsKey);
									pnt = way[index];
								}
								if (!flag) continue;
								if (way[0] != cp.Begin) way.Reverse();

								ConnectTwoRooms(cp.Room, candidate.Room, forbid, connectors, way1.ToArray());
								toRemove.Add(candidate);
								break;
							}
							if (flag)
							{
								break;
							}
						}
					}
					if (cp.Room.IsConnected)
					{
						toRemove.Add(cp);
					}
				}

				actRemove(toRemove.Distinct());

				#endregion
			}

			if (true)
			{
				#region остатки ConnectionPoints

				var notConnectedToAny = rooms.Where(_room => !_room.IsConnected).ToList();
				foreach (var room in notConnectedToAny)
				{
					var cps = _connectionPoints.Where(_point => _point.Room == room).ToList();
					if (cps.Count == 0)
					{
						continue;
					}
					foreach (var cp in cps)
					{
						if (cp.End == new Point(-2, 73))
						{
						}
						var delta = cp.Dir.GetDelta();
						var point = cp.End;
						Room rm = null;
						var i = 0;
						var flag = false;
						for (; i < 10; ++i)
						{
							point += delta;
							EDirections dir;
							if (forbid.TryGetValue(point, out dir))
							{
								if (!dir.HasFlag(cp.Dir)) break;
							}

							Connector connector;
							if (connectors.TryGetValue(point, out connector))
							{
								ConnectTwoRooms(room, null, forbid, connectors, cp.Begin, cp.End, point);
								flag = true;
								break;
							}
							else
							{
								rm = rooms.FirstOrDefault(_room => _room.WorldRoomRectangle.Contains(point));
								if (rm != null)
								{
									ConnectTwoRooms(room, rm, forbid, connectors, cp.Begin, cp.End, point - delta);
									flag = true;
									break;
								}
							}
						}
						_connectionPoints.Remove(cp);
						if (flag) break;
					}
				}

				#endregion
			}

			if (true)
			{
				#region финальный этап

				while (true)
				{
					var nConnectors = connectors.Where(_pair => _pair.Value.Rooms.Any(_room => !_room.IsConnected)).ToArray();
					var nKeys = nConnectors.Select(_pair => _pair.Key).ToArray();
					var pConnectors = connectors.Except(nConnectors).ToArray();
					if (nConnectors.Length == 0)
					{
						break;
					}

					var flag = false;
					foreach (var nPair in nConnectors)
					{
						var nPnt = nPair.Key;

						var possible =
							pConnectors.Where(_pair => (_pair.Key.X == nPnt.X || _pair.Key.Y == nPnt.Y)).OrderBy(
								_pair => (_pair.Key - nPnt).QLenght).ToList();
						foreach (var pPair in possible)
						{
							var pPnt = pPair.Key;

							var npDir = Util.GetDirection(nPnt, pPnt);

							flag = true;

							foreach (var point in nPnt.GetLineToPoints(pPnt))
							{
								if (point != nPnt && nKeys.Contains(point))
								{
									flag = false;
									break;
								}

								EDirections dir;
								if (forbid.TryGetValue(point, out dir))
								{
									if (!dir.HasFlag(npDir))
									{
										flag = false;
										break;
									}
								}
							}
							if (flag)
							{
								var nRooms = nPair.Value.Rooms.ToArray();
								foreach (var room in nRooms)
								{
									ConnectTwoRooms(room, null, forbid, connectors, nPnt + npDir.GetDelta(), pPnt);
								}
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}

				#endregion
			}
		}

		private void ConnectTwoRooms(Room _room1, Room _room2, IDictionary<Point, EDirections> _forbid, IDictionary<Point, Connector> _connectors, params Point[] _points)
		{
			var rnd = new Random(m_mazeBlocks[_room1.BlockId].RandomSeed);
			var pnt = _points[0];
			var defEmpty = DefaultEmptySpaces.ToArray();
			Point point = null;
			for (var i = 1; i < _points.Length; ++i)
			{
				var line = pnt.GetLineToPoints(_points[i]).ToArray();
				var border = Util.GetDirection(pnt, _points[i]).GetBorders().ToArray();
				for (var index = 0; index < line.Length; index++)
				{
					if (point == line[index]) continue;
					point = line[index];

					Connector connector;
					if (_connectors.TryGetValue(point, out connector))
					{
						_room1.Connect(connector.Rooms.ToArray());
						connector.Rooms.Add(_room1);

						if (_room2 != null)
						{
							ConnectTwoRooms(_room2, null, _forbid, _connectors, _points.Reverse().ToArray());
						}
						return;
					}
					if (_room2 != null)
					{
						_connectors.Add(point, new Connector(_room1, _room2));
					}
					else
					{
						_connectors.Add(point, new Connector(_room1));
					}
					var inBlock = BaseMapBlock.GetInBlockCoords(point);
					var blockId = BaseMapBlock.GetBlockId(point);

					BaseMapBlock block;
					if (!m_mazeBlocks.TryGetValue(blockId, out block))
					{
						block = new MapBlock(blockId);
						MapBlockHelper.Clear(block, rnd, this, DefaultWalls);
						m_mazeBlocks[blockId] = block;
					}
					block.Map[inBlock.X, inBlock.Y] = defEmpty[rnd.Next(defEmpty.Length)];

					if (index != 0 && index < line.Length - 1)
					{
						foreach (var delta in border)
						{
							var borderPnt = point + delta.Key;

							EDirections dir;
							if (!_forbid.TryGetValue(borderPnt, out dir))
							{
								dir = EDirections.DOWN | EDirections.UP | EDirections.LEFT | EDirections.RIGHT;
							}
							_forbid[borderPnt] = dir & delta.Value;
						}
					}
				}
				pnt = _points[i];
			}
			_room1.Connect(_room2);
		}
	}
}