using System.Diagnostics;
using GameCore.Misc;

namespace GameCore.Mapping
{
	class LiveMapBlock
	{
		private readonly LiveMap m_liveMap;
		private readonly Point m_liveMapBlockId;
		private readonly int m_liveMapBlockIndex;
		private MapBlock m_mapBlock;

		public LiveMapBlock(LiveMap _liveMap, Point _liveMapBlockId, int _liveMapBlockIndex)
		{
			m_liveMap = _liveMap;
			m_liveMapBlockId = _liveMapBlockId;
			m_liveMapBlockIndex = _liveMapBlockIndex;
		}

		public bool Filled { get; private set; }

		public MapBlock MapBlock
		{
			get { return m_mapBlock; }
			private set
			{
				m_mapBlock = value;
			}
		}

		public void ClearLight()
		{
			var liveCellZero = m_liveMapBlockId * MapBlock.SIZE;
			for (var i = 0; i < MapBlock.SIZE; i++)
			{
				for (var j = 0; j < MapBlock.SIZE; j++)
				{
					m_liveMap.Cells[liveCellZero.X + i, liveCellZero.Y + j].ClearLight();
				}
			}
		}

		public void Fill()
		{
			var liveCellZero = m_liveMapBlockId*MapBlock.SIZE;
			var mapCellZero = m_mapBlock.BlockId*MapBlock.SIZE;
			for (var i = 0; i < MapBlock.SIZE; i++)
			{
				for (var j = 0; j < MapBlock.SIZE; j++)
				{
					var ij = new Point(i, j);
					var mc = new MapCell(m_mapBlock, ij, mapCellZero + ij);
					m_liveMap.Cells[liveCellZero.X + i, liveCellZero.Y + j].SetMapCell(mc);
				}
			}
		}

		public void SetMapBlock(MapBlock _mapBlock)
		{
			Debug.WriteLine("* mapped " + _mapBlock.BlockId + " to live " + m_liveMapBlockId);
			MapBlock = _mapBlock;
		}

		public override string ToString()
		{
			return "LB:" + (m_mapBlock==null?"<null>":m_mapBlock.BlockId.ToString());
		}

		public void Clear()
		{
			MapBlock = null;
		}
	}
}