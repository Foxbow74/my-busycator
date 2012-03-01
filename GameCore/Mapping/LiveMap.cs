﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping.Layers;
using GameCore.Messages;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameCore.Mapping
{
	public class LiveMap
	{
		private readonly LosManager m_visibilityManager;


		//1 - active lights (visible for player)
		//2 - active creatures
		//3 - border
		public const int ACTIVE_QRADIUS = 3;
		public readonly Point ACTIVE_QPOINT = new Point(ACTIVE_QRADIUS, ACTIVE_QRADIUS);

		private readonly Point[] m_blockIds;

		private readonly int m_sizeInBlocks;
		private Point m_vieportSize;

		internal LiveMapBlock[,] Blocks { get; private set; }

		public LiveMapCell[,] Cells { get; private set; }

		public static int SizeInCells { get; private set; }

		public Creature FirstActiveCreature
		{
			get
			{
				Creature first = World.TheWorld.Avatar;
				foreach (var block in Blocks)
				{
					if (block.IsBorder) continue;
					foreach (var creature in block.Creatures)
					{
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

		public LiveMap()
		{
			m_visibilityManager = new LosManager(20);

			m_sizeInBlocks = (2 * ACTIVE_QRADIUS + 1);
			SizeInCells = m_sizeInBlocks * MapBlock.SIZE;

			Blocks = new LiveMapBlock[m_sizeInBlocks, m_sizeInBlocks];
			Cells = new LiveMapCell[SizeInCells, SizeInCells];
	
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
		}

		private void LayerChanged()
		{
			foreach (var blockId in m_blockIds)
			{
				ClearBlock(blockId);
			}
			Actualize();
		}

		private Point GetCenterLiveCell()
		{
			var inBlock = MapBlock.GetInBlockCoords(World.TheWorld.Avatar.LiveCoords);
			return CenterLiveBlock * MapBlock.SIZE + inBlock;
		}

		public void SetViewPortSize(Point _size)
		{
			m_vieportSize = _size;
		}

		public void Actualize()
		{
			using (new Profiler())
			{
				var layer = World.TheWorld.Avatar.Layer;
				var d = (ACTIVE_QPOINT - CenterLiveBlock);
				foreach (var blockId in m_blockIds)
				{
					var edelta = (blockId + d).Wrap(m_sizeInBlocks, m_sizeInBlocks) - ACTIVE_QPOINT;
					var liveMapBlock = Blocks[blockId.X, blockId.Y];
					if (liveMapBlock.MapBlock == null)
					{
						var mapBlockId = World.TheWorld.AvatarBlockId + edelta;
						liveMapBlock.SetMapBlock(layer[mapBlockId]);
					}
					liveMapBlock.IsBorder = edelta.QLenght >= ACTIVE_QRADIUS;
				}
			}
		}
		
		public Point GetData()
		{
			var centerLiveCell = GetCenterLiveCell();

			using (new Profiler())
			{
				var lighted = CenterLiveBlock.NearestPoints.Select(Wrap).ToList();

				lighted.Add(CenterLiveBlock);

				foreach (var blockId in lighted)
				{
					Blocks[blockId.X, blockId.Y].ClearTemp();
				}

				m_visibilityManager.SetVisibleCelss(this, centerLiveCell, FColor.White);

				foreach (var blockId in lighted)
				{
					var liveCellZero = blockId * MapBlock.SIZE;
					var liveMapBlock = Blocks[blockId.X, blockId.Y];

					foreach (var tuple in liveMapBlock.MapBlock.LightSources)
					{
						tuple.Item1.LightCells(this, liveCellZero + tuple.Item2);
					}
				}

				if (World.TheWorld.Avatar.Light != null)
				{
					World.TheWorld.Avatar.Light.LightCells(this, centerLiveCell);
				}
			}

			var zeroLiveCell = centerLiveCell - m_vieportSize/2;
			return zeroLiveCell;
		}

		private Point Wrap(Point _liveBlockId)
		{
			return _liveBlockId.Wrap(m_sizeInBlocks, m_sizeInBlocks);
		}

		public void CreaturesLayerChanged(Creature _creature, WorldLayer _oldLayer, WorldLayer _newLayer)
		{
			if(_creature.IsAvatar)
			{
				var coords = _creature.LiveCoords;
				LayerChanged();
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
			if(_creature.IsAvatar)
			{
				MessageManager.SendMessage(this, WorldMessage.AvatarMove);
			}
		}

		#region avatar move

		private void AvatarChangesBlock(Point _blockId, Point _newBlockId)
		{
			var delta = _newBlockId - _blockId;

			var clear = Wrap(CenterLiveBlock - delta * ACTIVE_QRADIUS);

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

		private void ClearBlock(Point _blockId)
		{
			Blocks[_blockId.X, _blockId.Y].Clear();
		}

		#endregion

		public LiveMapCell GetCell(Point _liveCoords)
		{
			var coords = WrapCellCoords(_liveCoords);
			return Cells[coords.X, coords.Y];
		}

		public static Point WrapCellCoords(Point _point)
		{
			return _point.Wrap(SizeInCells, SizeInCells);
		}
	}
}
