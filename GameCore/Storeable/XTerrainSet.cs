using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XTerrainSet : XAbstractTileSet
	{
		public XTerrainSet() { }

#pragma warning disable 649
		[X("TERRAIN")]private readonly IXValue<int> m_terrains;
#pragma warning restore 649

		public override EStoreKind Kind
		{
			get { return EStoreKind.TERRAIN_SET; }
		}

		public ETerrains Terrains { get { return (ETerrains)m_terrains.Value; } set { m_terrains.Value = (int)value; } }

	}
}