using System;
using System.Collections.Generic;
using GameCore;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
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
            if (_blockId.Y == 0)
            {
                block.Clear(World.Rnd, this, DefaultWalls);
                block.Fill(World.Rnd, this, DefaultEmptySpaces, new Rct(0, 0, Constants.MAP_BLOCK_SIZE, 13));
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
