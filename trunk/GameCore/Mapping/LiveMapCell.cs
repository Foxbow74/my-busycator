using System;
using GameCore.Misc;

namespace GameCore.Mapping
{
	public class LiveMapCell
	{
		private MapCell m_mapCell;
		private uint m_seenMask;
		private Point m_inBlockCoords;
		private MapBlock m_mapBlock;

		public MapCell MapCell
		{
			get
			{
				return m_mapCell;
			}
		}

		public FColor Visibility { get; set; }

		public FColor Lighted { get; set; }

		public bool IsSeenBefore { get; private set; }

		public void SetMapCell(MapCell _mc, MapBlock _mapBlock, Point _inBlockCoords)
		{
			m_mapCell = _mc;
			m_inBlockCoords = _inBlockCoords;
			m_mapBlock = _mapBlock;
			m_seenMask = ((UInt32)1) << m_inBlockCoords.X;

			IsSeenBefore = (_mapBlock.SeenCells[_inBlockCoords.Y] & m_seenMask) != 0;
			ClearTemp();
		}

		public void SetIsSeenBefore()
		{
			if (!IsSeenBefore)
			{
				IsSeenBefore = true;
				m_mapBlock.SeenCells[m_inBlockCoords.Y] |= m_seenMask;
			}
		}

		public void ClearTemp()
		{
			Visibility = Lighted = FColor.Empty;
		}

		public override string ToString()
		{
			return "LC:" + (m_mapCell == null ? "<null>" : m_mapCell.WorldCoords.ToString()); ;
		}
	}
}