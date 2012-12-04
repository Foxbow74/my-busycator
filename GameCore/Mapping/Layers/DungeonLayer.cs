using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Essences.Things;
using GameCore.Essences.Things.LightSources;
using GameCore.Materials;
using GameCore.Misc;

namespace GameCore.Mapping.Layers
{
	internal class DungeonLayer : WorldLayer
	{
		public DungeonLayer(Point _enterCoords)
		{
		    Ambient = new FColor(0, 0, 0.01f, 0.01f);
			EnterCoords = _enterCoords;
			FogColor = FColor.FromArgb(255, 100, 100, 100);
			FogLightness = FogColor.Lightness();
		}

		internal override IEnumerable<ETerrains> DefaultEmptySpaces { get { yield return ETerrains.STONE_FLOOR; } }


		internal override IEnumerable<ETerrains> DefaultWalls { get { yield return ETerrains.STONE_WALL; } }

		public Point EnterCoords { get; private set; }

		public override void AddStair(WorldLayer _enterFromLayer, Point _worldCoords, Stair _stair)
		{
			var blockId = BaseMapBlock.GetBlockId(EnterCoords);
			var inBlockCoords = BaseMapBlock.GetInBlockCoords(EnterCoords);
			var block = this[blockId];

			if (_stair is StairUp)
			{
				block.AddEssence(new StairDown(_enterFromLayer, EssenceHelper.GetFirstFoundedMaterial<MineralMaterial>()), inBlockCoords);
			}
			else
			{
				block.AddEssence(new StairUp(_enterFromLayer, EssenceHelper.GetFirstFoundedMaterial<MineralMaterial>()), inBlockCoords);
			}
		}

		public override float GetFogColorMultiplier(LiveMapCell _liveCell) { return _liveCell.DungeonFogColorMultiplier; }

		protected override MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);
			MapBlockHelper.Clear(block, rnd, this, DefaultEmptySpaces);

			const int v = Constants.MAP_BLOCK_SIZE/2 - 2;

			for (var i = 0; i < v; ++i)
			{
				block.Map[i, 0] = ETerrains.STONE_WALL;
				block.Map[0, i] = ETerrains.STONE_WALL;
				block.Map[Constants.MAP_BLOCK_SIZE - 1 - i, 0] = ETerrains.STONE_WALL;
				block.Map[0, Constants.MAP_BLOCK_SIZE - 1 - i] = ETerrains.STONE_WALL;
			}

			block.AddEssence(new OnWallTorch(new LightSource(8, new FColor(5f, 1f, 0, 0)), EDirections.DOWN, EssenceHelper.GetFirstFoundedMaterial<WoodMaterial>()), new Point(1, 1));
			block.AddEssence(new OnWallTorch(new LightSource(8, new FColor(1f, 0f, 1f, 0f)), EDirections.UP, EssenceHelper.GetFirstFoundedMaterial<WoodMaterial>()), new Point(Constants.MAP_BLOCK_SIZE - 1, Constants.MAP_BLOCK_SIZE - 1));
			block.AddEssence(new OnWallTorch(new LightSource(8, new FColor(1f, 0f, 0f, 1f)), EDirections.RIGHT, EssenceHelper.GetFirstFoundedMaterial<WoodMaterial>()), new Point(Constants.MAP_BLOCK_SIZE - 1, 1));
			block.AddEssence(new OnWallTorch(new LightSource(8, new FColor(1f, 1f, 0f, 1f)), EDirections.LEFT, EssenceHelper.GetFirstFoundedMaterial<WoodMaterial>()), new Point(1, Constants.MAP_BLOCK_SIZE - 1));

			{
				var x = rnd.Next(Constants.MAP_BLOCK_SIZE);
				var y = rnd.Next(Constants.MAP_BLOCK_SIZE);
				block.Creatures.Add(EssenceHelper.GetFirstFoundedCreature<AbstractMonster>(), new Point(x, y));
			}

			block.Map[9, 9] = ETerrains.RED_BRICK_WALL;
			block.Map[10, 9] = ETerrains.RED_BRICK_WINDOW;
			block.Map[11, 9] = ETerrains.RED_BRICK_WALL;
			block.Map[11, 10] = ETerrains.RED_BRICK_WINDOW;
			block.Map[9, 10] = ETerrains.RED_BRICK_WINDOW;
			block.Map[9, 11] = ETerrains.RED_BRICK_WALL;
			block.AddEssence(new ClosedDoor(null), new Point(10, 11));
			block.Map[11, 11] = ETerrains.RED_BRICK_WALL;
			//block.AddLightSource(new Point(10, 10), new LightSource(18, new FColor(53f, 0f, 1f, 1f)));

			return block;
		}
	}
}