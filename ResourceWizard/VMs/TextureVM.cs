using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using ClientCommonWpf;
using GameCore;
using ResourceWizard.StoreableVMs;
using XTransport.Server.Storage;

namespace ResourceWizard.VMs
{
	internal class TextureVM : AbstractViewModel
	{
		private readonly XTileInfoVM m_xTileInfoVM;
		private ETextureSet m_texture;
		private int m_cursorX;
		private int m_cursorY;

		public ETextureSet Texture
		{
			get { return m_texture; }
			set
			{
				m_texture = value;
				OnPropertyChanged(() => TextureSource);
			}
		}

		public TextureVM(XTileInfoVM _xTileInfoVM)
		{
			m_xTileInfoVM = _xTileInfoVM;
			Texture = _xTileInfoVM.Texture;
			CursorX = _xTileInfoVM.X;
			CursorY = _xTileInfoVM.Y;
			Sets = new ObservableCollection<ETextureSet>(Enum.GetValues(typeof(ETextureSet)).Cast<ETextureSet>().OrderBy(_set => _set.ToString()));
			TextureClick = new RelayCommand(ExecuteTextureClick);
			SetCommand = new RelayCommand(ExecuteSetCommand, CanSetCommand);
			AddCommand = new RelayCommand(ExecuteAddCommand, CanAddCommand);
			Subscribe(m_xTileInfoVM, info => info.PlusHalfX, _changed => OnPropertyChanged(() => ScreenCursorX));
			Subscribe(m_xTileInfoVM, info => info.PlusHalfY, _changed => OnPropertyChanged(() => ScreenCursorY));
		}

		public int ScreenCursorX { get { return CursorX * Constants.TILE_SIZE + (m_xTileInfoVM.PlusHalfX ? (Constants.TILE_SIZE / 2) : 0); } }
		public int ScreenCursorY { get { return CursorY * Constants.TILE_SIZE + (m_xTileInfoVM.PlusHalfY ? (Constants.TILE_SIZE / 2) : 0); } }

		private bool CanAddCommand(object _obj)
		{
			return true;
		}

		private bool CanSetCommand(object _obj)
		{
			return true;
		}

		private void ExecuteAddCommand(object _obj)
		{
			var d = new XTileInfoVM();
			m_xTileInfoVM.Parent.Children.Add(d);
			d = Manager.Instance.XClient.Get<XTileInfoVM>(d.Uid);
			d.Texture = Texture;
            d.Opacity = 1;
			d.X = CursorX;
			d.Y = CursorY;
			d.Order = m_xTileInfoVM.Parent.Children.Max(_vm => _vm.Order) + 1;
			m_xTileInfoVM.Parent.SelectedItem = d;
            m_xTileInfoVM.Parent.RefreshChildren();
		}

		private void ExecuteSetCommand(object _obj)
		{
			m_xTileInfoVM.Texture = Texture;
			m_xTileInfoVM.X = CursorX;
			m_xTileInfoVM.Y = CursorY;
			m_xTileInfoVM.RefreshImage();
		}

		private void ExecuteTextureClick(object _o)
		{
			CursorX = (int)(MousePoint.X / TileSize);
			CursorY = (int)(MousePoint.Y / TileSize);
		}

		public ObservableCollection<ETextureSet> Sets { get; private set; }

		public BitmapSource TextureSource
		{
			get { return Manager.Instance[Texture, m_xTileInfoVM.Parent is XTerrainSetVM].Source(); }
		}

		public RelayCommand TextureClick { get; private set; }

		public Point MousePoint { get; set; }

		public int TileSize
		{
			get { return Constants.TILE_SIZE; }
		}

		public int CursorX
		{
			get { return m_cursorX; }
			set
			{
				m_cursorX = value;
				OnPropertyChanged(() => CursorX);
				OnPropertyChanged(() => ScreenCursorX);
			}
		}

		public int CursorY
		{
			get { return m_cursorY; }
			set
			{
				m_cursorY = value;
				OnPropertyChanged(() => CursorY);
				OnPropertyChanged(() => ScreenCursorY);
			}
		}

		public RelayCommand SetCommand { get; private set; }

		public RelayCommand AddCommand { get; private set; }
	}
}
