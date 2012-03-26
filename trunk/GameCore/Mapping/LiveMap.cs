using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping.Layers;
using GameCore.Messages;
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
		public const int AVATAR_SIGHT = 25;
		public static readonly Point ActiveQpoint = new Point(ACTIVE_QRADIUS, ACTIVE_QRADIUS);

		private readonly Point[] m_blockIds;

		private readonly int m_sizeInBlocks;
		private readonly LosManager m_visibilityManager;
		private Point m_vieportSize;

		public LiveMap()
		{
			m_visibilityManager = new LosManager(AVATAR_SIGHT);

			m_sizeInBlocks = (2*ACTIVE_QRADIUS + 1);
			SizeInCells = m_sizeInBlocks*BaseMapBlock.SIZE;

			Blocks = new LiveMapBlock[m_sizeInBlocks,m_sizeInBlocks];
			Cells = new LiveMapCell[SizeInCells,SizeInCells];

			CenterLiveBlock = Point.Zero;

			{
				var blockIds = new List<Point>();
				for (var i = 0; i < m_sizeInBlocks; i++) for (var j = 0; j < m_sizeInBlocks; j++) blockIds.Add(new Point(i, j));
				m_blockIds = blockIds.ToArray();
			}

			for (var index = 0; index < m_blockIds.Length; index++)
			{
				var id = m_blockIds[index];
				Blocks[id.X, id.Y] = new LiveMapBlock(this, id, index);
			}

			PathFinder = new PathFinder(SizeInCells);
		}

		public PathFinder PathFinder { get; private set; }

		internal LiveMapBlock[,] Blocks { get; private set; }

		public LiveMapCell[,] Cells { get; private set; }

		public static int SizeInCells { get; private set; }

		public Creature FirstActiveCreature
		{
			get
			{
				var i = 1;
				Creature first = World.TheWorld.Avatar;
				foreach (var block in Blocks)
				{
					if (block.IsBorder) continue;
					foreach (var creature in block.Creatures)
					{
						i++;
						if (first.BusyTill > creature.BusyTill)
						{
							first = creature;
						}
					}
				}
				return first;
			}
		}

		public Point CenterLiveBlock { get; private set; }

		private void AvatarLayerChanged(WorldLayer _oldLayer)
		{
			foreach (var mapBlock in _oldLayer.Blocks.Values)
			{
				mapBlock.AvatarLeftLayer();
			}
			Reset();
		}

		private Point GetCenterLiveCell()
		{
			var inBlock = BaseMapBlock.GetInBlockCoords(World.TheWorld.Avatar.LiveCoords);
			return CenterLiveBlock*BaseMapBlock.SIZE + inBlock;
		}

		public void SetViewPortSize(Point _size) { m_vieportSize = _size; }

		public void Actualize()
		{
			using (new Profiler())
			{
				var layer = World.TheWorld.Avatar.Layer;
				var d = (ActiveQpoint - CenterLiveBlock);
				foreach (var blockId in m_blockIds)
				{
					var edelta = (blockId + d).Wrap(m_sizeInBlocks, m_sizeInBlocks) - ActiveQpoint;
					var liveMapBlock = Blocks[blockId.X, blockId.Y];
					if (liveMapBlock.MapBlock == null)
					{
						var mapBlockId = World.TheWorld.AvatarBlockId + edelta;
						var mapBlock = layer[mapBlockId];
						liveMapBlock.SetMapBlock(mapBlock);
					}
					else
					{
						liveMapBlock.UpdatePathFinderMapCoords();
					}
					liveMapBlock.IsBorder = edelta.QLenght >= ACTIVE_QRADIUS;
				}
				PathFinder.Clear();
			}
		}

		public byte GetPfIsPassable(Point _pathMapCoords, Creature _creature)
		{
			var delta = _pathMapCoords - _creature[0, 0].PathMapCoords;
			var liveMapCell = _creature[delta];
			float result;
			if (_creature.IsAvatar)
			{
				if (liveMapCell.Visibility.Lightness() > World.TheWorld.Avatar.Layer.FogLightness)
				{
					result = liveMapCell.GetPfIsPassableBy(_creature);
				}
				else if (liveMapCell.IsSeenBefore)
				{
					result = liveMapCell.TerrainAttribute.IsPassable;
				}
				else
				{
					result = 0;
				}
			}
			else
			{
				result = liveMapCell.GetPfIsPassableBy(_creature);
			}
			if (result == 0) return 0;
			return (byte) (255 - result*254);
		}

		public Point GetData()
		{
			using (new Profiler())
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
					var liveCellZero = blockId*BaseMapBlock.SIZE;
					var liveMapBlock = Blocks[blockId.X, blockId.Y];

					foreach (var tuple in liveMapBlock.MapBlock.LightSources)
					{
						var lightSource = tuple.Item1;
						var point = liveCellZero + tuple.Item2;

						if ((lightSource.Radius + AVATAR_SIGHT) >= World.TheWorld.Avatar[0, 0].WorldCoords.GetDistTill(Cells[point.X, point.Y].WorldCoords))
						{
							lightSource.LightCells(this, point);
						}
					}
				}

				if (World.TheWorld.Avatar.Light != null)
				{
					World.TheWorld.Avatar.Light.LightCells(this, centerLiveCell);
				}

				var zeroLiveCell = centerLiveCell - m_vieportSize/2;
				return zeroLiveCell;
			}
		}

		private Point Wrap(Point _liveBlockId) { return _liveBlockId.Wrap(m_sizeInBlocks, m_sizeInBlocks); }

		public void CreaturesLayerChanged(Creature _creature, WorldLayer _oldLayer, WorldLayer _newLayer)
		{
			if (_creature.IsAvatar)
			{
				var coords = _creature.LiveCoords;
				AvatarLayerChanged(_oldLayer);
				_creature.LiveCoords = coords;
				MessageManager.SendMessage(this, WorldMessage.AvatarMove);
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public void CreaturesCellChanged(Creature _creature, Point _oldLiveCoords, Point _newLiveCoords)
		{
			if (_oldLiveCoords != null && _newLiveCoords != null && BaseMapBlock.GetInBlockCoords(_oldLiveCoords) == BaseMapBlock.GetInBlockCoords(_newLiveCoords)) return;
			var oldBlock = _oldLiveCoords != null ? GetCell(_oldLiveCoords).LiveMapBlock : null;
			if (_newLiveCoords == null)
			{
				oldBlock.RemoveCreature(_creature, _oldLiveCoords);
				return;
			}
			var newBlock = GetCell(_newLiveCoords).LiveMapBlock;
			if (newBlock != oldBlock)
			{
				newBlock.AddCreature(_creature, _newLiveCoords);
				if (oldBlock != null)
				{
					oldBlock.RemoveCreature(_creature, _oldLiveCoords);
					if (_creature.IsAvatar)
					{
						AvatarChangesBlock(oldBlock.LiveMapBlockId, newBlock.LiveMapBlockId);
						CenterLiveBlock = newBlock.LiveMapBlockId;
						World.TheWorld.SetAvatarBlockId(newBlock.MapBlock.BlockId);
						Actualize();
					}
				}
			}
			else
			{
				newBlock.RemoveCreature(_creature, _oldLiveCoords);
				newBlock.AddCreature(_creature, _newLiveCoords);
			}
			if (_creature.IsAvatar)
			{
				MessageManager.SendMessage(this, WorldMessage.AvatarMove);
			}
		}

		public LiveMapCell GetCell(Point _liveCoords)
		{
			var coords = WrapCellCoords(_liveCoords);
			return Cells[coords.X, coords.Y];
		}

		public static Point WrapCellCoords(Point _point) { return _point.Wrap(SizeInCells, SizeInCells); }

		public void Reset()
		{
			foreach (var blockId in m_blockIds)
			{
				ClearBlock(blockId);
			}
			Actualize();
		}

		#region avatar move

		private void AvatarChangesBlock(Point _blockId, Point _newBlockId)
		{
			var delta = _newBlockId - _blockId;

			var clear = Wrap(CenterLiveBlock - delta*ACTIVE_QRADIUS);

			if (delta.X != 0)
			{
				ClearColumn(clear.X);
			}
			if (delta.Y != 0)
			{
				ClearRow(clear.Y);
			}
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

		private void ClearBlock(Point _blockId) { Blocks[_blockId.X, _blockId.Y].Clear(); }

		#endregion
	}
}