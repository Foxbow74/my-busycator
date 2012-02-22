using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects.Furniture;
using Point = GameCore.Misc.Point;

namespace GameCore.Mapping.Layers
{
	internal class DungeonLayer : WorldLayer
	{
		public DungeonLayer(WorldLayer _enterFromLayer, Point _enterCoords, Stair _stair)
		{
			EnterCoords = _enterCoords;
			var blockId = MapBlock.GetBlockCoords(_enterCoords);
			var inBlockCoords = MapBlock.GetInBlockCoords(_enterCoords);
			var block = this[blockId];

			if (_stair is StairUp)
			{
				block.AddObject(new StairDown(_enterFromLayer), inBlockCoords);
			}
			else
			{
				block.AddObject(new StairUp(_enterFromLayer), inBlockCoords);
			}
		}

		internal override IEnumerable<ETerrains> DefaultEmptySpaces
		{
			get { yield return ETerrains.STONE_FLOOR; }
		}


		internal override IEnumerable<ETerrains> DefaultWalls
		{
			get { yield return ETerrains.STONE_WALL; }
		}

		protected override MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);
			MapBlockHelper.Clear(block, rnd, this, DefaultEmptySpaces);

			const int v = MapBlock.SIZE / 2 - 2;

			for (var i = 0; i < v;++i )
			{
				block.Map[i, 0] = ETerrains.STONE_WALL;
				block.Map[0, i] = ETerrains.STONE_WALL;
				block.Map[MapBlock.SIZE - 1 - i, 0] = ETerrains.STONE_WALL;
				block.Map[0, MapBlock.SIZE - 1 - i] = ETerrains.STONE_WALL;
			}
			//block.Map[1,1] = ETerrains.MUSHROOM;

			block.AddLightSource(new Point(2, 2), new LightSource(18, new FColor(5f, 1f, 0, 0)));
			block.AddLightSource(new Point(MapBlock.SIZE - 2, MapBlock.SIZE - 2), new LightSource(8, new FColor(1f, 0f, 1f, 0f)));
			block.AddLightSource(new Point(MapBlock.SIZE - 2, 2), new LightSource(8, new FColor(1f, 0f, 0f, 1f)));
			block.AddLightSource(new Point(2, MapBlock.SIZE - 2), new LightSource(8, new FColor(1f, 1f, 0f, 1f)));

			{
				var x = rnd.Next(MapBlock.SIZE);
				var y = rnd.Next(MapBlock.SIZE);
				block.AddCreature(new Monster(this), new Point(x, y));
			}

			block.Map[9, 9] = ETerrains.BRICK_WALL;
			block.Map[10, 9] = ETerrains.WINDOW;
			block.Map[11, 9] = ETerrains.BRICK_WALL;
			block.Map[11, 10] = ETerrains.WINDOW;
			block.Map[9, 10] = ETerrains.WINDOW;
			block.Map[9, 11] = ETerrains.BRICK_WALL;
			block.AddObject(new Door(), new Point(10, 11));
			block.Map[11, 11] = ETerrains.BRICK_WALL;
			//block.AddLightSource(new Point(10, 10), new LightSource(18, new FColor(53f, 0f, 1f, 1f)));

			return block;
		}

		public override FColor Ambient
		{
			get
			{
				//return new FColor(Color.FromArgb(255, 50, 255, 50)); 
				return new FColor(0, 0, 0.01f, 0.01f);
			}
		}

		public Point EnterCoords { get; private set; }
	}
}