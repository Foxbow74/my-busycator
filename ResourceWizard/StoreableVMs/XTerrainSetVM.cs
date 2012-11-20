using GameCore;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XTerrainSetVM : XAbstractTileSetVM
	{
		public XTerrainSetVM() { }

#pragma warning disable 649
		[X("TERRAIN")] private readonly IXValue<int> m_eTerrains;
#pragma warning restore 649

		public override EStoreKind Kind
		{
			get { return EStoreKind.TERRAIN_SET; }
		}

		public ETerrains Key { get { return (ETerrains)m_eTerrains.Value; } set { m_eTerrains.Value = (int)value; } }

        public string KeyName { get { return Key.ToString(); } }

		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			BindProperty(m_eTerrains, ()=>Key);
		}
	}
}