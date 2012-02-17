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

		public MapBlock MapBlock
		{
			get { return m_mapBlock; }
			private set
			{
				m_mapBlock = value;
			}
		}

		public void ClearTemp()
		{
			var liveCellZero = m_liveMapBlockId * MapBlock.SIZE;
			for (var i = 0; i < MapBlock.SIZE; i++)
			{
				for (var j = 0; j < MapBlock.SIZE; j++)
				{
					m_liveMap.Cells[liveCellZero.X + i, liveCellZero.Y + j].ClearTemp();
				}
			}
		}

		private void Fill()
		{
			var liveCellZero = m_liveMapBlockId*MapBlock.SIZE;
			var mapCellZero = m_mapBlock.BlockId*MapBlock.SIZE;
			for (var i = 0; i < MapBlock.SIZE; i++)
			{
				for (var j = 0; j < MapBlock.SIZE; j++)
				{
					var ij = new Point(i, j);
					var mc = new MapCell(m_mapBlock, ij, mapCellZero + ij);
					m_liveMap.Cells[liveCellZero.X + i, liveCellZero.Y + j].SetMapCell(mc, m_mapBlock, ij);
				}
			}
		}

		public void SetMapBlock(MapBlock _mapBlock)
		{
			MapBlock = _mapBlock;
			Fill();
		}

		public override string ToString()
		{
			return "LB:" + (m_mapBlock==null?"<null>":m_mapBlock.BlockId.ToString());
		}

		public void Clear()
		{
			MapBlock = null;
		}

		public void LightCells(LiveMap _liveMap)
		{
			var liveCellZero = m_liveMapBlockId * MapBlock.SIZE;
			foreach (var tuple in MapBlock.LightSources)
			{
				tuple.Item2.LightCells(_liveMap, liveCellZero + tuple.Item1);
			}
		}
	}
}