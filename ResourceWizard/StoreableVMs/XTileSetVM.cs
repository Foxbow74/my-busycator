using GameCore;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XTileSetVM : XAbstractTileSetVM
	{
		public XTileSetVM() { }

		[X("TILE")]
		private readonly IXValue<int> m_eTile;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_SET; }
		}

		public ETileset Key { get { return (ETileset)m_eTile.Value; } set { m_eTile.Value = (int)value; } }

        public string KeyName { get { return Key.ToString(); } }
        
		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			BindProperty(m_eTile, () => Key);
		}
	}
}