using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
		readonly List<Keys> m_downKeys = new List<Keys>();
		readonly Queue<Tuple<ConsoleKey, EKeyModifiers>> m_pressed = new Queue<Tuple<ConsoleKey, EKeyModifiers>>();
		EKeyModifiers m_keyModifiers = EKeyModifiers.NONE;
		readonly Keys[] m_keyModificators = new[] { Keys.RightShift, Keys.LeftShift, Keys.RightControl, Keys.LeftControl, Keys.RightAlt, Keys.LeftAlt };

		private int m_second;
		private int m_frames;
		private int m_fps;

		const int AUTO_MOVE_REPEAT_MILLISECONDS = 100;
		const int AUTO_MOVE_REPEAT_AFTER = 200;
		private bool m_isAutoRepeateMode;

		private DateTime m_moveKeyHoldedSince;

		private DateTime m_lastUpdate;

		private readonly GraphicsDeviceManager m_graphics;
		private SpriteBatch m_spriteBatch;

		private readonly Stack<UIBlock> m_uiBlocks = new Stack<UIBlock>();
		private MainBlock m_mainBlock;

		private Texture2D m_sceneTexture;

		private World m_world;

		private readonly Keys[] m_moveKeys = new[]
			               	{
			               		Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4,
			               		Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.NumPad5, Keys.Home, Keys.PageUp,
			               		Keys.PageDown, Keys.End
			               	};


		public TheGame()
		{
			m_frames = 0;
			IsMouseVisible = true;
			MessageManager.NewMessage += MessageManagerNewMessage;
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
			
			m_graphics = new GraphicsDeviceManager(this);
			if(!InitGraphicsMode(1024, 768, false)) Exit();
			Content.RootDirectory = "Content";
		}

		static void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
		}

		void MessageManagerNewMessage(object _sender, Message _message)
		{
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
		}

		private bool InitGraphicsMode(int _width, int _height, bool _fullScreen)
		{
			_width = (int)Math.Round((decimal)_width / Tile.Size) * Tile.Size;
			_height = (int)Math.Round((decimal)_height / Tile.Size) * Tile.Size;

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

			if (m_pressed.Count>0 && m_world.Avatar.GetNextAct()==null)
			{
				var tuple = m_pressed.Dequeue();
				m_uiBlocks.Peek().KeysPressed(tuple.Item1, tuple.Item2);
			}

			//if ((DateTime.Now - m_lastUpdate).TotalMilliseconds>10)
			{
				//Debug.WriteLine((DateTime.Now - m_lastUpdate).TotalMilliseconds);
				m_lastUpdate = DateTime.Now;
				m_world.GameUpdated();
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
				m_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
				uiBlock.DrawBackground(m_spriteBatch);
				m_spriteBatch.End();

				uiBlock.DrawContent(m_spriteBatch);
				uiBlock.DrawFrame(m_spriteBatch);
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

			var format = string.Format("���:{0} XY:{1}", m_fps, m_mainBlock.World.Avatar.Coords);

			m_spriteBatch.WriteString(format, 1, 1, Color.White, Color.Gray, Tile.Font);

			m_spriteBatch.End();
		}
	}
}