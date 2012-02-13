using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
					//Debug.WriteLine("Generated new in " + _blockId);
				}
				return block;
			}
		}

		internal abstract IEnumerable<ETerrains> DefaultEmptyTerrains { get; }

		protected virtual MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			MapBlockGenerator.Generate(this, block, _blockId, World.TheWorld);
			return block;
		}

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
						_mapTiles[x, y] = new MapCell(block, new Point(i, j), new Point(worldX, worldY));//{Lighted = Ambient};
					}
				}
			}
		}

		public abstract FColor Ambient { get; }

		public abstract FColor Lighted { get; }

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