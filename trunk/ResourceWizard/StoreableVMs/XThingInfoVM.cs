﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ClientCommonWpf;
using GameCore;
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

		[X("TILEINFO")]private IXValue<XTileInfoVM> m_tileInfoVM;
		[X("NAME")]private IXValue<string> m_name;
		[X("SEX")]private IXValue<byte> m_sex;
        [X("CATEGORY")]private IXValue<byte> m_category;
        [X("LEVEL")]private IXValue<byte> m_level;
		[X("COLOR")]private IXValue<XColorVM> m_color;
        [X("Opacity")]private IXValue<float> m_opacity;
        [X((int)EStoreKind.THING_INFO)]private ICollection<XThingInfoVM> m_thingInfos;
        [X((int)EStoreKind.TILE_INFO)]private IXValue<XTileInfoVM> m_tileInfo;

		public XTileInfoVM TileInfoVM { get { return m_tileInfoVM.Value; } set { m_tileInfoVM.Value = value; } }
		public string Name { get { return m_name.Value; } set { m_name.Value = value; } }
		public ESex Sex { get { return (ESex)m_sex.Value; } set { m_sex.Value = (byte)value; } }
		public EThingCategory Category { get { return (EThingCategory)m_category.Value; } set { m_category.Value = (byte)value; } }
        public ELevel Level { get { return (ELevel)m_level.Value; } set { m_level.Value = (byte)value; } }
		public XColorVM Color { get { return m_color.Value; } set { m_color.Value = value; } }
	    public XTileInfoVM Tile { get { return m_tileInfo.Value; } set { m_tileInfo.Value = value; } }
        public float Opacity { get { return m_opacity.Value; } set { m_opacity.Value = value; } }

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
            BindProperty(m_tileInfo, () => Tile);
            BindProperty(m_tileInfo, () => TileSet);
            BindProperty(m_opacity, () => Opacity);
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

	    public IEnumerable<EThingCategory> EThingCategories
	    {
	        get 
            {
	            return Enum.GetValues(typeof(EThingCategory)).Cast<EThingCategory>();
	        }
	    }

        public IEnumerable<ELevel> ELevels
        {
            get
            {
                return Enum.GetValues(typeof(ELevel)).Cast<ELevel>();
            }
        }

        public ReadOnlyObservableCollection<XTileSetVM> TileSetsObsCol { get { return Manager.Instance.XRoot.TileSetsObsCol; } }
	    public XTileSetVM TileSet
	    {
	        get
	        {
	            return Tile==null?null:(XTileSetVM)Tile.Parent;
	        }
            set
            {
                if(value!=TileSet)
                {
                    Tile = value.Children.FirstOrDefault();
                }
            }
	    }
	}
}
