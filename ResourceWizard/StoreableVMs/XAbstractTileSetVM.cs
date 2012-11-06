using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	abstract class XAbstractTileSetVM : XObjectVM
	{
		[X("LIST")]
		private ICollection<XTileInfoVM> m_children;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_SET; }
		}

		public ICollection<XTileInfoVM> Children
		{
			get { return m_children; }
		}

		public ReadOnlyObservableCollection<XTileInfoVM> ChildrenObsCol { get; private set; }

		protected override void InstantiationFinished()
		{
			ChildrenObsCol = CreateObservableCollection(m_children);
		}
	}
}