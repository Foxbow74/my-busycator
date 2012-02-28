using System;
using System.Collections.Generic;
using Point = GameCore.Misc.Point;

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
		}

		public virtual float GetFogColorMultiplier(LiveMapCell _liveCell)
		{
			return 0.5f;
		}

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

		protected abstract MapBlock GenerateBlock(Point _blockId);

		public IEnumerable<Tuple<Point, MapBlock>> GetBlocksNear(Point _worldCoords)
		{
			var centralBlockCoord = MapBlock.GetBlockCoords(_worldCoords);
			for (var i = -ACTIVE_SIZE_HALF; i < ACTIVE_SIZE_HALF; ++i)
			{
				for (var j = -ACTIVE_SIZE_HALF; j < ACTIVE_SIZE_HALF; ++j)
				{
					var blockId = new Point(centralBlockCoord.X + i, centralBlockCoord.Y + j);
					yield return new Tuple<Point, MapBlock>(blockId, this[blockId]);
				}
			}
		}

		public abstract FColor Ambient { get; }

		public Dictionary<Point, MapBlock> Blocks { get; private set; }
	}
}