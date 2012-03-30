using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	public static class SurfaceBlockGenerator
	{
		public static MapBlock GenerateBlock(Point _blockId, Surface _surface)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);
			var mp = _surface.WorldMap;

			var pm = new EMapBlockTypes[BaseMapBlock.SIZE,BaseMapBlock.SIZE];

			foreach (var dir in Util.AllDirections)
			{
				Point from;
				Point to;
				switch (dir)
				{
					case EDirections.UP:
						from = Point.Zero;
						to = new Point(BaseMapBlock.SIZE-2,0);
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
				if (_surface.Blocks.ContainsKey(nearestBlockId))
				{
					var nearestBlock = _surface.Blocks[nearestBlockId];
					foreach (var point in from.GetLineToPoints(to))
					{
						pm[point.X, point.Y] = ;
					}
				}
				else
				{
					var type = _surface.WorldMap[nearestBlockId.X, nearestBlockId.Y];
					foreach (var point in from.GetLineToPoints(to))
					{
						pm[point.X,point.Y] = type;
					}
				}
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
