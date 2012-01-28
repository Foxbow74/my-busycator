#region

using System;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;

#endregion

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
						_block.Objects.Add(new Tuple<Thing, Point>(ThingHelper.GetFaketThing(_block), new Point(x, y)));
					}
				}
			}

			//if(rnd.NextDouble()<)
			{
				var x = rnd.Next(MapBlock.SIZE);
				var y = rnd.Next(MapBlock.SIZE);
				_block.Creatures.Add(new Monster(new Point(_blockId.X*MapBlock.SIZE + x, _blockId.Y*MapBlock.SIZE + y)));
			}
		}
	}
}