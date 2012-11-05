using GameCore.Storage;
using GameUi;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XTerrainInfoVM : XChildObjectVM<XTileSetVM>
	{
		public override EStoreKind Kind
		{
			get { return EStoreKind.TERRAIN_INFO; }
		}

		public XTerrainInfoVM() { }

		[X("TEXTURE")]
		private readonly IXValue<int> m_eTexture;
		[X("X")]
		private IXValue<int> m_x;
		[X("Y")]
		private IXValue<int> m_y;
		[X("Color")]
		private IXValue<string> m_color;

		public int X { get { return m_x.Value; } set { m_x.Value = value; } }

		public int Y { get { return m_y.Value; } set { m_y.Value = value; } }

		public string Color { get { return m_color.Value; } set { m_color.Value = value; } }

		public ETextureSet Texture { get { return (ETextureSet)m_eTexture.Value; } set { m_eTexture.Value = (int)value; } }

		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			BindProperty(m_color, () => Color);
			BindProperty(m_eTexture, () => Texture);
			BindProperty(m_x, () => X);
			BindProperty(m_y, () => Y);
		}
	}
}