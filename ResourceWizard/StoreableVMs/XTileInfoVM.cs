using System.Linq;
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
			TextureVM = new TextureVM(){Texture = Texture, CursorX = X * Constants.TILE_SIZE, CursorY = Y * Constants.TILE_SIZE};
			SelectColorCommand = new RelayCommand(ExecuteSelectColorCommand);
			DublicateCommand  = new RelayCommand(ExecuteDublicateCommand);
			MoveLeftCommand = new RelayCommand(ExecuteMoveLeftCommand, _o => Parent.Children.Any(_vm => _vm.Order<Order));
			MoveRightCommand = new RelayCommand(ExecuteMoveRightCommand, _o => Parent.Children.Any(_vm => _vm.Order>Order));
		}

		private void ExecuteMoveRightCommand(object _obj)
		{
			throw new System.NotImplementedException();
		}

		private void ExecuteMoveLeftCommand(object _obj)
		{
			throw new System.NotImplementedException();
		}

		private void ExecuteDublicateCommand(object _obj)
		{
			throw new System.NotImplementedException();
		}

		private ColorDialog m_cd = new ColorDialog();
		private void ExecuteSelectColorCommand(object _obj)
		{
			m_cd.Color = WColor;
			if (m_cd.ShowDialog() != DialogResult.Cancel)
			{
				Color = new FColor(m_cd.Color.A / 255f, m_cd.Color.R / 255f, m_cd.Color.G / 255f, m_cd.Color.B / 255f);
				OnPropertyChanged(()=>Image);
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

		public TextureVM TextureVM { get; private set; }

		public BitmapSource Image { get { return Manager.Instance[Texture, X, Y, WColor, Color].Source(); } }

		public Brush Brush { get { return new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)(Color.A * 255), (byte)(Color.R * 255), (byte)(Color.G * 255), (byte)(Color.B * 255))); } }

		public RelayCommand SelectColorCommand { get; private set; }

		public RelayCommand DublicateCommand { get; private set; }

		public RelayCommand MoveLeftCommand { get; private set; }

		public RelayCommand MoveRightCommand{ get; private set; }

		public RelayCommand DeleteCommand { get; private set; }
	}
}
