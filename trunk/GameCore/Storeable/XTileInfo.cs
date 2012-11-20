using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XTileInfo : XChildObject<XAbstractTileSet>
	{
		public XTileInfo(){}

#pragma warning disable 649
		[X("TEXTURE")]
		private readonly IXValue<int> m_eTexture;
		[X("X")]private IXValue<int> m_x;
		[X("Y")]private IXValue<int> m_y;
		[X("Color")]private IXValue<XColor> m_color;
		[X("Order")]private IXValue<int> m_order;

		[X("CX")]private IXValue<int> m_cx;
		[X("CY")]private IXValue<int> m_cy;
		[X("Opacity")]private IXValue<float> m_opacity;
#pragma warning restore 649


		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_INFO; }
		}

		public int X { get { return m_x.Value; } set { m_x.Value = value; } }

		public int Y { get { return m_y.Value; } set { m_y.Value = value; } }

		#region Порядковый номер тайла


		public int Order
		{
			get { return m_order.Value; }
			set { m_order.Value = value; }
		}

		#endregion


		#region координаты тайла на скомпилированной текстуре


		public int Cx
		{
			get { return m_cx.Value; }
			set { m_cx.Value = value; }
		}

		public int Cy
		{
			get { return m_cy.Value; }
			set { m_cy.Value = value; }
		}

		#endregion


		public XColor Color { get { return m_color.Value; } set { m_color.Value = value; } }

		public int Texture { get { return m_eTexture.Value; } set { m_eTexture.Value = value; } }

		public float Opacity
		{
			get { return m_opacity.Value; }
			set { m_opacity.Value = value; }
		}
	}
}