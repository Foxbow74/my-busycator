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

        protected override void ExecuteAdd(object _obj)
        {
            var t = new XThingInfoVM();
            Manager.Instance.XRoot.ThingInfos.Add(t);
            t.Name = "Новый";
        }
    }
}