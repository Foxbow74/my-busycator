using System;
using System.Collections.Generic;
using Busycator.Layers;
using GameCore;
using GameCore.Mapping;
using GameCore.Misc;

namespace Busycator
{
    public static class SurfaceBlockGenerator
    {
        public static MapBlock GenerateBlock(Point _blockId, Surface _surface)
        {
            var block = new MapBlock(_blockId);
            var baseType = _surface.GetBlockType(_blockId);
            if (baseType == EMapBlockTypes.NONE)
            {
                return block;
            }

            var rnd = new Random(block.RandomSeed);
            var pm = new EMapBlockTypes[Constants.MAP_BLOCK_SIZE, Constants.MAP_BLOCK_SIZE];

            var points = Constants.MAP_BLOCK_SIZE * Constants.MAP_BLOCK_SIZE;
            var toAdd = new List<EMapBlockTypes> { baseType, baseType, baseType };

            #region размытие границ с соседними блоками

            foreach (var dir in Util.AllDirections)
            {
                Point from;
                Point to;
                switch (dir)
                {
                    case EDirections.UP:
                        from = Point.Zero;
                        to = new Point(Constants.MAP_BLOCK_SIZE - 2, 0);
                        break;
                    case EDirections.DOWN:
                        from = new Point(1, Constants.MAP_BLOCK_SIZE - 1);
                        to = new Point(Constants.MAP_BLOCK_SIZE - 1, Constants.MAP_BLOCK_SIZE - 1);
                        break;
                    case EDirections.LEFT:
                        from = new Point(0, Constants.MAP_BLOCK_SIZE - 1);
                        to = new Point(0, 1);
                        break;
                    case EDirections.RIGHT:
                        from = new Point(Constants.MAP_BLOCK_SIZE - 1, 0);
                        to = new Point(Constants.MAP_BLOCK_SIZE - 1, Constants.MAP_BLOCK_SIZE - 2);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var delta = dir.GetDelta();
                var nearestBlockId = _blockId + delta;

                var type = _surface.GetBlockType(nearestBlockId);
                if (type == EMapBlockTypes.NONE)
                {
                    type = baseType;
                }

                toAdd.Add(type);

                if (_surface.Blocks.ContainsKey(nearestBlockId))
                {
                    var nearestBlock = _surface.Blocks[nearestBlockId];
                    foreach (var point in from.GetLineToPoints(to))
                    {
                        type =
                            TerrainAttribute.GetMapBlockType(
                                nearestBlock.Map[
                                    (point.X + delta.X + Constants.MAP_BLOCK_SIZE) % Constants.MAP_BLOCK_SIZE,
                                    (point.Y + delta.Y + Constants.MAP_BLOCK_SIZE) % Constants.MAP_BLOCK_SIZE]);
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

            #endregion

            foreach (var t in toAdd)
            {
                var pnt = Point.Zero;
                while (pm[pnt.X, pnt.Y] != EMapBlockTypes.NONE)
                {
                    pnt = new Point(1 + rnd.Next(Constants.MAP_BLOCK_SIZE - 2), 1 + rnd.Next(Constants.MAP_BLOCK_SIZE - 2));
                }
                pm[pnt.X, pnt.Y] = t;
                points--;
            }

            var dpoints = Util.AllDeltas;
            while (points > 0)
            {
                foreach (var point in Point.AllBlockPoints)
                {
                    var xy = pm[point.X, point.Y];
                    if (xy == 0) continue;

                    var dpoint = dpoints[rnd.Next(4)];
                    var x1 = point.X + dpoint.X;
                    if (x1 < 0 || x1 == Constants.MAP_BLOCK_SIZE) continue;
                    var y1 = point.Y + dpoint.Y;
                    if (y1 < 0 || y1 == Constants.MAP_BLOCK_SIZE) continue;
                    var xy1 = pm[x1, y1];
                    if (xy1 == 0)
                    {
                        pm[x1, y1] = xy;
                        points--;
                    }
                }

            }

            #region заполнение карты блока

            foreach (var pnt in new Rct(0, 0, Constants.MAP_BLOCK_SIZE, Constants.MAP_BLOCK_SIZE).AllPoints)
            {
                ETerrains tr;
                switch (pm[pnt.X, pnt.Y])
                {
                    case EMapBlockTypes.NONE:
                        continue;
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
                    case EMapBlockTypes.LAKE_COAST:
                        tr = ETerrains.LAKE_COAST;
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

            #endregion

            return block;
        }
    }
}