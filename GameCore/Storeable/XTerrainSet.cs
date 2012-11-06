using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XTerrainSet : XAbstractTileSet
	{
		public XTerrainSet() { }

		[X("TERRAIN")]
		private readonly IXValue<int> m_terrains;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TERRAIN_SET; }
		}

		public ETerrains Terrains { get { return (ETerrains)m_terrains.Value; } set { m_terrains.Value = (int)value; } }

	}
}