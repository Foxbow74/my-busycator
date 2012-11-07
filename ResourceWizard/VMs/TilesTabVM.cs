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
			foreach (var vm in from ETiles key in Enum.GetValues(typeof(ETiles)) where Set.All(_vm => _vm.Key != key) && key!=ETiles.NONE select new XTileSetVM { Key = key, })
			{
				Manager.Instance.XRoot.TileSets.Add(vm);
				vm.Children.Add(new XTileInfoVM());
			}
		}

		public ReadOnlyObservableCollection<XTileSetVM> Set { get; private set; }

		public override string DisplayName
		{
			get { return "Tiles"; }
		}
	}
}
