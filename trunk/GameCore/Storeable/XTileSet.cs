using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XTileSet : XAbstractTileSet
	{
		public XTileSet() { }

		[X("TILE")]
		private readonly IXValue<int> m_tiles;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_SET;} 
		}

		public ETiles Tile{get { return (ETiles)m_tiles.Value; } set { m_tiles.Value = (int)value; }}

	}
}