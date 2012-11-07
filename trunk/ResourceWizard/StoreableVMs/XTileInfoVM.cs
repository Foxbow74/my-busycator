using System.Drawing;
using System.Linq;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClientCommonWpf;
using GameCore;
using GameCore.Storage;
using GameUi;
using ResourceWizard.Properties;
using ResourceWizard.VMs;
using XTransport;
using XTransport.WPF;
using Brush = System.Windows.Media.Brush;

namespace ResourceWizard.StoreableVMs
{
	class XTileInfoVM : ClientXChildObjectVM<EStoreKind, XAbstractTileSetVM>
	{
	    [X("TEXTURE")]private readonly IXValue<int> m_eTexture;
	    [X("GRAYSCALE")]private readonly IXValue<bool> m_grayScale;
	    [X("REMOVE_TRANSPARENCY")]private readonly IXValue<bool> m_removeTransparency;
	    [X("Color")]private IXValue<XColorVM> m_color;
	    [X("CX")]private IXValue<int> m_cx;
	    [X("CY")]private IXValue<int> m_cy;
	    [X("Order")]private IXValue<int> m_order;
	    [X("X")]private IXValue<int> m_x;
	    [X("Y")]private IXValue<int> m_y;

        private TextureVM m_textureVM;
        private FColor m_fColor;

	    public XTileInfoVM()
	    {
	        SelectColorCommand = new RelayCommand(ExecuteSelectColorCommand);
	        DublicateCommand = new RelayCommand(ExecuteDublicateCommand);
	        DeleteCommand = new RelayCommand(ExecuteDeleteCommand, _o => Parent.Children.Count>1);
	        MoveLeftCommand = new RelayCommand(ExecuteMoveLeftCommand, _o => Parent.Children.Any(_vm => _vm.Order<Order));
	        MoveRightCommand = new RelayCommand(ExecuteMoveRightCommand, _o => Parent.Children.Any(_vm => _vm.Order>Order));
	        RefreshMosaicCommand = new RelayCommand(_o => Parent.UpdateMosaic());
	        CopyCommand = new RelayCommand(_o => Manager.Instance.TileBuffer = this);
	        PasteCommand = new RelayCommand(ExecutePasteCommand, _o => Manager.Instance.TileBuffer!=null);
	    }

	    public FColor FColor
		{
			get
			{
				if(m_fColor.Equals(default(FColor)))
				{
					m_fColor = Color.GetFColor();
				}
				return m_fColor;
			}
			set
			{
				m_fColor = value;
				OnPropertyChanged(()=>FColor);
				OnPropertyChanged(() => Brush);
				RefreshImage();
			}
		}

		public BitmapSource CopyImage { get { return Resources.copy.Source(); } }
		public BitmapSource PasteImage { get { return Resources.paste.Source(); } }
		public BitmapSource DuplicateImage { get { return Resources.components.Source(); } }
		public BitmapSource DeleteImage { get { return Resources.delete2.Source(); } }
		public BitmapSource LeftImage { get { return Resources.navigate_left.Source(); } }
		public BitmapSource RightImage { get { return Resources.navigate_right.Source(); } }
		public BitmapSource ColorsImage { get { return Resources.colors.Source(); } }
        public BitmapSource GrayScaleImage { get { return Resources.yinyang.Source(); } }
        
