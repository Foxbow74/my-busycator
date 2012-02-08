using System;
using GameCore.Misc;
using GameCore.Objects.Furniture;

namespace GameCore.Mapping.Layers
{
	class DungeonLayer:WorldLayer
	{
		private readonly WorldLayer m_enterFromLayer;
		private readonly Point m_enterCoords;
		private readonly Stair m_stair;

		public DungeonLayer(WorldLayer _enterFromLayer, Point _enterCoords, Stair _stair)
		{
			m_enterFromLayer = _enterFromLayer;
			m_enterCoords = _enterCoords;
			m_stair = _stair;

			var blockId = MapBlock.GetBlockCoords(_enterCoords);
			var inBlockCoords = MapBlock.GetInBlockCoords(_enterCoords);
			var block = this[blockId];
			//block.Map[inBlockCoords.X, inBlockCoords.Y].
			block.AddObject(inBlockCoords, new StairUp(this));
		}

		internal override System.Collections.Generic.IEnumerable<ETerrains> DefaultEmptyTerrains
		{
			get
			{
				yield return ETerrains.STONE_FLOOR;
			}
		}

		protected override MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);
			MapBlockGenerator.Clear(block, rnd, this);
			return block;
		}
	}
}
