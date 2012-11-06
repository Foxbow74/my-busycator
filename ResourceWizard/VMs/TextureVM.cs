using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using ClientCommonWpf;
using GameCore;
using GameUi;
using ResourceWizard.StoreableVMs;

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
			Texture = _xTileInfoVM.Texture;
			CursorX = _xTileInfoVM.X * Constants.TILE_SIZE;
			CursorY = _xTileInfoVM.Y * Constants.TILE_SIZE;
			m_xTileInfoVM = _xTileInfoVM;
			Sets = new ObservableCollection<ETextureSet>(Enum.GetValues(typeof(ETextureSet)).Cast<ETextureSet>());
			TextureClick = new RelayCommand(ExecuteTextureClick);
			SetCommand = new RelayCommand(ExecuteSetCommand, CanSetCommand);
			AddCommand = new RelayCommand(ExecuteAddCommand, CanAddCommand);
		}

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
			var vm = new XTileInfoVM {Texture = Texture, X = CursorX/Constants.TILE_SIZE, Y = CursorY/Constants.TILE_SIZE, Color = m_xTileInfoVM.Color, Order = m_xTileInfoVM.Parent.Children.Max(_vm => _vm.Order) + 1};
			m_xTileInfoVM.Parent.Children.Add(vm);
			m_xTileInfoVM.Parent.SelectedItem = vm;
		}

		private void ExecuteSetCommand(object _obj)
		{
			m_xTileInfoVM.Texture = Texture;
			m_xTileInfoVM.X = CursorX / Constants.TILE_SIZE;
			m_xTileInfoVM.Y = CursorY / Constants.TILE_SIZE;
			m_xTileInfoVM.RefreshImage();
		}

		private void ExecuteTextureClick(object _o)
		{
			CursorX = (int)(MousePoint.X / 16) * 16;
			CursorY = (int)(MousePoint.Y / 16) * 16;
			OnPropertyChanged(() => CursorX);
			OnPropertyChanged(() => CursorY);
		}

		public ObservableCollection<ETextureSet> Sets { get; private set; }

		public BitmapSource TextureSource
		{
			get { return Manager.Instance[Texture].Source(); }
		}

		public RelayCommand TextureClick { get; private set; }

		public Point MousePoint { get; set; }

		public int CursorX
		{
			get { return m_cursorX; }
			set
			{
				m_cursorX = value;
				OnPropertyChanged(() => CursorX);
			}
		}

		public int CursorY
		{
			get { return m_cursorY; }
			set
			{
				m_cursorY = value;
				OnPropertyChanged(() => CursorY);
			}
		}

		public RelayCommand SetCommand { get; private set; }

		public RelayCommand AddCommand { get; private set; }
	}
}
