using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Materials;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture;
using GameCore.Objects.Furniture.LightSources;

namespace GameCore.Mapping.Layers
{
	internal class DungeonLayer : WorldLayer
	{
		public DungeonLayer(WorldLayer _enterFromLayer, Point _enterCoords, Stair _stair)
		{
			EnterCoords = _enterCoords;
			var blockId = MapBlock.GetBlockId(_enterCoords);
			var inBlockCoords = MapBlock.GetInBlockCoords(_enterCoords);
			var block = this[blockId];

			if (_stair is StairUp)
			{
				block.AddObject(new StairDown(_enterFromLayer, ThingHelper.GetMaterial<StoneMaterial>()), inBlockCoords);
			}
			else
			{
				block.AddObject(new StairUp(_enterFromLayer, ThingHelper.GetMaterial<StoneMaterial>()), inBlockCoords);
			}

			FogColor = FColor.FromArgb(255, 100, 100, 100);
			FogLightness = FogColor.Lightness();
		}

		public override float GetFogColorMultiplier(LiveMapCell _liveCell)
		{
			return _liveCell.DungeonFogColorMultiplier;
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

			block.AddObject(new OnWallTorch(new LightSource(8, new FColor(5f, 1f, 0, 0)), EDirections.DOWN, ThingHelper.GetMaterial<OakMaterial>()), new Point(1, 1));
			block.AddObject(new OnWallTorch(new LightSource(8, new FColor(1f, 0f, 1f, 0f)), EDirections.UP, ThingHelper.GetMaterial<OakMaterial>()), new Point(MapBlock.SIZE - 1, MapBlock.SIZE - 1));
			block.AddObject(new OnWallTorch(new LightSource(8, new FColor(1f, 0f, 0f, 1f)), EDirections.RIGHT, ThingHelper.GetMaterial<OakMaterial>()), new Point(MapBlock.SIZE - 1, 1));
			block.AddObject(new OnWallTorch(new LightSource(8, new FColor(1f, 1f, 0f, 1f)), EDirections.LEFT, ThingHelper.GetMaterial<OakMaterial>()), new Point(1, MapBlock.SIZE - 1));

			{
				var x = rnd.Next(MapBlock.SIZE);
				var y = rnd.Next(MapBlock.SIZE);
				block.AddCreature(new Monster(this), new Point(x, y));
			}

			block.Map[9, 9] = ETerrains.RED_BRICK_WALL;
			block.Map[10, 9] = ETerrains.RED_BRICK_WINDOW;
			block.Map[11, 9] = ETerrains.RED_BRICK_WALL;
			block.Map[11, 10] = ETerrains.RED_BRICK_WINDOW;
			block.Map[9, 10] = ETerrains.RED_BRICK_WINDOW;
			block.Map[9, 11] = ETerrains.RED_BRICK_WALL;
			block.AddObject(new Door(null), new Point(10, 11));
			block.Map[11, 11] = ETerrains.RED_BRICK_WALL;
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