using System.Collections.ObjectModel;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard.VMs
{
	class TilesTabVM : TabVM
	{
		public ReadOnlyObservableCollection<XOldTileInfoVM> Tiles { get { return Manager.Instance.XRoot.TileInfosObsCol; } }

		public TilesTabVM()
		{
		}


		public override string DisplayName
		{
			get { return "Tiles"; }
		}
	}
}
