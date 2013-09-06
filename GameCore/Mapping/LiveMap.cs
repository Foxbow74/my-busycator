using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.PathFinding;

namespace GameCore.Mapping
{
	public class LiveMap
	{
		//1 - active lights (visible for player)
		//2 - active creatures
		//3 - border
		public const int ACTIVE_QRADIUS = 3;
		private const int SIZE_IN_BLOCKS = (2 * ACTIVE_QRADIUS + 1);
		public const int AVATAR_SIGHT = 55;
		public const int SIZE_IN_CELLS = SIZE_IN_BLOCKS*Constants.MAP_BLOCK_SIZE;

		public static readonly Point ActiveQpoint = new Point(ACTIVE_QRADIUS, ACTIVE_QRADIUS);

		private readonly Point[] m_blockIds;

		private readonly LosManager m_visibilityManager;

	    public LiveMap()
		{
			m_visibilityManager = new LosManager(AVATAR_SIGHT);

			Blocks = new LiveMapBlock[SIZE_IN_BLOCKS,SIZE_IN_BLOCKS];
			Cells = new LiveMapCell[SIZE_IN_CELLS,SIZE_IN_CELLS];

			CenterLiveBlock = Point.Zero;

			{
				var blockIds = new List<Point>();
				for (var i = 0; i < SIZE_IN_BLOCKS; i++) for (var j = 0; j < SIZE_IN_BLOCKS; j++) blockIds.Add(new Point(i, j));
				m_blockIds = blockIds.ToArray();
			}

			for (var index = 0; index < m_blockIds.Length; index++)
			{
				var id = m_blockIds[index];
				Blocks[id.X, id.Y] = new LiveMapBlock(this, id, index);
			}

			PathFinder = new PathFinder(SIZE_IN_CELLS);
		}

		public PathFinder PathFinder { get; private set; }

		public LiveMapBlock[,] Blocks { get; private set; }

		public LiveMapCell[,] Cells { get; private set; }

		public Point CenterLiveBlock { get; private set; }

	    public Point VieportSize { get; private set; }

	    private Point GetCenterLiveCell()
		{
			var inBlock = BaseMapBlock.GetInBlockCoords(World.TheWorld.Avatar.GeoInfo.WorldCoords);
			return CenterLiveBlock*Constants.MAP_BLOCK_SIZE + inBlock;
		}

		public void SetViewPortSize(Point _size) { VieportSize = _size; }

		public void Actualize()
		{
#if DEBUG
			using (new Profiler())
#endif
			{
				var layer = World.TheWorld.Avatar.GeoInfo.Layer;
				var d = (ActiveQpoint - CenterLiveBlock);
				foreach (var blockId in m_blockIds)
				{
					var edelta = (blockId + d).Wrap(SIZE_IN_BLOCKS, SIZE_IN_BLOCKS) - ActiveQpoint;
					var liveMapBlock = Blocks[blockId.X, blockId.Y];
					var mapBlockId = World.TheWorld.AvatarBlockId + edelta; 
					if (liveMapBlock.MapBlock == null)
					{
						var mapBlock = layer[mapBlockId];
						liveMapBlock.SetMapBlock(mapBlock);
					}
					else
					{
						if(liveMapBlock.MapBlock.BlockId!=mapBlockId)
						{
							throw  new ApplicationException();
						}
					}
					liveMapBlock.UpdatePathFinderMapCoords();
					liveMapBlock.IsBorder = edelta.QLenght >= ACTIVE_QRADIUS;
				}
				PathFinder.Clear();
			}

			foreach (var geoInfo in World.TheWorld.CreatureManager.PointByCreature.Keys)
			{
				geoInfo.Check();
			}
		}

		public byte GetPfIsPassable(Point _pathMapCoords, Creature _creature)
		{
			var delta = _pathMapCoords - _creature.GeoInfo.PathMapCoords;
			var liveMapCell = _creature[delta];

			float result;
			if (_creature.IsAvatar)
			{
				if (liveMapCell.Visibility.Lightness() > World.TheWorld.Avatar.GeoInfo.Layer.FogLightness)
				{
					result = liveMapCell.GetIsPassableBy(_creature,true);
				}
				else if (liveMapCell.IsSeenBefore)
				{
					result = liveMapCell.TerrainAttribute.Passability;
				}
				else
				{
					result = 0;
				}
			}
			else
			{
				result = liveMapCell.GetIsPassableBy(_creature, true);
			}
			if (result > 0) return (byte)(255 - result * 254);
			return 0;
		}

