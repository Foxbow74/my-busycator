using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XTerrainInfo : XChildObject<XTileSet>
	{
		public XTerrainInfo() { }

		[X("X")]private IXValue<int> m_x;
		[X("Y")]private IXValue<int> m_y;
		[X("Color")]private IXValue<string> m_color;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TERRAIN_INFO; }
		}

		public int X { get { return m_x.Value; } set { m_x.Value = value; } }

		public int Y { get { return m_y.Value; } set { m_y.Value = value; } }

		public string Color { get { return m_color.Value; } set { m_color.Value = value; } }


	}
}