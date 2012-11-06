using GameCore;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XTerrainSetVM : XAbstractTileSetVM
	{
		public XTerrainSetVM() { }

		[X("TERRAIN")]
		private readonly IXValue<int> m_eTerrains;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TERRAIN_SET; }
		}

		public ETerrains Terrain { get { return (ETerrains)m_eTerrains.Value; } set { m_eTerrains.Value = (int)value; } }

		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			BindProperty(m_eTerrains, ()=>Terrain);
		}
	}
}