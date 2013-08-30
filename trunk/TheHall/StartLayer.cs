using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Misc;

namespace TheHall
{
    class StartLayer:WorldLayer
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
            block.Clear(World.Rnd, this, DefaultEmptySpaces);
            return block;
        }

        public override EMapBlockTypes GetBlockType(Point _blockId)
        {
            throw new NotImplementedException();
        }
    }
}
