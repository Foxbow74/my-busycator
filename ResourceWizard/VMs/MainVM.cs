using System.Collections.ObjectModel;
using ClientCommonWpf;

namespace ResourceWizard.VMs
{
	class MainVM
	{
        public ObservableCollection<AbstractViewModel> Tabs { get; private set; }

		public MainVM()
		{
			SaveCommand = new RelayCommand(_o => Manager.Instance.Save());
			Tabs = new ObservableCollection<AbstractViewModel>
				       {
					       new TilesTabVM(),
						   new TerrainsTabVM(),
				       };
		}

		public RelayCommand SaveCommand { get; private set; }
	}
}
