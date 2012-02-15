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
			block.LightSources.Add(new LightSource(new Point(1, 1) + _blockId * MapBlock.SIZE, 9, new FColor(Color.Red)));
			block.LightSources.Add(new LightSource(new Point(MapBlock.SIZE - 1, MapBlock.SIZE - 1) + _blockId * MapBlock.SIZE, 9, new FColor(Color.Green)));
			block.LightSources.Add(new LightSource(new Point(MapBlock.SIZE - 1, 1) + _blockId * MapBlock.SIZE, 9, new FColor(Color.Blue)));
			block.LightSources.Add(new LightSource(new Point(1, MapBlock.SIZE - 1) + _blockId * MapBlock.SIZE, 9, new FColor(Color.Magenta)));
			return block;
		}

		public override FColor Ambient
		{
			get { return new FColor(1f, 0, 0.01f, 0.1f); }
		}

		public override FColor Lighted
		{
			get { return new FColor(Color.FromArgb(255, 0, 0, 0)); }
		}
	}
}