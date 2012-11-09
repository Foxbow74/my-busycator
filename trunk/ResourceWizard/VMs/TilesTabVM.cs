using System;
using System.Collections.ObjectModel;
using System.Linq;
using GameCore;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard.VMs
{
	internal class TilesTabVM : TabVM
	{
		public TilesTabVM()
		{
			Set = Manager.Instance.XRoot.TileSetsObsCol;
			var todel = Set.Where(_vm => _vm.Key == ETiles.NONE).ToArray();
			foreach (var vm in todel)
			{
				Manager.Instance.XRoot.TileSets.Remove(vm);
			}
			Manager.Instance.Save();

			foreach (var key in from ETiles key in Enum.GetValues(typeof(ETiles)) where Set.All(_vm => _vm.Key != key) && key!=ETiles.NONE select key)
			{
				var set = new XTileSetVM();
				Manager.Instance.XRoot.TileSets.Add(set);
				set.Key = key;
				set.Children.Add(new XTileInfoVM());
			}
		}

		public ReadOnlyObservableCollection<XTileSetVM> Set { get; private set; }

		public override string DisplayName
		{
			get { return "Tiles"; }
		}
	}
}
