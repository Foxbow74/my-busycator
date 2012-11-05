using GameCore.Storage;
using RusLanguage;
using XTransport;

namespace GameCore.Storeable
{
	class XTileInfo : XObject
	{
		public XTileInfo(){}

		[X("ETile")]private IXValue<int> m_eTile;
		[X("X")]private IXValue<int> m_x;
		[X("Y")]private IXValue<int> m_y;
		[X("Color")]private IXValue<string> m_color;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_INFO; }
		}

		public int X { get { return m_x.Value; } set { m_x.Value = value; } }

		public int Y { get { return m_y.Value; } set { m_y.Value = value; } }

		public string Color { get { return m_color.Value; } set { m_color.Value = value; } }

		public ETiles Tile { get { return (ETiles)m_eTile.Value; } set { m_eTile.Value = (int)value; } }
	}

	class XNicksInfo : XObject
	{
		public XNicksInfo() { }

		[X("Sex")]
		private IXValue<int> m_sex;
		[X("Nicks")]
		private IXValue<string> m_nicks;

		public override EStoreKind Kind
		{
			get { return EStoreKind.NICKS_INFO; }
		}

		public string Nicks { get { return m_nicks.Value; } set { m_nicks.Value = value; } }

		public ESex Sex { get { return (ESex)m_sex.Value; } set { m_sex.Value = (int)value; } }
	}
}