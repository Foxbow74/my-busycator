using System.Collections.ObjectModel;
using ClientCommonWpf;

namespace ResourceWizard.VMs
{
	class MainVM
	{
        public ObservableCollection<AbstractViewModel> Tabs { get; private set; }

		public MainVM()
		{
			SaveCommand = new RelayCommand(_o => Manager.Instance.Save(), CanExecuteSave);
			ShrinkCommand = new RelayCommand(_o => Manager.Instance.Shrink());
			Tabs = new ObservableCollection<AbstractViewModel>
				       {
					       new TilesTabVM(),
						   new TerrainsTabVM(),
				       };
		}

	    private bool CanExecuteSave(object _obj)
	    {
            return Manager.Instance.HasChanges;
	    }

		public RelayCommand SaveCommand { get; private set; }
		public RelayCommand ShrinkCommand { get; private set; }
	}
}
