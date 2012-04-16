using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Misc;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	public static class SurfaceBlockGenerator
	{
		public static MapBlock GenerateBlock(Point _blockId, Surface _surface)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);

			var pm = new EMapBlockTypes[BaseMapBlock.SIZE,BaseMapBlock.SIZE];
			var baseType = _surface.GetBlockType(_blockId);
					
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
				if (nearestBlockId.X >= 0 && nearestBlockId.X < Surface.WORLD_MAP_SIZE && nearestBlockId.Y >= 0 &&
					nearestBlockId.Y < Surface.WORLD_MAP_SIZE)
				{
					type = _surface.GetBlockType(nearestBlockId);
				}
				toAdd.Add(type);

				if (_surface.Blocks.ContainsKey(nearestBlockId))
				{
					var nearestBlock = _surface.Blocks[nearestBlockId];
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
			
			return block;
		}
	}
}
