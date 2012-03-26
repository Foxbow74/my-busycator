using System;
using System.Collections.Generic;
using GameCore.Misc;
using GameCore.Objects.Furniture;

namespace GameCore.Mapping.Layers
{
	public abstract class WorldLayer
	{
		/// <summary>
		/// 	Сколько блоков вокруг игрока считаются активными и отображаются на карте
		/// </summary>
		public const int ACTIVE_SIZE_HALF = 3;

		protected WorldLayer()
		{
			Blocks = new Dictionary<Point, MapBlock>();
			FogColor = FColor.FromArgb(255, 60, 60, 60);
			FogLightness = FogColor.Lightness();
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

		internal abstract IEnumerable<ETerrains> DefaultEmptySpaces { get; }
		internal abstract IEnumerable<ETerrains> DefaultWalls { get; }

		public abstract FColor Ambient { get; }

		public Dictionary<Point, MapBlock> Blocks { get; private set; }
		public virtual float GetFogColorMultiplier(LiveMapCell _liveCell) { return _liveCell.FogColorMultiplier; }
		protected abstract MapBlock GenerateBlock(Point _blockId);

		public IEnumerable<Tuple<Point, MapBlock>> GetBlocksNear(Point _worldCoords)
		{
			var centralBlockCoord = BaseMapBlock.GetBlockId(_worldCoords);
			for (var i = -ACTIVE_SIZE_HALF; i < ACTIVE_SIZE_HALF; ++i)
			{
				for (var j = -ACTIVE_SIZE_HALF; j < ACTIVE_SIZE_HALF; ++j)
				{
					var blockId = new Point(centralBlockCoord.X + i, centralBlockCoord.Y + j);
					yield return new Tuple<Point, MapBlock>(blockId, this[blockId]);
				}
			}
		}

		public virtual void AddStair(WorldLayer _enterFromLayer, Point _worldCoords, Stair _stair) { throw new NotImplementedException(); }
	}
}