﻿using System;
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

		private readonly int m_tileSizeX;
		private readonly int m_tileSizeY;
		private OpenTKDrawHelper m_drawHelper;
		private OpenTKResourceProvider m_resourceProvider;

		public OpenTKGameProvider(int _tileSizeX, int _tileSizeY, int _screenWidth, int _screenHeight)
			: base(_tileSizeX * (int)(_screenWidth / _tileSizeX), _tileSizeX * (int)(_screenHeight / _tileSizeX), GraphicsMode.Default, "Open TK")
		{
			m_tileSizeX = _tileSizeX;
			m_tileSizeY = _tileSizeY;
			m_core.Reset();
			Keyboard.KeyDown += KeyboardKeyDown;
			Keyboard.KeyUp += Keyboard_KeyUp;
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

		public void Clear(Color _color)
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
			get { return (int) Math.Round((double) Width/m_tileSizeX); }
		}

		public int HeightInCells
		{
			get { return (int) Math.Round((double) Height/m_tileSizeY); }
		}

		public bool IsActive
		{
			get { return Focused; }
		}

		#endregion

		protected override void OnRenderFrame(FrameEventArgs _e)
		{
			base.OnRenderFrame(_e);
			m_drawHelper.DrawTextBitmap();
		}

		protected override void OnLoad(EventArgs _e)
		{
			m_core.Init();
			m_resourceProvider = new OpenTKResourceProvider(this);
			m_drawHelper = new OpenTKDrawHelper(m_resourceProvider, this);
		}


		protected override void OnUnload(EventArgs _e)
		{
			m_resourceProvider.Dispose();
			m_drawHelper.Dispose();
		}


		protected override void OnResize(EventArgs _e)
		{
			m_core.Reset();
			m_core.Resize(Width, Height);
			m_drawHelper.Resize(Width, Height);
			base.OnResize(_e);
		}

		private void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
		{
			var key = e.Key;

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

		private void KeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
		{
			var key = e.Key;

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
				_consoleKey = (ConsoleKey) (_key - (Key.A - (int)ConsoleKey.A));
				return true;
			}
			if (_key >= Key.Number0 && _key <= Key.Number9)
			{
				_consoleKey = (ConsoleKey)(_key - (Key.Number0 - (int)ConsoleKey.D0));
				return true;
			}
			if (_key >= Key.Keypad0 && _key <= Key.Keypad9)
			{
				_consoleKey = (ConsoleKey)(_key - (Key.Keypad0 - (int)ConsoleKey.NumPad0));
				return true;
			}
			switch (_key)
			{
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