	    public Point[,] GetLightedLiveBlocks()
	    {
#if DEBUG
            using (new Profiler("GetLightedLiveBlocks"))
#endif
	        {
	            var dlts = new[]
	            {
	                new Point(- 1, - 1),
	                new Point(- 1, 0),
	                new Point(- 1, 1),
	                new Point(1, - 1),
	                new Point(1, 0),
                    new Point(1,  1),
	                new Point(0,  -1),
                    new Point(0, 0),
	                new Point(0, 1),
	            };

	            var result = new Point[9, 2];
	            for (int i = 0; i < 9; i++)
	            {
                    result[i, 0] = dlts[i];
                    result[i, 1] = Wrap(dlts[i] + CenterLiveBlock);
	            }
                return result;

	            //return dlts.ToDictionary(_p => _p, _p =>Wrap(_p + CenterLiveBlock));
	        }
	    }

        /// <summary>
        /// Возвращает смещение левой верхней ячейки, видимой на экране, относительно центральной точки где рисуется аватар
        /// </summary>
        /// <returns></returns>
	    public Point GetDPoint()
	    {
            return GetCenterLiveCell() - VieportSize / 2;
	    }

	    public Point GetData()
		{
#if DEBUG
			using (new Profiler())
#endif
			{
				var centerLiveCell = GetCenterLiveCell();

				var lighted = CenterLiveBlock.NearestPoints.Select(Wrap).ToList();

				lighted.Add(CenterLiveBlock);

				foreach (var blockId in lighted)
				{
					Blocks[blockId.X, blockId.Y].ClearTemp();
				}

				m_visibilityManager.SetVisibleCelss(this, centerLiveCell, FColor.White);

				foreach (var blockId in lighted)
				{
					var liveCellZero = blockId*Constants.MAP_BLOCK_SIZE;
					var liveMapBlock = Blocks[blockId.X, blockId.Y];

					foreach (var tuple in liveMapBlock.MapBlock.LightSources)
					{
						var lightSource = tuple.Source;
						var point = liveCellZero + tuple.Point;

						if ((lightSource.Radius + AVATAR_SIGHT) >= World.TheWorld.Avatar[0, 0].WorldCoords.GetDistTill(Cells[point.X, point.Y].WorldCoords))
						{
							lightSource.LightCells(this, point);
						}
					}
				}

				foreach (var tuple in World.TheWorld.CreatureManager.LightSources())
				{
					var lightSource = tuple.Source;
					var info = tuple.CreatureGeoInfo;

					if ((lightSource.Radius + AVATAR_SIGHT) >= World.TheWorld.Avatar[0, 0].WorldCoords.GetDistTill(info.WorldCoords))
					{
						lightSource.LightCells(this, info.LiveCoords);
					}
				}


				if (World.TheWorld.Avatar.Light != null)
				{
					World.TheWorld.Avatar.Light.LightCells(this, centerLiveCell);
				}

				var layer = World.TheWorld.Avatar.GeoInfo.Layer;
				var ambient = layer.Ambient;
				var fogLightness = layer.FogLightness;

				foreach (var blockId in lighted)
				{
					Blocks[blockId.X, blockId.Y].UpdateVisibility(fogLightness, ambient);
				}

				World.TheWorld.Avatar[0,0].UpdateAvatarCellVisibility();

                return GetDPoint();
			}
		}

		private Point Wrap(Point _liveBlockId) { return _liveBlockId.Wrap(SIZE_IN_BLOCKS, SIZE_IN_BLOCKS); }

		public LiveMapCell GetCell(Point _liveCoords)
		{
			var coords = WrapCellCoords(_liveCoords);
			return Cells[coords.X, coords.Y];
		}

		public static Point WrapCellCoords(Point _point) { return _point.Wrap(SIZE_IN_CELLS, SIZE_IN_CELLS); }

		public void Reset()
		{
			foreach (var blockId in m_blockIds)
			{
				ClearBlock(blockId);
			}
			Actualize();
		}

		#region avatar move

		public void AvatarChangesBlock(Point _oldBlockId, Point _newBlockId, Point _newCenterLiveBlockId)
		{
			var delta = _newBlockId - _oldBlockId;

			var clear = Wrap(CenterLiveBlock - delta*ACTIVE_QRADIUS);

			if (delta.X != 0)
			{
				ClearColumn(clear.X);
			}
			if (delta.Y != 0)
			{
				ClearRow(clear.Y);
			}

			CenterLiveBlock = _newCenterLiveBlockId;
			World.TheWorld.SetAvatarBlockId(_newBlockId);
			Actualize();
		}

		private void ClearColumn(int _columnIndex)
		{
			foreach (var blockId in m_blockIds.Where(_blockId => _blockId.X == _columnIndex))
			{
				ClearBlock(blockId);
			}
		}

		private void ClearRow(int _rowIndex)
		{
			foreach (var blockId in m_blockIds.Where(_blockId => _blockId.Y == _rowIndex))
			{
				ClearBlock(blockId);
			}
		}

		private void ClearBlock(Point _blockId)
		{
			Blocks[_blockId.X, _blockId.Y].Clear();
		}

		#endregion
	}
}