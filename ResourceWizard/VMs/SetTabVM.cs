using ClientCommonWpf;

namespace ResourceWizard.VMs
{
    abstract class SetTabVM: AbstractViewModel
    {
        protected SetTabVM()
        {
            AddCommand = new RelayCommand(ExecuteAdd, CanExecuteAdd);
        }

        protected virtual bool CanExecuteAdd(object _obj)
        {
            return true;
        }

        protected abstract void ExecuteAdd(object _obj);

        public RelayCommand AddCommand { get; private set; }
    }
}