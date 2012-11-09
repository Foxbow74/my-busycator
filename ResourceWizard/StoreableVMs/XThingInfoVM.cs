using GameCore.Objects;
using GameCore.Storage;
using RusLanguage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XThingInfoVM:XObjectVM
	{
		public override EStoreKind Kind
		{
			get { return EStoreKind.THING_INFO; }
		}

		[X("TILESET")]private IXValue<XTileSetVM> m_tileSetVM;
		[X("TILEINFO")]private IXValue<XTileInfoVM> m_tileInfoVM;
		[X("NAME")]private IXValue<string> m_name;
		[X("SEX")]private IXValue<byte> m_sex;
		[X("CATEGORY")]private IXValue<byte> m_category;
		[X("COLOR")]private IXValue<XColorVM> m_color;

		public XTileSetVM TileSetVM { get { return m_tileSetVM.Value; } set { m_tileSetVM.Value = value; } }
		public XTileInfoVM TileInfoVM { get { return m_tileInfoVM.Value; } set { m_tileInfoVM.Value = value; } }
		public string Name { get { return m_name.Value; } set { m_name.Value = value; } }
		public ESex Sex { get { return (ESex)m_sex.Value; } set { m_sex.Value = (byte)value; } }
		public EThingCategory Category { get { return (EThingCategory)m_category.Value; } set { m_category.Value = (byte)value; } }
		public XColorVM Color { get { return m_color.Value; } set { m_color.Value = value; } }
	}
}
