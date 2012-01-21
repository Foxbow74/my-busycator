using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Graphics;

namespace GameCore
{
	public class Map
	{
		const int ACTIVE_SIZE_HALF = 2;

		readonly Dictionary<Point, MapBlock> m_blocks = new Dictionary<Point, MapBlock>();

		public Map()
		{
			var initialized = GetBlocksNear(new Point()).Count();
		}

		private IEnumerable<Tuple<Point, MapBlock>> GetBlocksNear(Point _point)
		{
			var centralBlockCoord = MapBlock.GetBlockCoords(_point);
			for (var i = -ACTIVE_SIZE_HALF; i < ACTIVE_SIZE_HALF; ++i)
			{
				for (var j = -ACTIVE_SIZE_HALF; j < ACTIVE_SIZE_HALF; ++j)
				{
					var blockId = new Point(centralBlockCoord.X + i, centralBlockCoord.Y + j);
					MapBlock block;
					if (!m_blocks.TryGetValue(blockId, out block))
					{
						block = GenerateBlock(blockId);
						m_blocks.Add(blockId, block);
						Debug.WriteLine("Generated new in " + blockId);
					}
					yield return new Tuple<Point, MapBlock>(blockId, block);
				}
			}
		}

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

		private MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock();
			MapBlockGenerator.Generate(block, _blockId);
			return block;
		}

		public ETerrains GetTerrain(int _newX, int _newY)
		{
			var point = new Point(_newX, _newY);
			var blockCoords = MapBlock.GetBlockCoords(point);
			var block = m_blocks[blockCoords];
			var coords = MapBlock.GetInBlockCoords(point);
			var content = block.Map[coords.X, coords.Y];
			return content;
		}
	}
}
