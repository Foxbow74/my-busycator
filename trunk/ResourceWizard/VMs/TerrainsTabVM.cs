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
			foreach (var vm in from ETerrains key in Enum.GetValues(typeof(ETerrains)) where Set.All(_vm => _vm.Key != key) && key!=ETerrains.NONE select new XTerrainSetVM { Key = key, })
			{
				Manager.Instance.XRoot.TerrainSets.Add(vm);
				vm.Children.Add(new XTileInfoVM());
			}
		}

		public ReadOnlyObservableCollection<XTerrainSetVM> Set { get; private set; }
		
		public override string DisplayName
		{
			get { return "Terrains"; }
		}
	}
}