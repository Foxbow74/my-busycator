using System;
using System.Collections.Generic;
using GameCore.Creatures;
using Point = GameCore.Misc.Point;

namespace GameCore.Mapping.Layers
{
	public abstract class WorldLayer
	{
		/// <summary>
		/// 	Сколько блоков вокруг игрока считаются активными и отображаются на карте
		/// </summary>
		public const int ACTIVE_SIZE_HALF = 3;

		private readonly Dictionary<Point, MapBlock> m_blocks = new Dictionary<Point, MapBlock>();

		public MapBlock this[Point _blockId]
		{
			get
			{
				MapBlock block;
				if (!m_blocks.TryGetValue(_blockId, out block))
				{
					block = GenerateBlock(_blockId);
					m_blocks.Add(_blockId, block);
				}
				return block;
			}
		}

		internal abstract IEnumerable<ETerrains> DefaultEmptyTerrains { get; }

		protected abstract MapBlock GenerateBlock(Point _blockId);

		public void MoveCreature(Creature _creature, Point _fromBlock, Point _toBlock)
		{
			if (!_creature.IsAvatar)
			{
				this[_fromBlock].Creatures.Remove(_creature);
				this[_toBlock].Creatures.Add(_creature);
			}
		}

		public void RemoveCreature(Creature _creature)
		{
			if (!_creature.IsAvatar) _creature.MapBlock.Creatures.Remove(_creature);
		}

		public void AddCreature(Creature _creature)
		{
			if (!_creature.IsAvatar) _creature.MapBlock.Creatures.Add(_creature);
		}

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

		/// <summary>
		/// 	Заполняет двумерный массив значениями из карты вокруг игрока
		/// </summary>
		/// <param name = "_mapCells"></param>
		/// <param name = "_avatarPoint"></param>
		public void SetData(MapCell[,] _mapCells, Point _avatarPoint)
		{
			var w = _mapCells.GetLength(0);
			var h = _mapCells.GetLength(1);

			var avatar = new Point(_avatarPoint.X - w/2, _avatarPoint.Y - h/2);

			foreach (var tuple in GetBlocksNear(_avatarPoint))
			{
				var block = tuple.Item2;
				var blockId = tuple.Item1;

				var blockPoint = new Point(blockId.X*MapBlock.SIZE, blockId.Y*MapBlock.SIZE);

				for (var i = 0; i < MapBlock.SIZE; i++)
				{
					for (var j = 0; j < MapBlock.SIZE; j++)
					{
						var ij = new Point(i, j);

						var world = blockPoint + ij;

						var map = world - avatar;

						if (map.X < 0 || map.Y < 0 || map.X >= w || map.Y >= h)
						{
							continue;
						}
						var mc = new MapCell(block, ij, world);
						_mapCells[map.X, map.Y] = mc;
					}
				}
			}
		}

		public abstract FColor Ambient { get; }

		public MapBlock GetMapBlock(Point _worldCoords)
		{
			var blockCoords = MapBlock.GetBlockCoords(_worldCoords);
			var block = this[blockCoords];
			return block;
		}

		public MapCell GetMapCell(Point _worldCoords)
		{
			var blockCoords = MapBlock.GetBlockCoords(_worldCoords);
			var block = this[blockCoords];
			var coords = MapBlock.GetInBlockCoords(_worldCoords);
			return new MapCell(block, coords, _worldCoords);
		}
	}
}