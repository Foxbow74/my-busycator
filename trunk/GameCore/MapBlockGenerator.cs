using System;
using GameCore.Objects;
using Graphics;
using Object = GameCore.Objects.Object;

namespace GameCore
{
	static class MapBlockGenerator
	{
		public static void Generate(MapBlock _block, Point _blockId)
		{
			var rnd = new Random(_block.RandomSeed);

			for (var i = 0; i < MapBlock.SIZE; ++i)
			{
				for (int j = 0; j < MapBlock.SIZE; ++j)
				{
					_block.Map[i, j] = rnd.Next(3) == 0 ? ETerrains.GRASS : ETerrains.GROUND;
				}
			}

			for (int i = 3; i < 17; i++)
			{
				_block.Map[i, 10] = ETerrains.MUSHROOM;
				_block.Map[10, i] = ETerrains.MUSHROOM;
			}


			var cnt = rnd.Next(rnd.Next(40));
			for (var i = 0; i < cnt; ++i)
			{
				var x = rnd.Next(MapBlock.SIZE);
				var y = rnd.Next(MapBlock.SIZE);
				_block.Map[x, y] = ETerrains.MUSHROOM;
			}

			var itmcnt = rnd.Next(rnd.Next(5));
			for (var i = 0; i < itmcnt; ++i)
			{
				var x = rnd.Next(MapBlock.SIZE);
				var y = rnd.Next(MapBlock.SIZE);
				if (_block.Map[x, y].IsPassable())
				{
					_block.Objects.Add(new Tuple<Object, Point>(GenerateFakeItem(rnd), new Point(x, y)));
				}
			}
		}

		public static FakeItem GenerateFakeItem(Random _random)
		{
			switch (_random.Next(3))
			{
				case 0: return FakeItems.Weapon;
				case 1: return FakeItems.Chest;
				case 2: return FakeItems.Door;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}