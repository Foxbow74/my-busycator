using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Misc;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	public static class SurfaceBlockGenerator
	{
		public static MapBlock GenerateBlock(Point _blockId, EMapBlockTypes[,] _map, Dictionary<Point, MapBlock> _blocks)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);

			var pm = new EMapBlockTypes[BaseMapBlock.SIZE,BaseMapBlock.SIZE];
			var baseType = _map[_blockId.X, _blockId.Y];
					
			var points = BaseMapBlock.SIZE * BaseMapBlock.SIZE;
			var toAdd = new List<EMapBlockTypes> { baseType, baseType, baseType };

			foreach (var dir in Util.AllDirections)
			{
				Point from;
				Point to;
				switch (dir)
				{
					case EDirections.UP:
						from = Point.Zero;
						to = new Point(BaseMapBlock.SIZE - 2, 0);
						break;
					case EDirections.DOWN:
						from = new Point(1, BaseMapBlock.SIZE - 1);
						to = new Point(BaseMapBlock.SIZE - 1, BaseMapBlock.SIZE - 1);
						break;
					case EDirections.LEFT:
						from = new Point(0, BaseMapBlock.SIZE - 1);
						to = new Point(0, 1);
						break;
					case EDirections.RIGHT:
						from = new Point(BaseMapBlock.SIZE - 1, 0);
						to = new Point(BaseMapBlock.SIZE - 1, BaseMapBlock.SIZE - 2);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				var delta = dir.GetDelta();
				var nearestBlockId = _blockId + delta;

				var type= baseType;
				if (nearestBlockId.X >= 0 && nearestBlockId.X < _map.GetLength(0) && nearestBlockId.Y >= 0 &&
					nearestBlockId.Y < _map.GetLength(1))
				{
					type = _map[nearestBlockId.X, nearestBlockId.Y];
				}
				toAdd.Add(type);

				if (_blocks.ContainsKey(nearestBlockId))
				{
					var nearestBlock = _blocks[nearestBlockId];
					foreach (var point in from.GetLineToPoints(to))
					{
						type = TerrainAttribute.GetMapBlockType(nearestBlock.Map[(point.X + delta.X + MapBlock.SIZE)%MapBlock.SIZE, (point.Y + delta.Y + MapBlock.SIZE)%MapBlock.SIZE]);
						pm[point.X, point.Y] = type;
						points--;
					}
				}
				else
				{
					foreach (var point in from.GetLineToPoints(to))
					{
						pm[point.X, point.Y] = type;
						points--;
					}
				}
			}
			foreach (var t in toAdd)
			{
				Point pnt = Point.Zero;
				while (pm[pnt.X, pnt.Y] != EMapBlockTypes.NONE)
				{
					pnt = new Point(1 + rnd.Next(MapBlock.SIZE - 2), 1 + rnd.Next(MapBlock.SIZE - 2));
				}
				pm[pnt.X, pnt.Y] = t;
				points--;
			}
	
			var dpoints = Util.AllDirections.Select(_directions => _directions.GetDelta()).ToArray();
			while (points > 0)
			{
				for (var x = 0; x < BaseMapBlock.SIZE; ++x)
				{
					for (var y = 0; y < BaseMapBlock.SIZE; ++y)
					{
						var xy = pm[x, y];
						if (xy != 0)
						{
							var dpoint = dpoints[rnd.Next(4)];
							var x1 = x + dpoint.X;
							if (x1 < 0 || x1 == BaseMapBlock.SIZE) continue;
							var y1 = y + dpoint.Y;
							if (y1 < 0 || y1 == BaseMapBlock.SIZE) continue;
							var xy1 = pm[x1, y1];
							if (xy1 == 0)
							{
								//m_sizes[xy]++;
								pm[x1, y1] = xy;
								points--;
							}
							else if (xy != xy1)
							{
								if (xy1 < xy)
								{
									var a = xy1;
									xy1 = xy;
									xy = a;
								}
								//m_neighbours[xy, xy1] = true;
								//m_neighbours[xy1, xy] = true;
							}
						}
					}
				}
			}


			foreach (var pnt in new Rct(0, 0, MapBlock.SIZE, MapBlock.SIZE).AllPoints)
			{
				ETerrains tr;
				switch (pm[pnt.X,pnt.Y])
				{
					case EMapBlockTypes.NONE:
						continue;
						break;
					case EMapBlockTypes.GROUND:
						tr = ETerrains.GRASS;
						break;
					case EMapBlockTypes.FOREST:
						tr = ETerrains.FOREST;
						break;
					case EMapBlockTypes.SEA:
						tr = ETerrains.SEA;
						break;
					case EMapBlockTypes.DEEP_SEA:
						tr = ETerrains.DEEP_SEA;
						break;
					case EMapBlockTypes.FRESH_WATER:
						tr = ETerrains.FRESH_WATER;
						break;
					case EMapBlockTypes.DEEP_FRESH_WATER:
						tr = ETerrains.DEEP_FRESH_WATER;
						break;
					case EMapBlockTypes.CITY:
						tr = ETerrains.GROUND;
						break;
					case EMapBlockTypes.COAST:
						tr = ETerrains.COAST;
						break;
					case EMapBlockTypes.MOUNT:
						tr = ETerrains.MOUNT;
						break;
					case EMapBlockTypes.SWAMP:
						tr = ETerrains.SWAMP;
						break;
					case EMapBlockTypes.ETERNAL_SNOW:
						tr = ETerrains.ETERNAL_SNOW;
						break;
					case EMapBlockTypes.SHRUBS:
						tr = ETerrains.SHRUBS;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				block.Map[pnt.X, pnt.Y] = tr;
			}
			//var centers = new Dictionary<Point, Point>();
			//var dists = new Dictionary<Point, float>();
			//foreach (var dPoint in Point.NearestDPoints)
			//{
			//    centers[dPoint] = dPoint * BaseMapBlock.SIZE + BaseMapBlock.Rect.Center;
			//}

			//var terrains = new Dictionary<EMapBlockTypes, ETerrains[]>();
			//foreach (EMapBlockTypes blockTypes in Enum.GetValues(typeof(EMapBlockTypes)))
			//{
			//    switch (blockTypes)
			//    {
			//        case EMapBlockTypes.NONE:
			//            break;
			//        case EMapBlockTypes.COAST:
			//        case EMapBlockTypes.ETERNAL_SNOW:
			//        case EMapBlockTypes.MOUNT:
			//        case EMapBlockTypes.CITY:
			//        case EMapBlockTypes.FOREST:
			//        case EMapBlockTypes.SHRUBS:
			//        case EMapBlockTypes.SWAMP:
			//        case EMapBlockTypes.GROUND:
			//            terrains.Add(blockTypes, _surface.DefaultEmptySpaces.ToArray());
			//            break;
			//        case EMapBlockTypes.SEA:
			//        case EMapBlockTypes.DEEP_SEA:
			//        case EMapBlockTypes.FRESH_WATER:
			//        case EMapBlockTypes.DEEP_FRESH_WATER:
			//            terrains.Add(blockTypes, new[] { ETerrains.WATER, });
			//            break;
			//        default:
			//            throw new ArgumentOutOfRangeException();
			//    }
			//}

			//var map = new Dictionary<Point, EMapBlockTypes>();

			//foreach (var point in BaseMapBlock.Rect.AllPoints)
			//{
			//    foreach (var center in centers)
			//    {
			//        dists[center.Key] = (center.Value - point).Lenght;
			//    }
			//    var dpoint = dists.OrderBy(_pair => _pair.Value + rnd.NextDouble() * 16).First().Key;
			//    var type = GetBlockType(_blockId + dpoint);
			//    map[point] = type;
			//}

			//var list = new List<EMapBlockTypes>();
			//var baseType = GetBlockType(_blockId);
			//foreach (var point in BaseMapBlock.Rect.AllPoints)
			//{
			//    list.Clear();
			//    var xy = point;
			//    switch (baseType)
			//    {
			//        case EMapBlockTypes.CITY:
			//        case EMapBlockTypes.GROUND:
			//            block.Map[point.X, point.Y] = terrains[baseType][rnd.Next(terrains[baseType].Length)];
			//            break;
			//        case EMapBlockTypes.SEA:
			//            {
			//                list.Add(baseType);
			//                list.Add(baseType);
			//                list.AddRange(from dPoint in Point.NearestDPoints select dPoint + xy into key where map.ContainsKey(key) select map[key]);

			//                var type = list.GroupBy(_types => _types).ToDictionary(_types => _types, _types => _types.Count()).OrderBy(_pair => _pair.Value).First().Key.Key;
			//                if (type != EMapBlockTypes.NONE)
			//                {
			//                    block.Map[point.X, point.Y] = terrains[type][rnd.Next(terrains[type].Length)];
			//                }
			//            }
			//            break;
			//        case EMapBlockTypes.FOREST:
			//            block.Map[point.X, point.Y] = terrains[baseType][rnd.Next(terrains[baseType].Length)];
			//            {
			//                list.AddRange(from dPoint in Point.NearestDPoints select dPoint + xy into key where map.ContainsKey(key) select map[key]);

			//                var type = list.GroupBy(_types => _types).ToDictionary(_types => _types, _types => _types.Count()).OrderBy(_pair => _pair.Value + rnd.Next(5)).First().Key.Key;
			//                if (type != EMapBlockTypes.NONE && rnd.Next(3) == 0)
			//                {
			//                    block.AddObject(m_forestTiles[rnd.Next(m_forestTiles.Count)].GetThing(), point);
			//                }
			//            }
			//            break;
			//        case EMapBlockTypes.NONE:
			//            break;
			//        //default:
			//        //throw new ArgumentOutOfRangeException();
			//    }
			//}


			//if (baseType == EMapBlockTypes.CITY)
			//{
			//    City.GenerateCityBlock(block, rnd, this);
			//}

			//{
			//    var itmcnt = 20 + rnd.Next(rnd.Next(20));
			//    for (var i = 0; i < itmcnt; ++i)
			//    {
			//        var x = rnd.Next(BaseMapBlock.SIZE);
			//        var y = rnd.Next(BaseMapBlock.SIZE);

			//        var attr = TerrainAttribute.GetAttribute(block.Map[x, y]);
			//        if (attr.IsPassable > 0)
			//        {
			//            var point = new Point(x, y);
			//            var any = block.Objects.Where(_tuple => _tuple.Item2 == point).Select(_tuple => _tuple.Item1);
			//            var thing = World.Rnd.Next(2) == 0 ? ThingHelper.GetFakedThing(block) : ThingHelper.GetFakedItem(block.RandomSeed);

			//            if (thing.Is<Stair>())
			//            {
			//                if (x == BaseMapBlock.SIZE - 1 || y == BaseMapBlock.SIZE - 1)
			//                {
			//                    continue;
			//                }
			//            }

			//            if (thing is Item)
			//            {
			//                if (any.Any(_thing => !(_thing is Item)))
			//                {
			//                    continue;
			//                }
			//            }
			//            else if (any.Any())
			//            {
			//                continue;
			//            }

			//            block.AddObject(thing, point);
			//        }
			//    }
			//}
			return block;
		}
	}
}
