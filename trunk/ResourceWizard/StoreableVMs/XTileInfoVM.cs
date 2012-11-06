using System.Linq;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClientCommonWpf;
using GameCore;
using GameCore.Storage;
using GameUi;
using ResourceWizard.VMs;
using XTransport;
using XTransport.WPF;

namespace ResourceWizard.StoreableVMs
{
	class XTileInfoVM : ClientXChildObjectVM<EStoreKind, XAbstractTileSetVM>
	{
		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_INFO; }
		}

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

		private void ExecutePasteCommand(object _obj)
		{
			var b = Manager.Instance.TileBuffer;

			var d = new XTileInfoVM { X = b.X, Y = b.Y, Color = b.Color, Texture = b.Texture, Order = Order + 1 };
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
			var d = new XTileInfoVM {X = X, Y = Y, Color = Color, Texture = Texture, Order = Order+1};
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
			Manager.Instance.ColorDialog.Color = WColor;
			if (Manager.Instance.ColorDialog.ShowDialog() != DialogResult.Cancel)
			{
				Color = new FColor(Manager.Instance.ColorDialog.Color.A / 255f, Manager.Instance.ColorDialog.Color.R / 255f, Manager.Instance.ColorDialog.Color.G / 255f, Manager.Instance.ColorDialog.Color.B / 255f);
			}
		}

		public System.Drawing.Color WColor
		{
			get
			{
				var clr = Color.Multiply(255f);
				return System.Drawing.Color.FromArgb((int)clr.A, (int)clr.R, (int)clr.G, (int)clr.B);
			}
		}

		[X("Order")]
		private IXValue<int> m_order;

		public int Order
		{
			get { return m_order.Value; }
			set { m_order.Value = value; }
		}

		[X("TEXTURE")]private readonly IXValue<int> m_eTexture;
		[X("X")]private IXValue<int> m_x;
		[X("Y")]private IXValue<int> m_y;
		[X("Color")]private IXValue<string> m_color;
		[X("CX")]private IXValue<int> m_cx;
		[X("CY")]private IXValue<int> m_cy;
		private bool m_removeTransparency;
		private TextureVM m_textureVM;

		public int X { get { return m_x.Value; } set { m_x.Value = value; } }
		public int Y { get { return m_y.Value; } set { m_y.Value = value; } }
		public int CX { get { return m_cx.Value; } set { m_cx.Value = value; } }
		public int CY { get { return m_cy.Value; } set { m_cy.Value = value; } }

		public FColor Color
		{
			get { return FColor.Parse(m_color.Value); }
			set
			{
				m_color.Value = value.ToShortText();
				OnPropertyChanged(()=>Brush);
				RefreshImage();
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

		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			BindProperty(m_color, () => Color);
			BindProperty(m_eTexture, () => Texture);
			BindProperty(m_x, () => X);
			BindProperty(m_y, () => Y);
			BindProperty(m_cx, () => CX);
			BindProperty(m_cy, () => CY);
			BindProperty(m_order, ()=>Order);
		}

		public TextureVM TextureVM
		{
			get { return m_textureVM??(m_textureVM=new TextureVM(this)); }
		}

		public BitmapSource Image { get { return Manager.Instance[Texture, X, Y, WColor, Color, RemoveTransparency].Source(); } }

		public Brush Brush { get { return new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)(Color.A * 255), (byte)(Color.R * 255), (byte)(Color.G * 255), (byte)(Color.B * 255))); } }

		public RelayCommand SelectColorCommand { get; private set; }

		public RelayCommand DublicateCommand { get; private set; }

		public RelayCommand MoveLeftCommand { get; private set; }

		public RelayCommand MoveRightCommand{ get; private set; }

		public RelayCommand DeleteCommand { get; private set; }

		public bool RemoveTransparency
		{
			get { return m_removeTransparency; }
			set
			{
				m_removeTransparency = value;
				RefreshImage();
			}
		}

		public float ColorR { get { return Color.R; } set { Color = new FColor(1, value, Color.G, Color.B); } }
		public float ColorG { get { return Color.G; } set { Color = new FColor(1, Color.R, value, Color.B); } }
		public float ColorB { get { return Color.B; } set { Color = new FColor(1, Color.R, Color.G, value); } }

		public RelayCommand RefreshMosaicCommand { get; private set; }

		public RelayCommand CopyCommand { get; private set; }
		public RelayCommand PasteCommand { get; private set; }

		public void RefreshImage()
		{
			OnPropertyChanged(() => Image);
		}
	}
}
