using System.Collections.Generic;
using System.Collections.ObjectModel;
using ClientCommonWpf;
using GameCore.Objects;
using GameCore.Storage;
using RusLanguage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XThingInfoVM:XChildObjectVM<XThingInfoVM>
	{
		public override EStoreKind Kind
		{
			get { return EStoreKind.THING_INFO; }
		}

		[X("TILESET")]private IXValue<XTileSetVM> m_tileSetVM;
		[X("TILEINFO")]private IXValue<XTileInfoVM> m_tileInfoVM;
		[X("NAME")]private IXValue<string> m_name;
		[X("SEX")]private IXValue<byte> m_sex;
		[X("CATEGORY")]private IXValue<byte> m_category;
		[X("COLOR")]private IXValue<XColorVM> m_color;
        [X((int)EStoreKind.THING_INFO)]private ICollection<XThingInfoVM> m_thingInfos;

		public XTileSetVM TileSetVM { get { return m_tileSetVM.Value; } set { m_tileSetVM.Value = value; } }
		public XTileInfoVM TileInfoVM { get { return m_tileInfoVM.Value; } set { m_tileInfoVM.Value = value; } }
		public string Name { get { return m_name.Value; } set { m_name.Value = value; } }
		public ESex Sex { get { return (ESex)m_sex.Value; } set { m_sex.Value = (byte)value; } }
		public EThingCategory Category { get { return (EThingCategory)m_category.Value; } set { m_category.Value = (byte)value; } }
		public XColorVM Color { get { return m_color.Value; } set { m_color.Value = value; } }

        public ReadOnlyObservableCollection<XThingInfoVM> ChildrentObsCol { get; private set; }

        public ICollection<XThingInfoVM> Children
        {
            get { return m_thingInfos; }
        }

        protected override void InstantiationFinished()
        {
            base.InstantiationFinished();
            ChildrentObsCol = CreateObservableCollection(m_thingInfos);
            BindProperty(m_name, ()=>Name);
        }

        public XThingInfoVM()
        {
            AddChildCommand = new RelayCommand(ExecuteAddChild, CanExecuteAddChild);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanExecuteDelete);
        }

        protected virtual bool CanExecuteDelete(object _obj)
        {
            return true;
        }

        protected void ExecuteDelete(object _obj)
        {
            if(Parent==null)
            {
                Manager.Instance.XRoot.ThingInfos.Remove(this);
            }
            else
            {
                Parent.Children.Remove(this);
            }
        }

        protected virtual bool CanExecuteAddChild(object _obj)
        {
            return true;
        }

        protected void ExecuteAddChild(object _obj)
        {
            var n = new XThingInfoVM();
            Children.Add(n);
            n.Name = "Новый чилд";
        }
        
        public RelayCommand AddChildCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
	}
}
