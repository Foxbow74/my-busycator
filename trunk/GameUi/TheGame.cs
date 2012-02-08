using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Messages;
using GameUi.UIBlocks;
using GameUi.UIBlocks.Items;

namespace GameUi
{
	public class TheGame
	{
		private readonly IGameProvider m_gameProvider;
		private const int AUTO_MOVE_REPEAT_MILLISECONDS = 100;
		private const int AUTO_MOVE_REPEAT_AFTER = 200;
		private readonly List<ConsoleKey> m_downKeys = new List<ConsoleKey>();

		private readonly Queue<Tuple<ConsoleKey, EKeyModifiers>> m_pressed = new Queue<Tuple<ConsoleKey, EKeyModifiers>>();
		private readonly Stack<UIBlock> m_uiBlocks = new Stack<UIBlock>();

		private int m_fps;
		private int m_frames;

		private bool m_isAutoRepeateMode;
		private EKeyModifiers m_keyModifiers = EKeyModifiers.NONE;

		private MainBlock m_mainBlock;
		private DateTime m_moveKeyHoldedSince;

		//private Texture2D m_sceneTexture;
		private int m_second;

		public TheGame(IGameProvider _gameProvider)
		{
			m_gameProvider = _gameProvider;
			m_frames = 0;
			MessageManager.NewMessage += MessageManagerNewMessage;
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		public void WindowClientSizeChanged(int _newWidthInCells, int _newHeightInCells)
		{
			var blocks = new Stack<UIBlock>();
			do
			{
				var pop = m_uiBlocks.Pop();
				if (pop is MainBlock)
				{
					m_mainBlock = new MainBlock(_newWidthInCells, _newHeightInCells);
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
						m_gameProvider.Exit();
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
				var mess = (AskSelectThingsMessage)_message;
				m_uiBlocks.Push(new SelectItemsUiBlock(m_mainBlock.Rectangle, mess.ItemDescriptors, mess.Act, mess.Behavior));
			}
			else if (_message is AskSelectThingsFromBackPackMessage)
			{
				var mess = (AskSelectThingsFromBackPackMessage)_message;
				m_uiBlocks.Push(new BackpackUiBlock(m_mainBlock.Rectangle, mess.Behavior, mess.AllowedCategory, mess.Act));
			}
			else if (_message is AskDirectionMessage)
			{
				m_uiBlocks.Push(new AskDirectionUiBlock(m_mainBlock.Rectangle, (AskDirectionMessage) _message));
			}
			else if (_message is AskHowMuchMessage)
			{
				m_uiBlocks.Push(new AskHowMuchUiBlock(m_mainBlock.Messages.ContentRectangle, (AskHowMuchMessage) _message));
			}
			else if (_message is AskShootTargerMessage)
			{
				var askShootTargerMessage = (AskShootTargerMessage)_message;
				m_uiBlocks.Push(new SelectTargetUiBlock(m_mainBlock.Messages, m_mainBlock.Map.Rectangle, askShootTargerMessage.MaxDistance, askShootTargerMessage.Act));
			}
		}

		/// <summary>
		/// 	LoadContent will be called once per game and is the place to load
		/// 	all of your content.
		/// </summary>
		/// <param name="_resourceProvider"> </param>
		public void LoadContent(IResourceProvider _resourceProvider)
		{
			TileHelper.Init(_resourceProvider, m_gameProvider.DrawHelper);
			UIBlock.Init(m_gameProvider.DrawHelper);
			World.LetItBeeee();

			m_mainBlock = new MainBlock(m_gameProvider.Width/ATile.Size, m_gameProvider.Height/ATile.Size);
			m_uiBlocks.Push(m_mainBlock);

			MessageManager.SendMessage(this, " [?] - ����� ������");
		}

		/// <summary>
		/// 	UnloadContent will be called once per game and is the place to unload
		/// 	all content.
		/// </summary>
		public void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		public void Update(KeyState _keyState)
		{
			// TODO: Add your update logic here

			var state = _keyState;

			var keyModifiers = _keyState.KeyModifiers;
			//(state.IsKeyDown(ConsoleKey.LeftShift) || state.IsKeyDown(ConsoleKey.RightShift))
			//                    ? EKeyModifiers.SHIFT
			//                    : EKeyModifiers.NONE;
			//keyModifiers |= (state.IsKeyDown(ConsoleKey.LeftControl) || state.IsKeyDown(ConsoleKey.RightControl))
			//                    ? EKeyModifiers.CTRL
			//                    : EKeyModifiers.NONE;
			//keyModifiers |= (state.IsKeyDown(ConsoleKey.LeftAlt) || state.IsKeyDown(ConsoleKey.RightAlt))
			//                    ? EKeyModifiers.ALT
			//                    : EKeyModifiers.NONE;


			var downKeys = _keyState.PressedKeys;// state.GetPressedKeys().Except(KeyHelper.KeyModificators).ToArray();

			if (keyModifiers != m_keyModifiers) m_downKeys.Clear();

			var pressedKeys = new List<ConsoleKey>();
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

			if (m_downKeys.Except(KeyTranslator.MoveKeys).Any() || pressedKeys.Any())
			{
				m_isAutoRepeateMode = false;
			}
			else
			{
				if (m_downKeys.Intersect(KeyTranslator.MoveKeys).Any())
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
		}

		public void Draw()
		{
			if (!m_gameProvider.IsActive) return;

			foreach (var uiBlock in m_uiBlocks.Reverse())
			{
				uiBlock.DrawBackground();
				uiBlock.DrawContent();
				uiBlock.DrawFrame();
			}

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

			//var format = string.Format("���:{0} XY:{1}", m_fps, World.TheWorld.Avatar.Coords);
			//WriteString(format, 2, GraphicsDevice.Viewport.Height - 18, Color.White, Color.Gray, EFonts.COMMON);
		}
	}
}