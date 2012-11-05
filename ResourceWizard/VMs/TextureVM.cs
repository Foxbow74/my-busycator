using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using ClientCommonWpf;
using GameUi;

namespace ResourceWizard.VMs
{
	internal class TextureVM : AbstractViewModel
	{
		private ETextureSet m_texture;
		private double m_cursorX;
		private double m_cursorY;

		public ETextureSet Texture
		{
			get { return m_texture; }
			set
			{
				m_texture = value;
				OnPropertyChanged(() => TextureSource);
			}
		}

		public TextureVM()
		{
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
			
		}

		private void ExecuteSetCommand(object _obj)
		{
			
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

		public double CursorX
		{
			get { return m_cursorX; }
			set
			{
				m_cursorX = value;
				OnPropertyChanged(() => CursorX);
			}
		}

		public double CursorY
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
