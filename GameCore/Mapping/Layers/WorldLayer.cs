using System;
using System.Collections.Generic;
using GameCore.Essences.Things;
using GameCore.Misc;

namespace GameCore.Mapping.Layers
{
	public abstract class WorldLayer
	{
		protected WorldLayer()
		{
			Blocks = new Dictionary<Point, MapBlock>();
			FogColor = FColor.FromArgb(255, 60, 60, 60);
			FogLightness = FogColor.Lightness()/3;
		}

		public FColor FogColor { get; protected set; }
		public float FogLightness { get; protected set; }

		public MapBlock this[Point _blockId]
		{
			get
			{
				MapBlock block;
				if (!Blocks.TryGetValue(_blockId, out block))
				{
					block = GenerateBlock(_blockId);
					Blocks[_blockId] = block;
				}
				return block;
			}
		}

        public abstract IEnumerable<ETerrains> DefaultEmptySpaces { get; }
        public abstract IEnumerable<ETerrains> DefaultWalls { get; }

        public FColor Ambient { get; set; }

		public Dictionary<Point, MapBlock> Blocks { get; private set; }
	    public abstract Point GetAvatarStartingBlockId();

	    public virtual float GetFogColorMultiplier(LiveMapCell _liveCell) { return _liveCell.FogColorMultiplier; }
		protected abstract MapBlock GenerateBlock(Point _blockId);

		public virtual void AddStair(WorldLayer _enterFromLayer, Point _worldCoords, Stair _stair) { throw new NotImplementedException(); }

	    public abstract EMapBlockTypes GetBlockType(Point _blockId);
	}
}