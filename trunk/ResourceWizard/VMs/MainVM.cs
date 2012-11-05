using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
