using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameCore;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XTileSetVM : XObjectVM
	{
		public XTileSetVM() { }

		[X("TILE")]
		private readonly IXValue<int> m_eTile;

		[X("LIST")]
		private ICollection<XTerrainInfoVM> m_children;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_SET; }
		}

		public ETile Tile { get { return (ETile)m_eTile.Value; } set { m_eTile.Value = (int)value; } }

		public ICollection<XTerrainInfoVM> Children
		{
			get { return m_children; }
		}

		public ReadOnlyObservableCollection<XTerrainInfoVM> ChildrenObsCol { get; private set; }

		protected override void InstantiationFinished()
		{
			BindProperty(m_eTile, () => Tile);
			ChildrenObsCol = CreateObservableCollection(m_children);
		}
	}
}