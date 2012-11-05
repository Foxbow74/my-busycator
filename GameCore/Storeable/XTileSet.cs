using System.Collections.Generic;
using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XTileSet:XObject
	{
		public XTileSet() { }

		[X("TERRAIN")]
		private readonly IXValue<int> m_eTerrains;

		[X("LIST")]
		private ICollection<XTerrainInfo> m_children;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_SET;} 
		}

		public ETile Tile{get { return (ETile)m_eTerrains.Value; } set { m_eTerrains.Value = (int)value; }}

		public ICollection<XTerrainInfo> Children
		{
			get { return m_children; }
		}

	}
}