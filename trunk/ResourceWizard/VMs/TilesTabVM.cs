using System.Collections.ObjectModel;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard.VMs
{
	class TilesTabVM : TabVM
	{
		public ReadOnlyObservableCollection<XTileSetVM> Set { get { return Manager.Instance.XRoot.TileSetsObsCol; } }

		public override string DisplayName
		{
			get { return "Tiles"; }
		}
	}
}
