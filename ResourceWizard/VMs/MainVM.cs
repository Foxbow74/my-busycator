using System.Collections.ObjectModel;

namespace ResourceWizard.VMs
{
	class MainVM
	{
		public ObservableCollection<TabVM> Tabs { get; private set; }

		public MainVM()
		{
			Tabs = new ObservableCollection<TabVM>
				       {
					       new TilesTabVM()
				       };
		}
	}
}
