using System.Collections.ObjectModel;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard.VMs
{
	class TilesTabVM : TabVM
	{
		public ReadOnlyObservableCollection<XTileSetVM> Tiles { get { return Manager.Instance.XRoot.TileSetsObsCol; } }

		public TilesTabVM()
		{
		}


		public override string DisplayName
		{
			get { return "Tiles"; }
		}
	}
}
