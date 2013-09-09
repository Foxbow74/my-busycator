using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using GameCore;
using GameCore.Essences;
using GameCore.Essences.Things.LightSources;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Materials;
using GameCore.Misc;

namespace TheHall.Layers
{
    class TheHallLayer:WorldLayer
    {
        public override IEnumerable<ETerrains> DefaultEmptySpaces
        {
            get { yield return ETerrains.STONE_FLOOR; }
        }

        public override IEnumerable<ETerrains> DefaultWalls
        {
            get { yield return ETerrains.STONE_WALL; }
        }

        public override Point GetAvatarStartingBlockId()
        {
            return Point.Zero;
        }

        protected override MapBlock GenerateBlock(Point _blockId)
        {
            var block = new MapBlock(_blockId);
			
            if (_blockId.Y == 0 && _blockId.X == 0)
            {
                block.Clear(World.Rnd, this, DefaultEmptySpaces);
				//block.Fill(World.Rnd, this, DefaultEmptySpaces, new Rct(0, 0, Constants.MAP_BLOCK_SIZE, 13));

				block.AddEssence(new IndoorLight(new LightSource(18, new FColor(1f, 1f, 1f, 1f)), EssenceHelper.GetFirstFoundedMaterial<MetalMaterial>()), new Point(2, 0));
                block.Map[3, 0] = ETerrains.RED_BRICK_WINDOW;
				//block.AddEssence(new IndoorLight(new LightSource(18, new FColor(1f, 1f, 1f, 0f)), EssenceHelper.GetFirstFoundedMaterial<MetalMaterial>()), new Point(10, 10));

				//block.AddEssence(new IndoorLight(new LightSource(18, new FColor(1f, 0f, 1f, 0f)), EssenceHelper.GetFirstFoundedMaterial<MetalMaterial>()), new Point(15, 0));
				//block.Map[1, 2] = ETerrains.STONE_WALL;
				//block.Map[2, 2] = ETerrains.STONE_WALL;
                //block.Map[3, 2] = ETerrains.STONE_WALL;

				//block.Map[0, 3] = ETerrains.STONE_WALL;
				//block.Map[0, 4] = ETerrains.STONE_WALL;
				//block.Map[0, 5] = ETerrains.STONE_WALL;
            }
            else
            {
                block.Clear(World.Rnd, this, DefaultWalls);
            }
            return block;
        }

        public override EMapBlockTypes GetBlockType(Point _blockId)
        {
            throw new NotImplementedException();
        }
    }
}
