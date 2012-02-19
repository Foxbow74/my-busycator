using System.Collections.Generic;
using System.Linq;
using GameCore.Messages;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameCore.Mapping
{
	public class LiveMap
	{
		private readonly LosManager m_visibilityManager;

		public const int ACTIVE_QRADIUS =1;

		private Point m_centerLiveBlock;

		private readonly Point[] m_blockIds;

		private readonly int m_sizeInBlocks;
		private Point m_vieportSize;

		internal LiveMapBlock[,] Blocks { get; private set; }

		public LiveMapCell[,] Cells { get; private set; }

		public int SizeInCells { get; private set; }

		public LiveMap()
		{
			m_visibilityManager = new LosManager(20);

			m_sizeInBlocks = (2 * ACTIVE_QRADIUS + 1);
			SizeInCells = m_sizeInBlocks * MapBlock.SIZE;

			Blocks = new LiveMapBlock[m_sizeInBlocks, m_sizeInBlocks];
			Cells = new LiveMapCell[SizeInCells, SizeInCells];
	
			m_centerLiveBlock = new Point(ACTIVE_QRADIUS, ACTIVE_QRADIUS);

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
			for (var i = 0; i < SizeInCells; i++)
			{
				for (var j = 0; j < SizeInCells; j++)
				{
					Cells[i,j] = new LiveMapCell();
				}
			}

			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
			switch (_message.Type)
			{
				case WorldMessage.EType.TURN:
					OnTurn();
					break;
				case WorldMessage.EType.AVATAR_CHANGE_LAYER:
					LayerChanged();
					break;
			}
		}

		private void LayerChanged()
		{
			foreach (var block in Blocks)
			{
				block.Clear();
			}
			Actualize(World.TheWorld.Avatar.BlockId);
		}

		private void OnTurn()
		{
			using (new Profiler())
			{
				var lighted = m_centerLiveBlock.NearestPoints.Select(Wrap).ToList();
				lighted.Add(m_centerLiveBlock);

				foreach (var blockId in lighted)
				{
					Blocks[blockId.X, blockId.Y].ClearTemp();
				}

				var centerLiveCell = GetCenterLiveCell();
				m_visibilityManager.SetVisibleCelss(this, centerLiveCell, FColor.White);

				foreach (var blockId in lighted)
				{
					Blocks[blockId.X, blockId.Y].LightCells(this);
				}

				if (World.TheWorld.Avatar.Light != null)
				{
					World.TheWorld.Avatar.Light.LightCells(this, centerLiveCell);
				}
			}
		}

		private Point GetCenterLiveCell()
		{
			var inBlock = MapBlock.GetInBlockCoords(World.TheWorld.Avatar.Coords);
			return m_centerLiveBlock * MapBlock.SIZE + inBlock;
		}

		public void SetViewPortSize(Point _size)
		{
			m_vieportSize = _size;
		}

		public void Actualize(Point _avatarBlockId)
		{
			using (new Profiler())
			{
				var layer = World.TheWorld.Avatar.Layer;
				foreach (var blockId in m_blockIds)
				{
					if (Blocks[blockId.X, blockId.Y].MapBlock == null)
					{
						var offset = (blockId - m_centerLiveBlock).Sphere(ACTIVE_QRADIUS);
						var mapBlockId = _avatarBlockId + offset;
						Blocks[blockId.X, blockId.Y].SetMapBlock(layer[mapBlockId]);
					}
				}
			}
		}
		
		public Point GetData()
		{
			var inBlock = MapBlock.GetInBlockCoords(World.TheWorld.Avatar.Coords);
			var centerLiveCell = m_centerLiveBlock*MapBlock.SIZE + inBlock;
			var zeroLiveCell = centerLiveCell - m_vieportSize/2;
			return zeroLiveCell;
		}

		private Point Wrap(Point _liveBlockId)
		{
			return _liveBlockId.Wrap(m_sizeInBlocks, m_sizeInBlocks);
		}

		public void AvatarMoved(Point _blockId, Point _newBlockId)
		{
			var delta = _newBlockId - _blockId;

			var clear = Wrap(m_centerLiveBlock - delta*ACTIVE_QRADIUS);

			if (delta.X != 0)
			{
				ClearColumn(clear.X);
			}
			if (delta.Y != 0)
			{
				ClearRow(clear.Y);
			}

			m_centerLiveBlock = Wrap(m_centerLiveBlock + delta);

			Actualize(_newBlockId);
		}

		private void ClearColumn(int _columnIndex)
		{
			foreach (var blockId in m_blockIds.Where(_blockId => _blockId.X == _columnIndex))
			{
				Blocks[blockId.X, blockId.Y].Clear();
			}
		}

		private void ClearRow(int _rowIndex)
		{
			foreach (var blockId in m_blockIds.Where(_blockId => _blockId.Y == _rowIndex))
			{
				Blocks[blockId.X, blockId.Y].Clear();
			}
		}
	}
}
