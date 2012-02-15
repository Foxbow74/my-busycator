using System;

namespace GameCore.Mapping
{
	public class LiveMapCell
	{
		private MapCell m_mapCell;
		private FColor m_light;

		public LiveMapCell()
		{
		}

		public MapCell MapCell
		{
			get
			{
				return m_mapCell;
			}
		}

		public FColor Visibility { get; set; }

		public void SetMapCell(MapCell _mc)
		{
			m_mapCell = _mc;
			ClearLight();
		}

		public void ClearLight()
		{
			m_light = FColor.Empty;
		}

		public override string ToString()
		{
			return "LC:" + (m_mapCell == null ? "<null>" : m_mapCell.WorldCoords.ToString()); ;
		}
	}
}