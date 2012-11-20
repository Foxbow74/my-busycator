using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XTileSet : XAbstractTileSet
	{
		public XTileSet() { }

#pragma warning disable 649
		[X("TILE")]private readonly IXValue<int> m_tiles;
#pragma warning restore 649

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_SET;} 
		}

		public ETileset Tileset{get { return (ETileset)m_tiles.Value; } set { m_tiles.Value = (int)value; }}

	}
}