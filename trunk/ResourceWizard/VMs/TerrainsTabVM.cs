using System;
using System.Collections.ObjectModel;
using System.Linq;
using GameCore;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard.VMs
{
	class TerrainsTabVM : TabVM
	{

		public TerrainsTabVM()
		{
			Set = Manager.Instance.XRoot.TerrainSetsObsCol;
			var todel = Set.Where(_vm => _vm.Key == ETerrains.NONE).ToArray();
			foreach (var vm in todel)
			{
				Manager.Instance.XRoot.TerrainSets.Remove(vm);
			}
			Manager.Instance.Save();

			foreach (var key in from ETerrains key in Enum.GetValues(typeof(ETerrains)) where Set.All(_vm => _vm.Key != key) && key != ETerrains.NONE select key)
			{
				var set = new XTerrainSetVM();
				Manager.Instance.XRoot.TerrainSets.Add(set);
				set.Key = key;
				set.Children.Add(new XTileInfoVM());
			}
		}

		public ReadOnlyObservableCollection<XTerrainSetVM> Set { get; private set; }
		
		public override string DisplayName
		{
			get { return "Terrains"; }
		}
	}
}