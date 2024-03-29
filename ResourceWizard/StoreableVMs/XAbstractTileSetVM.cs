using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	[XFactory(typeof(ATileSetFactory))]
	abstract class XAbstractTileSetVM : XObjectVM
	{
		protected XAbstractTileSetVM()
		{
		}

		[X("LIST")]
#pragma warning disable 649
		private ICollection<XTileInfoVM> m_children;
#pragma warning restore 649

		private XTileInfoVM m_selectedItem;
		private ObservableCollection<ImageSource> m_mosaic;

		public ICollection<XTileInfoVM> Children
		{
			get { return m_children; }
		}

		public ReadOnlyObservableCollection<XTileInfoVM> ChildrenObsCol { get; private set; }

		public XTileInfoVM SelectedItem
		{
			get
			{
				if(m_selectedItem==null)
				{
					m_selectedItem = m_children.FirstOrDefault();
				}
				return m_selectedItem;
			}
			set
			{
				m_selectedItem = value;
				OnPropertyChanged(()=>SelectedItem);
			}
		}

		public ObservableCollection<ImageSource> Mosaic
		{
			get { return m_mosaic??(m_mosaic = new ObservableCollection<ImageSource>(GetMosaicItems())); }
		}

	    public void UpdateMosaic()
		{
			Mosaic.Clear();
			foreach (var imageSource in GetMosaicItems())
			{
				Mosaic.Add(imageSource);
			}
		}

		protected IEnumerable<ImageSource> GetMosaicItems()
		{
			var rnd = new Random();
			for (int i = 0; i < 256; i++)
			{
				foreach (var vm in Children.OrderBy(_vm => rnd.Next()))
				{
					yield return vm.Image;
				}
			}
		}

		protected override void InstantiationFinished()
		{
			ChildrenObsCol = CreateObservableCollection(m_children);
			CollectionViewSource.GetDefaultView(ChildrenObsCol).SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
		}

        public void RefreshChildren()
        {
            var arr = Children.OrderBy(_vm => _vm.Order).ToArray();
            for (var index = 0; index < arr.Length; index++)
            {
                arr[index].Order = index;
            }
            CollectionViewSource.GetDefaultView(ChildrenObsCol).Refresh();
        }
	}
}