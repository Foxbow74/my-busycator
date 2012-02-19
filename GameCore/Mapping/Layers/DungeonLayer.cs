using System;
using System.Collections.Generic;
using System.Drawing;
using GameCore.Misc;
using GameCore.Objects.Furniture;
using Point = GameCore.Misc.Point;

namespace GameCore.Mapping.Layers
{
	internal class DungeonLayer : WorldLayer
	{
		public DungeonLayer(WorldLayer _enterFromLayer, Point _enterCoords, Stair _stair)
		{
			var blockId = MapBlock.GetBlockCoords(_enterCoords);
			var inBlockCoords = MapBlock.GetInBlockCoords(_enterCoords);
			var block = this[blockId];

			if (_stair is StairUp)
			{
				block.AddObject(inBlockCoords, new StairDown(_enterFromLayer));
			}
			else
			{
				block.AddObject(inBlockCoords, new StairUp(_enterFromLayer));
			}
		}

		internal override IEnumerable<ETerrains> DefaultEmptyTerrains
		{
			get { yield return ETerrains.STONE_FLOOR; }
		}

		protected override MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);
			MapBlockHelper.Clear(block, rnd, this);

			const int v = MapBlock.SIZE / 2 - 2;

			for (var i = 0; i < v;++i )
			{
				block.Map[i, 0] = ETerrains.STONE_WALL;
				block.Map[0, i] = ETerrains.STONE_WALL;
				block.Map[MapBlock.SIZE - 1 - i, 0] = ETerrains.STONE_WALL;
				block.Map[0, MapBlock.SIZE - 1 - i] = ETerrains.STONE_WALL;
			}
			block.Map[1,1] = ETerrains.MUSHROOM;
			//block.AddLightSource(new Point(2, 2), new LightSource(8, new FColor(5f, 1f, 1f, 1f)));
			//block.AddLightSource(new Point(MapBlock.SIZE - 2, MapBlock.SIZE - 2), new LightSource(8, new FColor(3f, 0f, 1f, 0f)));
			//block.AddLightSource(new Point(MapBlock.SIZE - 2, 2), new LightSource(8, new FColor(3f, 0f, 0f, 1f)));
			//block.AddLightSource(new Point(2, MapBlock.SIZE - 2), new LightSource(8, new FColor(3f, 1f, 0f, 1f)));
			return block;
		}

		public override FColor Ambient
		{
			get
			{
				return new FColor(Color.FromArgb(255, 255, 255, 40)); 
				//return new FColor(0, 0, 0.01f, 0.01f);
			}
		}
	}
}