		public BitmapSource CopyImageD { get { return Resources.copy.SourceDisabled(); } }
		public BitmapSource PasteImageD { get { return Resources.paste.SourceDisabled(); } }
		public BitmapSource DuplicateImageD { get { return Resources.components.SourceDisabled(); } }
		public BitmapSource DeleteImageD { get { return Resources.delete2.SourceDisabled(); } }
		public BitmapSource LeftImageD { get { return Resources.navigate_left.SourceDisabled(); } }
		public BitmapSource RightImageD { get { return Resources.navigate_right.SourceDisabled(); } }
		public BitmapSource ColorsImageD { get { return Resources.colors.SourceDisabled(); } }
        
		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_INFO; }
		}

	    public int Order
		{
			get { return m_order.Value; }
			set { m_order.Value = value; }
		}

	    public int X { get { return m_x.Value; } set { m_x.Value = value; } }
		public int Y { get { return m_y.Value; } set { m_y.Value = value; } }
		public int CX { get { return m_cx.Value; } set { m_cx.Value = value; } }
		public int CY { get { return m_cy.Value; } set { m_cy.Value = value; } }

		public XColorVM Color
		{
			get { return m_color.Value; }
			set
			{
				m_color.Value = value;
			}
		}

		public ETextureSet Texture
		{
			get { return (ETextureSet) m_eTexture.Value; }
			set
			{
				m_eTexture.Value = (int) value;
			}
		}

	    public TextureVM TextureVM
		{
			get { return m_textureVM??(m_textureVM=new TextureVM(this)); }
		}

		public BitmapSource Image { get { return Bitmap.Source(); } }

		public Brush Brush { get { return new SolidColorBrush(FColor.GetColor()); } }

		public RelayCommand SelectColorCommand { get; private set; }

		public RelayCommand DublicateCommand { get; private set; }

		public RelayCommand MoveLeftCommand { get; private set; }

		public RelayCommand MoveRightCommand{ get; private set; }

		public RelayCommand DeleteCommand { get; private set; }

        public bool RemoveTransparency
        {
            get { return m_removeTransparency.Value; }
            set { m_removeTransparency.Value = value; RefreshImage(); }
        }

        public bool GrayScale
        {
            get { return m_grayScale.Value; }
            set { m_grayScale.Value = value; RefreshImage(); }
        }

		public float ColorR { get { return FColor.R; } set { FColor = new FColor(1, value, FColor.G, FColor.B); } }
		public float ColorG { get { return FColor.G; } set { FColor = new FColor(1, FColor.R, value, FColor.B); } }
		public float ColorB { get { return FColor.B; } set { FColor = new FColor(1, FColor.R, FColor.G, value); } }

		public RelayCommand RefreshMosaicCommand { get; private set; }

		public RelayCommand CopyCommand { get; private set; }
		public RelayCommand PasteCommand { get; private set; }

	    public Bitmap Bitmap
	    {
            get { return Manager.Instance[Texture, X, Y, FColor, RemoveTransparency, GrayScale]; }
	    }

	    private void ExecutePasteCommand(object _obj)
	    {
	        var b = Manager.Instance.TileBuffer;

	        var d = new XTileInfoVM { X = b.X, Y = b.Y, FColor = b.FColor, Texture = b.Texture, Order = Order + 1 };
	        foreach (var vm in Parent.Children)
	        {
	            if (vm.Order > Order)
	            {
	                vm.Order++;
	            }
	        }
	        Parent.Children.Add(d);
	        Parent.SelectedItem = d;
	    }

	    private void ExecuteDeleteCommand(object _obj)
	    {
	        Parent.Children.Remove(this);
	        Parent.SelectedItem = Parent.Children.OrderBy(_vm => _vm.Order).Last(_vm => _vm.Order < Order);
	    }

	    private void ExecuteMoveRightCommand(object _obj)
	    {
	        Parent.Children.OrderBy(_vm => _vm.Order).First(_vm => _vm.Order > Order).Order--;
	        Order++;
	        CollectionViewSource.GetDefaultView(Parent.ChildrenObsCol).Refresh();
	    }

	    private void ExecuteMoveLeftCommand(object _obj)
	    {
	        Parent.Children.OrderBy(_vm => _vm.Order).Last(_vm => _vm.Order < Order).Order++;
	        Order--;
	        CollectionViewSource.GetDefaultView(Parent.ChildrenObsCol).Refresh();
	    }

	    private void ExecuteDublicateCommand(object _obj)
	    {
	        var d = new XTileInfoVM {X = X, Y = Y, FColor = FColor, Texture = Texture, Order = Order+1};
	        foreach (var vm in Parent.Children)
	        {
	            if(vm.Order>Order)
	            {
	                vm.Order++;
	            }
	        }
	        Parent.Children.Add(d);
	        Parent.SelectedItem = d;
	    }

	    private void ExecuteSelectColorCommand(object _obj)
	    {
	        Manager.Instance.COLOR_DIALOG.Color = FColor.GetDColor();
	        if (Manager.Instance.COLOR_DIALOG.ShowDialog() != DialogResult.Cancel)
	        {
	            FColor = Manager.Instance.COLOR_DIALOG.Color.GetFColor();;
	        }
	    }

	    protected override void InstantiationFinished()
	    {
	        base.InstantiationFinished();
	        BindProperty(m_eTexture, () => Texture);
	        BindProperty(m_x, () => X);
	        BindProperty(m_y, () => Y);
	        BindProperty(m_cx, () => CX);
	        BindProperty(m_cy, () => CY);
	        BindProperty(m_order, ()=>Order);
	    }

	    public void RefreshImage()
		{
			OnPropertyChanged(() => Image);
		}

		public void BeforeSave()
		{
			if (!Color.GetFColor().Equals(FColor))
			{
				Color = m_fColor.GetXColorVM();
			}
		}
	}
}
