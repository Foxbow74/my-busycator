using System.Collections.ObjectModel;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard.VMs
{
    internal class ThingsTabVM : SetTabVM
    {
        public ThingsTabVM()
        {
        }

        public ReadOnlyObservableCollection<XThingInfoVM> Set
        {
            get { return Manager.Instance.XRoot.ThingInfosObsCol; }
        }

        public override string DisplayName
        {
            get { return "Things"; }
        }

        public string SearchText { get; set; }

        protected override void ExecuteAdd(object _obj)
        {
            var t = new XThingInfoVM();
            Manager.Instance.XRoot.ThingInfos.Add(t);
            t.Name = "Новый";
        }
    }
}