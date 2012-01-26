#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameCore.Creatures;
using GameCore.Misc;

#endregion

namespace GameCore.Mapping
{
	public class Map
	{
		private const int ACTIVE_SIZE_HALF = 4;

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
					Debug.WriteLine("Generated new in " + _blockId);
				}
				return block;
			}
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

		private static MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			MapBlockGenerator.Generate(block, _blockId, World.TheWorld);
			return block;
		}

		/// <summary>
		/// 	Заполняет двумерный массив значениями из карты вокруг игрока
		/// </summary>
		/// <param name = "_mapTiles"></param>
		/// <param name = "_avatarPoint"></param>
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
						var worldX = blockId.X*MapBlock.SIZE + i;
						var worldY = blockId.Y*MapBlock.SIZE + j;

						var x = worldX - _avatarPoint.X + w/2;
						var y = worldY - _avatarPoint.Y + h/2;

						if (x < 0 || y < 0 || x >= w || y >= h)
						{
							continue;
						}
						_mapTiles[x, y] = new MapCell(block, new Point(i, j), new Point(worldX, worldY));
					}
				}
			}
		}

		public static MapBlock GetMapBlock(Point _worldCoords)
		{
			var blockCoords = MapBlock.GetBlockCoords(_worldCoords);
			var block = World.TheWorld.Map[blockCoords];
			return block;
		}

		public static MapCell GetMapCell(Point _worldCoords)
		{
			var blockCoords = MapBlock.GetBlockCoords(_worldCoords);
			var block = World.TheWorld.Map[blockCoords];
			var coords = MapBlock.GetInBlockCoords(_worldCoords);
			return new MapCell(block, coords, _worldCoords);
		}

		public static void MoveCreature(Creature _creature, Point _fromBlock, Point _toBlock)
		{
			World.TheWorld.Map[_fromBlock].Creatures.Remove(_creature);
			World.TheWorld.Map[_toBlock].Creatures.Add(_creature);
		}
	}
}