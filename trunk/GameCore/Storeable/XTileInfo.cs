using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XTileInfo : XChildObject<XAbstractTileSet>
	{
		public XTileInfo(){}

		[X("TEXTURE")]private readonly IXValue<int> m_eTexture;
		[X("X")]
		private IXValue<int> m_x;
		[X("Y")]
		private IXValue<int> m_y;

		[X("CX")]
		private IXValue<int> m_cx;

		[X("CY")]
		private IXValue<int> m_cy;
		
		[X("Color")]
		private IXValue<string> m_color;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_INFO; }
		}

		public int X { get { return m_x.Value; } set { m_x.Value = value; } }

		public int Y { get { return m_y.Value; } set { m_y.Value = value; } }
		
		//координаты тайла на скомпилированной текстуре
		public int CX { get { return m_cx.Value; } set { m_cx.Value = value; } }

		//координаты тайла на скомпилированной текстуре
		public int CY { get { return m_cy.Value; } set { m_cy.Value = value; } }

		public FColor Color { get { return FColor.Parse(m_color.Value); } set { m_color.Value = value.ToShortText(); } }

		public int Texture { get { return m_eTexture.Value; } set { m_eTexture.Value = value; } }
	}
}