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

		[X("Color")]
		private IXValue<XColor> m_color;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_INFO; }
		}

		public int X { get { return m_x.Value; } set { m_x.Value = value; } }

		public int Y { get { return m_y.Value; } set { m_y.Value = value; } }

		#region Порядковый номер тайла

		[X("Order")] private IXValue<int> m_order;

		public int Order
		{
			get { return m_order.Value; }
			set { m_order.Value = value; }
		}

		#endregion


		#region координаты тайла на скомпилированной текстуре

		[X("CX")]
		private IXValue<int> m_cx;

		[X("CY")]
		private IXValue<int> m_cy;

		public int CX
		{
			get { return m_cx.Value; }
			set { m_cx.Value = value; }
		}

		public int CY
		{
			get { return m_cy.Value; }
			set { m_cy.Value = value; }
		}

		#endregion


		public XColor Color { get { return m_color.Value; } set { m_color.Value = value; } }

		public int Texture { get { return m_eTexture.Value; } set { m_eTexture.Value = value; } }
	}
}