using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using GameUi.UIBlocks;
using GameUi.UIBlocks.Items;

namespace GameUi
{
	public class TheGame
	{
		private const int AUTO_MOVE_REPEAT_MILLISECONDS = 100;
		private const int AUTO_MOVE_REPEAT_AFTER = 200;
		private readonly List<ConsoleKey> m_downKeys = new List<ConsoleKey>();
		private readonly IGameProvider m_gameProvider;

		private readonly Queue<Tuple<ConsoleKey, EKeyModifiers>> m_pressed = new Queue<Tuple<ConsoleKey, EKeyModifiers>>();
		private readonly Stack<UIBlock> m_uiBlocks = new Stack<UIBlock>();

		private bool m_isAutoRepeateMode;
		private EKeyModifiers m_keyModifiers = EKeyModifiers.NONE;

		private MainUiBlock m_mainUiBlock;
		private DateTime m_moveKeyHoldedSince;
		private int m_needRedraws = 2;

		public TheGame(IGameProvider _gameProvider)
		{
			m_gameProvider = _gameProvider;
			MessageManager.NewMessage += MessageManagerNewMessage;
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		public bool IsNeedDraw
		{
			get
			{
				var result = m_needRedraws >0;
				m_needRedraws--;
				return result;
			}
		}

		public void WindowClientSizeChanged(int _newWidthInCells, int _newHeightInCells)
		{
			var newRct = new Rct(0,0,_newWidthInCells,_newHeightInCells);
			if(m_uiBlocks.Any(_block => _block.Rct!=newRct))
			{
				foreach (var uiBlock in m_uiBlocks)
				{
					uiBlock.Resize(newRct);
				}
			}
			World.TheWorld.GameUpdated(true);
		}

		private static void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
		}

		private void MessageManagerNewMessage(object _sender, Message _message)
		{
			if (_message is OpenUIBlockMessage)
			{
				m_uiBlocks.Push(((OpenUIBlockMessage) _message).UIBlock);
				m_needRedraws = 4;
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
						MessageManager.SendMessage(this, WorldMessage.Turn);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else if (_message is AskSelectThingsMessage)
			{
				var mess = (AskSelectThingsMessage) _message;
				MessageManager.SendMessage(this,
				                           new OpenUIBlockMessage(new SelectItemsUiBlock(m_mainUiBlock.Rct, mess.ItemDescriptors,
				                                                                         mess.Act, mess.Behavior)));
			}
			else if (_message is AskSelectThingsFromBackPackMessage)
			{
				var mess = (AskSelectThingsFromBackPackMessage) _message;
				MessageManager.SendMessage(this,
				                           new OpenUIBlockMessage(new BackpackUiBlock(m_mainUiBlock.Rct, mess.Behavior,
				                                                                      mess.AllowedCategory, mess.Act)));
			}
			else if (_message is AskDirectionMessage)
			{
				MessageManager.SendMessage(this,
				                           new OpenUIBlockMessage(new AskDirectionUiBlock(m_mainUiBlock.Rct,
				                                                                          (AskDirectionMessage) _message)));
			}
			else if (_message is AskHowMuchMessage)
			{
				MessageManager.SendMessage(this,
				                           new OpenUIBlockMessage(new AskHowMuchUiBlock(m_mainUiBlock.Messages.ContentRct,
				                                                                        (AskHowMuchMessage) _message)));
			}
			else if (_message is AskShootTargerMessage)
			{
				var askShootTargerMessage = (AskShootTargerMessage) _message;
				MessageManager.SendMessage(this,
				                           new OpenUIBlockMessage(new SelectTargetUiBlock(m_mainUiBlock.Messages,
				                                                                          m_mainUiBlock.Map.Rct,
				                                                                          askShootTargerMessage.MaxDistance,
				                                                                          askShootTargerMessage.Act)));
			}
		}

		public void LoadContent(IResourceProvider _resourceProvider)
		{
			TileHelper.Init(_resourceProvider, m_gameProvider.DrawHelper);
			UIBlock.Init(m_gameProvider.DrawHelper);
			World.LetItBeeee();

			m_mainUiBlock = new MainUiBlock(m_gameProvider.Width/ATile.Size, m_gameProvider.Height/ATile.Size);
			m_uiBlocks.Push(m_mainUiBlock);

			MessageManager.SendMessage(this, WorldMessage.AvatarMove);
			MessageManager.SendMessage(this, " [?] - экран помощи");
		}

		public void UnloadContent()
		{
		}

		public void Update(KeyState _keyState)
		{
			var keyModifiers = _keyState.KeyModifiers;

			var downKeys = _keyState.PressedKeys;

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
				m_pressed.Enqueue(new Tuple<ConsoleKey, EKeyModifiers>(pressedKey, m_keyModifiers));
				m_needRedraws = 2;
			}

			if (m_pressed.Count > 0)
			{
				if (m_uiBlocks.Peek() != m_mainUiBlock || World.TheWorld.Avatar.NextAct == null)
				{
					MessageManager.SendMessage(this, WorldMessage.AvatarTurn);

					var tuple = m_pressed.Dequeue();
					m_uiBlocks.Peek().KeysPressed(tuple.Item1, tuple.Item2);
				}
			}

			using (new Profiler("World.TheWorld.GameUpdated()"))
			{
				if (m_uiBlocks.Peek() == m_mainUiBlock)
				{
					m_needRedraws = World.TheWorld.GameUpdated() ? 4 : m_needRedraws;
				}
			}
		}

		public void Draw()
		{
			foreach (var uiBlock in m_uiBlocks.Reverse())
			{
				uiBlock.DrawBackground();
				uiBlock.DrawContent();
				uiBlock.DrawFrame();
			}
		}
	}
}