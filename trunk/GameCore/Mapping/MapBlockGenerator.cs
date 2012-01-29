using System;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Mapping
{
	internal static class MapBlockGenerator
	{
		public static void Generate(MapBlock _block, Point _blockId, World _world)
		{
			var rnd = new Random(_block.RandomSeed);

			for (var i = 0; i < MapBlock.SIZE; ++i)
			{
				for (var j = 0; j < MapBlock.SIZE; ++j)
				{
					_block.Map[i, j] = rnd.Next(3) == 0 ? ETerrains.GRASS : ETerrains.GROUND;
				}
			}

			for (var i = 3; i <= 8; i++)
			{
				_block.Map[i, 3] = ETerrains.BRICK_WALL;
				_block.Map[3, i] = ETerrains.BRICK_WALL;
				_block.Map[i, 8] = ETerrains.BRICK_WALL;
				_block.Map[8, i] = ETerrains.BRICK_WALL;
			}

			_block.Map[6, 3] = ETerrains.WINDOW;
			_block.Map[3, 6] = ETerrains.WINDOW;

			{
				var cnt = rnd.Next(rnd.Next(70));
				for (var i = 0; i < cnt; ++i)
				{
					var x = rnd.Next(MapBlock.SIZE);
					var y = rnd.Next(MapBlock.SIZE);
					var attr = TerrainAttribute.GetAttribute(_block.Map[x, y]);
					if (attr.IsPassable > 0)
					{
						_block.Map[x, y] = ETerrains.MUSHROOM;
					}
				}
			}

			{
				var itmcnt = rnd.Next(rnd.Next(20));
				for (var i = 0; i < itmcnt; ++i)
				{
					var x = rnd.Next(MapBlock.SIZE);
					var y = rnd.Next(MapBlock.SIZE);

					var attr = TerrainAttribute.GetAttribute(_block.Map[x, y]);
					if (attr.IsPassable > 0)
					{
						var point = new Point(x, y);
						var any = _block.Objects.Where(_tuple => _tuple.Item2 == point).Select(_tuple => _tuple.Item1);
						var thig = World.Rnd.Next(2) == 0 ? ThingHelper.GetFaketThing(_block) : ThingHelper.GetFaketItem(_block);
						if (thig is Item)
						{
							if (any.Any(_thing => !(thig is Item)))
							{
								continue;
							}
						}
						else if (any.Any())
						{
							continue;
						}
						_block.Objects.Add(new Tuple<Thing, Point>(thig, point));
					}
				}
			}

			{
				var x = rnd.Next(MapBlock.SIZE);
				var y = rnd.Next(MapBlock.SIZE);
				_block.Creatures.Add(new Monster(new Point(_blockId.X*MapBlock.SIZE + x, _blockId.Y*MapBlock.SIZE + y)));
			}
		}
	}
}