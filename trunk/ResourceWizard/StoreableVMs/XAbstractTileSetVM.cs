using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using GameCore.Storage;
using XTransport;
using XTransport.Client;

namespace ResourceWizard.StoreableVMs
{
	[XFactory(typeof(ATileSetFactory))]
	abstract class XAbstractTileSetVM : XObjectVM
	{
		[X("LIST")]
		private ICollection<XTileInfoVM> m_children;

		public ICollection<XTileInfoVM> Children
		{
			get { return m_children; }
		}

		public ReadOnlyObservableCollection<XTileInfoVM> ChildrenObsCol { get; private set; }

		protected override void InstantiationFinished()
		{
			ChildrenObsCol = CreateObservableCollection(m_children);
			CollectionViewSource.GetDefaultView(ChildrenObsCol).SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
		}
	}

	class ATileSetFactory: IXObjectFactory<EStoreKind>
	{
		public EStoreKind Kind
		{
			get { throw new System.NotImplementedException(); }
		}

		public IClientXObject<EStoreKind> CreateInstance(EStoreKind _kind)
		{
			switch (_kind)
			{
				case EStoreKind.TILE_SET:
					return new XTileSetVM();
				case EStoreKind.TERRAIN_SET:
					return new XTerrainSetVM();
				default:
					throw new ArgumentOutOfRangeException("_kind");
			}
		}
	}
}