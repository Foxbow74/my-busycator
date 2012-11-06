using System.Collections.ObjectModel;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard.VMs
{
	class TerrainsTabVM : TabVM
	{
		public ReadOnlyObservableCollection<XTerrainSetVM> Set { get { return Manager.Instance.XRoot.TerrainSetsObsCol; } }
		
		public override string DisplayName
		{
			get { return "Terrains"; }
		}
	}
}