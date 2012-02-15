using System;
using System.Diagnostics;
using System.Drawing;
using GameCore;
using GameUi;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace OpenTKUi
{
	public class OpenTKGameProvider : GameWindow, IGameProvider
	{
		private readonly Core m_core = new Core();
		private readonly KeyState m_keyState = new KeyState();

		private OpenTKDrawHelper m_drawHelper;
		private OpenTKResourceProvider m_resourceProvider;
		private TileMapRenderer m_tileMapRenderer;

		public OpenTKGameProvider(int _tileSizeX, int _tileSizeY, int _screenWidth, int _screenHeight)
			: base(_tileSizeX*(_screenWidth/_tileSizeX), _tileSizeX*(_screenHeight/_tileSizeX), GraphicsMode.Default, "Open TK")
		{
			TileSizeX = _tileSizeX;
			TileSizeY = _tileSizeY;
			m_core.Reset();
			Keyboard.KeyDown += KeyboardKeyDown;
			Keyboard.KeyUp += KeyboardKeyUp;
		}

		#region IGameProvider Members

		public IResourceProvider ResourceProvider
		{
			get { return m_resourceProvider; }
		}

		public KeyState KeyState
		{
			get { return m_keyState; }
		}

		public void Clear(FColor _color)
		{
			m_core.SetClearColor(_color.R, _color.G, _color.B);
			m_core.Clear();
		}

		public IDrawHelper DrawHelper
		{
			get { return m_drawHelper; }
		}

		public int WidthInCells
		{
			get { return Width/TileSizeX; }
		}

		public int HeightInCells
		{
			get { return Height/TileSizeY; }
		}

		public bool IsActive
		{
			get { return Focused; }
		}

		internal TileMapRenderer TileMapRenderer
		{
			get
			{
				if (m_tileMapRenderer == null)
				{
					m_tileMapRenderer = new TileMapRenderer(Width, Height);
					OpenTKTile.TileMapRenderer = m_tileMapRenderer;
				}
				return m_tileMapRenderer;
			}
		}

		public int TileSizeX { get; private set; }

		public int TileSizeY { get; private set; }

		public void DrawTextLayer()
		{
			m_drawHelper.DrawTextBitmap();
		}

		#endregion

		protected override void OnLoad(EventArgs _e)
		{
			m_core.Init();
			m_resourceProvider = new OpenTKResourceProvider();
			m_drawHelper = new OpenTKDrawHelper(m_resourceProvider, this);
			//TileMapRenderer.Init(TileSizeX, TileSizeY, m_resourceProvider);
		}

		protected override void OnUnload(EventArgs _e)
		{
			m_resourceProvider.Dispose();
			m_drawHelper.Dispose();
			m_tileMapRenderer.Dispose();
		}

		protected override void OnResize(EventArgs _e)
		{
			m_core.Reset();
			m_core.Resize(Width, Height);
			m_drawHelper.Resize(Width, Height);
			if(m_tileMapRenderer!=null)
			{
				TileMapRenderer.Dispose();
				m_tileMapRenderer = null;
			}
			base.OnResize(_e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);
			if (TileMapRenderer.ResourceProvider == null)
			{
				TileMapRenderer.Init(TileSizeX, TileSizeY, m_resourceProvider);
			}
			TileMapRenderer.Iteration++;
		}

		protected virtual void OnRenderFinished()
		{
			TileMapRenderer.Draw();
			DrawTextLayer();
			SwapBuffers();
		}

		private void KeyboardKeyUp(object _sender, KeyboardKeyEventArgs _e)
		{
			var key = _e.Key;

			m_keyState.KeyModifiers ^= (key == Key.ShiftLeft || key == Key.ShiftRight)
			                           	? EKeyModifiers.SHIFT
			                           	: EKeyModifiers.NONE;
			m_keyState.KeyModifiers ^= (key == Key.ControlLeft || key == Key.ControlRight)
			                           	? EKeyModifiers.CTRL
			                           	: EKeyModifiers.NONE;
			m_keyState.KeyModifiers ^= (key == Key.AltLeft || key == Key.AltRight)
			                           	? EKeyModifiers.ALT
			                           	: EKeyModifiers.NONE;

			ConsoleKey consoleKey;
			if (TryParseConsoleKey(key, out consoleKey))
			{
				m_keyState.PressedKeys.Remove(consoleKey);
			}
		}

		private void KeyboardKeyDown(object _sender, KeyboardKeyEventArgs _e)
		{
			var key = _e.Key;

			m_keyState.KeyModifiers |= (key == Key.ShiftLeft || key == Key.ShiftRight)
			                           	? EKeyModifiers.SHIFT
			                           	: EKeyModifiers.NONE;
			m_keyState.KeyModifiers |= (key == Key.ControlLeft || key == Key.ControlRight)
			                           	? EKeyModifiers.CTRL
			                           	: EKeyModifiers.NONE;
			m_keyState.KeyModifiers |= (key == Key.AltLeft || key == Key.AltRight)
			                           	? EKeyModifiers.ALT
			                           	: EKeyModifiers.NONE;

			ConsoleKey consoleKey;
			if (TryParseConsoleKey(key, out consoleKey))
			{
				m_keyState.PressedKeys.Add(consoleKey);
			}
		}

		private static bool TryParseConsoleKey(Key _key, out ConsoleKey _consoleKey)
		{
			if (_key >= Key.A && _key <= Key.Z)
			{
				_consoleKey = (ConsoleKey) (_key - (Key.A - (int) ConsoleKey.A));
				return true;
			}
			if (_key >= Key.Number0 && _key <= Key.Number9)
			{
				_consoleKey = (ConsoleKey) (_key - (Key.Number0 - (int) ConsoleKey.D0));
				return true;
			}
			if (_key >= Key.Keypad0 && _key <= Key.Keypad9)
			{
				_consoleKey = (ConsoleKey) (_key - (Key.Keypad0 - (int) ConsoleKey.NumPad0));
				return true;
			}
			switch (_key)
			{
				case Key.Slash:
					_consoleKey = ConsoleKey.Oem2;
					break;
				case Key.Down:
					_consoleKey = ConsoleKey.DownArrow;
					break;
				case Key.Up:
					_consoleKey = ConsoleKey.UpArrow;
					break;
				case Key.Left:
					_consoleKey = ConsoleKey.LeftArrow;
					break;
				case Key.Right:
					_consoleKey = ConsoleKey.RightArrow;
					break;
				case Key.Home:
					_consoleKey = ConsoleKey.Home;
					break;
				case Key.PageUp:
					_consoleKey = ConsoleKey.PageUp;
					break;
				case Key.PageDown:
					_consoleKey = ConsoleKey.PageDown;
					break;
				case Key.End:
					_consoleKey = ConsoleKey.End;
					break;
				case Key.Enter:
					_consoleKey = ConsoleKey.Enter;
					break;
				case Key.BackSpace:
					_consoleKey = ConsoleKey.Backspace;
					break;
				case Key.Comma:
					_consoleKey = ConsoleKey.OemComma;
					break;
				case Key.Period:
					_consoleKey = ConsoleKey.OemPeriod;
					break;
				case Key.Escape:
					_consoleKey = ConsoleKey.Escape;
					break;
				default:
					var keyName = Enum.GetName(typeof (Key), _key);

					//switch (_key)
					//{
					//    //case Key.One:
					//        _consoleKey = ConsoleKey.D1;
					//        Debug.WriteLine(_key + " ( Delta= " + ((int)_consoleKey - (int)_key));
					//        break;
					//}

					if (Enum.TryParse(keyName, true, out _consoleKey))
					{
						Debug.WriteLine(_key + " ( Delta= " + ((int) _consoleKey - (int) _key));
					}
					else
					{
						Debug.WriteLine(_key);
					}

					return false;
			}
			return true;
		}
	}
}