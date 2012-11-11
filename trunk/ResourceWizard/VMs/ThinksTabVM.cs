using System.Collections.ObjectModel;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard.VMs
{
    internal class ThinksTabVM : SetTabVM
    {
        public ThinksTabVM()
        {
        }

        public ReadOnlyObservableCollection<XThingInfoVM> Set
        {
            get { return Manager.Instance.XRoot.ThingInfosObsCol; }
        }

        public override string DisplayName
        {
            get { return "Thinks"; }
        }

        public string SearchText { get; set; }

        private string m_selectedPath;
        public string SelectedPath
        {
            get { return m_selectedPath; }
            set { m_selectedPath = value; }
        }

        protected override void ExecuteAdd(object _obj)
        {
            var t = new XThingInfoVM();
            Manager.Instance.XRoot.ThingInfos.Add(t);
            t.Name = "Новый";
        }
    }
}