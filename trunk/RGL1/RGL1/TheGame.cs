using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Common.Messages;
using GameCore;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RGL1.UIBlocks;

namespace RGL1
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class TheGame : Game
	{
		DateTime m_lastMove = DateTime.Now;
		readonly List<Keys> m_downKeys = new List<Keys>();
		readonly Queue<Tuple<ConsoleKey, EKeyModifiers>> m_pressed = new Queue<Tuple<ConsoleKey, EKeyModifiers>>();
		EKeyModifiers m_keyModifiers = EKeyModifiers.NONE;
		readonly Keys[] m_keyModificators = new[] { Keys.RightShift, Keys.LeftShift, Keys.RightControl, Keys.LeftControl, Keys.RightAlt, Keys.LeftAlt };

		private int m_second = 0;
		private int m_frames = 0;
		private int m_fps = 0;

		const int AUTO_MOVE_REPEAT_MILLISECONDS = 100;
		const int AUTO_MOVE_REPEAT_AFTER = 200;
		private bool m_isAutoRepeateMode = false;

		private DateTime m_moveKeyHoldedSince;

		private readonly GraphicsDeviceManager m_graphics;
		private SpriteBatch m_spriteBatch;

		private readonly Stack<UIBlock> m_uiBlocks = new Stack<UIBlock>();
		private MainBlock m_mainBlock;

		private Texture2D m_sceneTexture;

		private World m_world;

		private Keys[] m_moveKeys = new Keys[]
			               	{
			               		Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4,
			               		Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.NumPad5, Keys.Home, Keys.PageUp,
			               		Keys.PageDown, Keys.End
			               	};


		public TheGame()
		{
			IsMouseVisible = true;
			MessageManager.NewMessage += MessageManagerNewMessage;
			
			m_graphics = new GraphicsDeviceManager(this);
			if(!InitGraphicsMode(1024, 768, false)) Exit();
			Content.RootDirectory = "Content";
		}

		void MessageManagerNewMessage(object _sender, Message _message)
		{
			if(_message.Type!=EMessageType.SYSTEM)
			{
				return;
			}
			if (_message is OpenUIBlockMessage)
			{
				m_uiBlocks.Push(((OpenUIBlockMessage)_message).UIBlock);	
			}
			else if (_message is SystemMessage)
			{
				switch (((SystemMessage)_message).Message)
				{
					case SystemMessage.ESystemMessage.EXIT_GAME:
						Exit();
						break;
					case SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK:
						m_uiBlocks.Pop().Dispose();
						m_pressed.Clear();
						m_downKeys.Clear();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				throw new NotImplementedException("Can't process " + _message.GetType() + " message.");
			}
		}

		private bool InitGraphicsMode(int _width, int _height, bool _fullScreen)
		{
			_width = (int)Math.Round((decimal)_width / Tile.Size) * Tile.Size;
			_height = (int)Math.Round((decimal)_height / Tile.Size) * Tile.Size;

			// If we aren't using a full screen mode, the height and width of the window can
			// be set to anything equal to or smaller than the actual screen size.)
			if (_fullScreen == false)
			{
				if ((_width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
					&& (_height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
				{
					m_graphics.PreferredBackBufferWidth = _width;
					m_graphics.PreferredBackBufferHeight = _height;
					m_graphics.IsFullScreen = false;
					m_graphics.ApplyChanges();
					return true;
				}
			}
			else
			{
				var dms = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes
						.Where(_mode => _mode.Width == _width && _mode.Height == _height);
				var dm = dms.FirstOrDefault();

				if (dm != default(DisplayMode))
				{
					// The mode is supported, so set the buffer formats, apply changes and return
					m_graphics.PreferredBackBufferWidth = _width;
					m_graphics.PreferredBackBufferHeight = _height;
					m_graphics.PreferredBackBufferFormat = dm.Format;
					m_graphics.IsFullScreen = true;
					m_graphics.ApplyChanges();
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			Tile.Init(Content);
			
			m_world = new World();
	
			m_mainBlock = new MainBlock(GraphicsDevice, m_world);

			m_uiBlocks.Push(m_mainBlock);

			// Create a new SpriteBatch, which can be used to draw textures.
			GraphicsDevice.Clear(Color.Orange);

			var pixels = new Color[GraphicsDevice.PresentationParameters.BackBufferWidth * GraphicsDevice.PresentationParameters.BackBufferHeight];
			m_sceneTexture = new Texture2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
			GraphicsDevice.GetBackBufferData(pixels);
			m_sceneTexture.SetData(pixels); 


			m_spriteBatch = new SpriteBatch(GraphicsDevice);
			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="_gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime _gameTime)
		{
			// TODO: Add your update logic here

			var state = Keyboard.GetState();

			var keyModifiers = (state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift)) ? EKeyModifiers.SHIFT : EKeyModifiers.NONE;
			keyModifiers |= (state.IsKeyDown(Keys.LeftControl) || state.IsKeyDown(Keys.RightControl)) ? EKeyModifiers.CTRL : EKeyModifiers.NONE;
			keyModifiers |= (state.IsKeyDown(Keys.LeftAlt) || state.IsKeyDown(Keys.RightAlt)) ? EKeyModifiers.ALT : EKeyModifiers.NONE;

			
			var downKeys = state.GetPressedKeys().Except(m_keyModificators).ToArray();

			if (keyModifiers != m_keyModifiers) m_downKeys.Clear();

			var pressedKeys = new List<Keys>();
			var prevDownKeys = m_downKeys.ToArray();

			foreach (var key in prevDownKeys)
			{
				if (downKeys.Contains(key)) continue;

				pressedKeys.Add(key);
				m_downKeys.Remove(key);
			}
			foreach (var key in downKeys)
			{
				if (m_downKeys.Contains(key)) continue;

				m_moveKeyHoldedSince = DateTime.Now;
				m_downKeys.Add(key);
			}

			m_keyModifiers = keyModifiers;

			if(m_downKeys.Except(m_moveKeys).Any() || pressedKeys.Any())
			{
				m_isAutoRepeateMode = false;
			}
			else
			{
				if (m_downKeys.Intersect(m_moveKeys).Any())
				{
					var totalMilliseconds = (DateTime.Now - m_moveKeyHoldedSince).TotalMilliseconds;
					if (m_isAutoRepeateMode)
					{
						if (totalMilliseconds > AUTO_MOVE_REPEAT_MILLISECONDS)
						{
							m_moveKeyHoldedSince = DateTime.Now;
							pressedKeys.AddRange(m_downKeys);
						}
					}
					else
					{
						if (totalMilliseconds > AUTO_MOVE_REPEAT_AFTER)
						{
							m_isAutoRepeateMode = true;
						}
					}
				}
			}

			foreach (var pressedKey in pressedKeys)
			{
				m_pressed.Enqueue(new Tuple<ConsoleKey, EKeyModifiers>((ConsoleKey)pressedKey, m_keyModifiers));
			}

			if (m_pressed.Count>0)
			{
				var tuple = m_pressed.Dequeue();
				m_uiBlocks.Peek().KeysPressed(tuple.Item1, tuple.Item2);
			}

			base.Update(_gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="_gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime _gameTime)
		{
			if(!IsActive) return;

			foreach (var uiBlock in m_uiBlocks.Reverse())
			{
				uiBlock.Draw(_gameTime, m_spriteBatch);
			}
			base.Draw(_gameTime);

			var tm = DateTime.Now;
			if (tm.Second == m_second)
			{
				m_frames++;
			}
			else
			{
				m_fps = m_frames;
				m_frames = 0;
				m_second = tm.Second;
			}

			m_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

			var format = string.Format("тоя:{0} XY:{1}", m_fps, m_mainBlock.World.Avatar.Coords);

			m_spriteBatch.WriteString(format, 1, 1, Color.White, Color.Gray, Tile.Font);

			m_spriteBatch.End();
		}
	}
}