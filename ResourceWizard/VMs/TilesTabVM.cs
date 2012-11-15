using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GameCore;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard.VMs
{
	internal class TilesTabVM : TabVM
	{
		public TilesTabVM()
		{
			Set = Manager.Instance.XRoot.TileSetsObsCol;
            
            var todel = Set.Where(_vm => _vm.Key == ETileset.NONE).ToArray();
            foreach (var vm in todel)
            {
                Manager.Instance.XRoot.TileSets.Remove(vm);
            }

			foreach (var key in from ETileset key in Enum.GetValues(typeof(ETileset)) where Set.All(_vm => _vm.Key != key) && key!=ETileset.NONE select key)
			{
				var set = new XTileSetVM();
				Manager.Instance.XRoot.TileSets.Add(set);
				set.Key = key;
				set.Children.Add(new XTileInfoVM());
			}

            CollectionViewSource.GetDefaultView(Set).SortDescriptions.Add(new SortDescription("KeyName", ListSortDirection.Ascending));
        }

		public ReadOnlyObservableCollection<XTileSetVM> Set { get; private set; }

		public override string DisplayName
		{
			get { return "Tiles"; }
		}
	}
}
