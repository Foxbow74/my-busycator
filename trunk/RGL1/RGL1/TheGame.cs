using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RGL1.UIBlocks;

namespace RGL1
{
	public class TheGame : Game
	{
		private const int AUTO_MOVE_REPEAT_MILLISECONDS = 100;
		private const int AUTO_MOVE_REPEAT_AFTER = 200;
		private readonly List<Keys> m_downKeys = new List<Keys>();
		private readonly GraphicsDeviceManager m_graphics;

		private readonly Queue<Tuple<ConsoleKey, EKeyModifiers>> m_pressed = new Queue<Tuple<ConsoleKey, EKeyModifiers>>();
		private readonly Stack<UIBlock> m_uiBlocks = new Stack<UIBlock>();

		private int m_fps;
		private int m_frames;

		private bool m_isAutoRepeateMode;
		private EKeyModifiers m_keyModifiers = EKeyModifiers.NONE;

		private MainBlock m_mainBlock;
		private DateTime m_moveKeyHoldedSince;

		private Texture2D m_sceneTexture;
		private int m_second;
		private SpriteBatch m_spriteBatch;

		public TheGame()
		{
			m_frames = 0;
			IsMouseVisible = true;
			MessageManager.NewMessage += MessageManagerNewMessage;
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;

			Content.RootDirectory = "Content";

			m_graphics = new GraphicsDeviceManager(this);
			if (!InitGraphicsMode(800, 600)) Exit();
		}

		private bool InitGraphicsMode(int _width, int _height)
		{
			_width = (int) Math.Round((decimal) _width/Tile.Size)*Tile.Size;
			_height = (int) Math.Round((decimal) _height/Tile.Size)*Tile.Size;

			m_graphics.PreferredBackBufferWidth = _width;
			m_graphics.PreferredBackBufferHeight = _height;

			m_graphics.SynchronizeWithVerticalRetrace = false;
			//m_graphics.IsFullScreen = true;
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += WindowClientSizeChanged;

			m_graphics.ApplyChanges();

			return true;
		}

		private void WindowClientSizeChanged(object _sender, EventArgs _e)
		{
			var blocks = new Stack<UIBlock>();
			do
			{
				var pop = m_uiBlocks.Pop();
				if (pop is MainBlock)
				{
					m_mainBlock = new MainBlock(GraphicsDevice);
					pop = m_mainBlock;
				}
				blocks.Push(pop);
			} while (m_uiBlocks.Count > 0);

			do
			{
				var pop = blocks.Pop();
				m_uiBlocks.Push(pop);
			} while (blocks.Count > 0);
		}

		private static void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
		}

		private void MessageManagerNewMessage(object _sender, Message _message)
		{
			if (_message is OpenUIBlockMessage)
			{
				m_uiBlocks.Push(((OpenUIBlockMessage) _message).UIBlock);
			}
			else if (_message is SystemMessage)
			{
				switch (((SystemMessage) _message).Message)
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
			else if (_message is AskSelectThingsMessage)
			{
				var mess = (AskSelectThingsMessage) _message;
				m_uiBlocks.Push(new SelectItemsUiBlock(m_mainBlock.MapRectangle, mess.ItemDescriptors, mess.Act));
			}
			else if (_message is AskDirectionMessage)
			{
				m_uiBlocks.Push(new AskDirectionUiBlock(m_mainBlock.MapRectangle, (AskDirectionMessage)_message));
			}
			else if(_message is AskHowMuchMessage)
			{
				m_uiBlocks.Push(new AskHowMuchUiBlock(m_mainBlock.MapRectangle, (AskHowMuchMessage)_message));
			}
		}

		/// <summary>
		/// 	LoadContent will be called once per game and is the place to load
		/// 	all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			TileHelper.Init(Content);
			Fonts.Init(Content);
			World.LetItBeeee();

			m_mainBlock = new MainBlock(GraphicsDevice);
			m_uiBlocks.Push(m_mainBlock);

			GraphicsDevice.Clear(Color.Orange);
			m_spriteBatch = new SpriteBatch(GraphicsDevice);

			MessageManager.SendMessage(this, " [?] - экран помощи");
		}

		/// <summary>
		/// 	UnloadContent will be called once per game and is the place to unload
		/// 	all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// 	Allows the game to run logic such as updating the world,
		/// 	checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name = "_gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime _gameTime)
		{
			// TODO: Add your update logic here

			var state = Keyboard.GetState();

			var keyModifiers = (state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))
			                   	? EKeyModifiers.SHIFT
			                   	: EKeyModifiers.NONE;
			keyModifiers |= (state.IsKeyDown(Keys.LeftControl) || state.IsKeyDown(Keys.RightControl))
			                	? EKeyModifiers.CTRL
			                	: EKeyModifiers.NONE;
			keyModifiers |= (state.IsKeyDown(Keys.LeftAlt) || state.IsKeyDown(Keys.RightAlt))
			                	? EKeyModifiers.ALT
			                	: EKeyModifiers.NONE;


			var downKeys = state.GetPressedKeys().Except(KeyHelper.KeyModificators).ToArray();

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

			if (m_downKeys.Except(KeyTranslator.MoveKeys.Cast<Keys>()).Any() || pressedKeys.Any())
			{
				m_isAutoRepeateMode = false;
			}
			else
			{
				if (m_downKeys.Intersect(KeyTranslator.MoveKeys.Cast<Keys>()).Any())
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
				m_pressed.Enqueue(new Tuple<ConsoleKey, EKeyModifiers>((ConsoleKey) pressedKey, m_keyModifiers));
			}

			if (m_pressed.Count > 0)
			{
				if (m_uiBlocks.Peek() != m_mainBlock || World.TheWorld.Avatar.NextAct == null)
				{
					MessageManager.SendMessage(this, WorldMessage.AvatarTurn);

					var tuple = m_pressed.Dequeue();
					m_uiBlocks.Peek().KeysPressed(tuple.Item1, tuple.Item2);
				}
			}

			if (m_uiBlocks.Peek() == m_mainBlock)
			{
				World.TheWorld.GameUpdated();
			}

			base.Update(_gameTime);
		}

		/// <summary>
		/// 	This is called when the game should draw itself.
		/// </summary>
		/// <param name = "_gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime _gameTime)
		{
			if (!IsActive) return;

			foreach (var uiBlock in m_uiBlocks.Reverse())
			{
				m_spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque);
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

			var format = string.Format("ФПС:{0} XY:{1}", m_fps, World.TheWorld.Avatar.Coords);

			m_spriteBatch.WriteString(format, 2, GraphicsDevice.Viewport.Height - 17, Color.White, Color.Gray, Fonts.Font);

			m_spriteBatch.End();
		}
	}
}