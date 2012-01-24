using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameCore.Creatures;
using Graphics;

namespace GameCore
{
	public class Map
	{
		private readonly World m_world;
		const int ACTIVE_SIZE_HALF = 2;

		readonly Dictionary<Point, MapBlock> m_blocks = new Dictionary<Point, MapBlock>();

		public Map(World _world)
		{
			m_world = _world;
		}

		public IEnumerable<Tuple<Point, MapBlock>> GetBlocksNear(Point _point)
		{
			var centralBlockCoord = MapBlock.GetBlockCoords(_point);
			for (var i = -ACTIVE_SIZE_HALF; i < ACTIVE_SIZE_HALF; ++i)
			{
				for (var j = -ACTIVE_SIZE_HALF; j < ACTIVE_SIZE_HALF; ++j)
				{
					var blockId = new Point(centralBlockCoord.X + i, centralBlockCoord.Y + j);
					yield return new Tuple<Point, MapBlock>(blockId, this[blockId]);
				}
			}
		}

		public MapBlock this[Point _blockId]
		{
			get
			{
				MapBlock block;
				if (!m_blocks.TryGetValue(_blockId, out block))
				{
					block = GenerateBlock(_blockId, m_world);
					m_blocks.Add(_blockId, block);
					Debug.WriteLine("Generated new in " + _blockId);
				}
				return block;
			}
		}

		private static MapBlock GenerateBlock(Point _blockId, World _world)
		{
			var block = new MapBlock(_blockId);
			MapBlockGenerator.Generate(block, _blockId, _world);
			return block;
		}

		/// <summary>
		/// Заполняет двумерный массив значениями из карты вокруг игрока
		/// </summary>
		/// <param name="_mapTiles"></param>
		/// <param name="_avatarPoint"></param>
		public void SetData(MapCell[,] _mapTiles, Point _avatarPoint)
		{
			var w = _mapTiles.GetLength(0);
			var h = _mapTiles.GetLength(1);

			foreach (var tuple in GetBlocksNear(_avatarPoint))
			{
				var block = tuple.Item2;
				var blockId = tuple.Item1;

				for (var i = 0; i < MapBlock.SIZE; i++)
				{
					for (var j = 0; j < MapBlock.SIZE; j++)
					{
						var worldX = blockId.X * MapBlock.SIZE + i;
						var worldY = blockId.Y * MapBlock.SIZE + j;

						var x = worldX - _avatarPoint.X + w / 2;
						var y = worldY - _avatarPoint.Y + h / 2;

						if (x < 0 || y < 0 || x >= w || y >= h)
						{
							continue;
						}
						_mapTiles[x, y] = new MapCell(block, i, j, new Point(worldX, worldY));
					}
				}
			}
		}

		public MapCell GetMapCell(int _worldX, int _worldY)
		{
			var point = new Point(_worldX, _worldY);
			var blockCoords = MapBlock.GetBlockCoords(point);
			var block = this[blockCoords];
			var coords = MapBlock.GetInBlockCoords(point);
			return new MapCell(block, coords.X, coords.Y, new Point(_worldX, _worldY));
		}

		public void MoveCreature(Creature _creature, Point _fromBlock, Point _toBlock)
		{
			this[_fromBlock].Creatures.Remove(_creature);
			this[_toBlock].Creatures.Add(_creature);
		}
	}
}
