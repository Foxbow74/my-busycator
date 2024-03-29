using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Mapping.Layers;
using GameCore.Messages;
using GameCore.Misc;
using GameUi.UIBlocks;
using GameUi.UIBlocks.Help;
using GameUi.UIBlocks.Items;

namespace GameUi
{
	public class TheGame
	{
		private readonly List<ConsoleKey> m_downKeys = new List<ConsoleKey>();
		private readonly IGameProvider m_gameProvider;

		private readonly Queue<Tuple<ConsoleKey, EKeyModifiers>> m_pressed = new Queue<Tuple<ConsoleKey, EKeyModifiers>>();
		private readonly Stack<UIBlock> m_uiBlocks = new Stack<UIBlock>();

		private bool m_isAutoRepeateMode;
		private EKeyModifiers m_keyModifiers = EKeyModifiers.NONE;
		private Point m_lastMousePos = Point.Zero;

		private MainUiBlock m_mainUiBlock;
		private DateTime m_moveKeyHoldedSince;

		public TheGame(IGameProvider _gameProvider)
		{
			m_gameProvider = _gameProvider;
			MessageManager.NewMessage += MessageManagerNewMessage;
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

	    public Stack<UIBlock> UiBlocks
	    {
	        get { return m_uiBlocks; }
	    }

	    public void WindowClientSizeChanged(int _newWidthInCells, int _newHeightInCells)
		{
			var newRct = new Rct(0, 0, _newWidthInCells, _newHeightInCells);
			if (UiBlocks.Any(_block => _block.Rct != newRct))
			{
				foreach (var uiBlock in UiBlocks)
				{
					uiBlock.Resize(newRct);
				}
			}
			if (World.TheWorld != null)
			{
				World.TheWorld.GameUpdated();
			}
		}

		private static void MessageManagerNewWorldMessage(object _sender, WorldMessage _message) { }

		private void MessageManagerNewMessage(object _sender, Message _message)
		{
			if (_message is AskMessageNg)
			{
				var amng = (AskMessageNg) _message;
				switch (amng.AskMessageType)
				{
					case EAskMessageType.LOOK_AT:
						MessageManager.SendMessage(this, new OpenUIBlockMessage(new LookAtUiBlock(m_mainUiBlock.Messages, m_mainUiBlock.Map.Rct)));
						break;
					case EAskMessageType.ASK_DIRECTION:
						MessageManager.SendMessage(this, new OpenUIBlockMessage(new AskDirectionUiBlock(m_mainUiBlock.Map.Rct, amng)));
						break;
					case EAskMessageType.HELP:
						MessageManager.SendMessage(this, new OpenUIBlockMessage(new HelpUiBlock(m_mainUiBlock.Rct)));
						break;
					case EAskMessageType.HOW_MUCH:
						MessageManager.SendMessage(this, new OpenUIBlockMessage(new AskHowMuchUiBlock(m_mainUiBlock.Messages.ContentRct, amng)));
						break;
					case EAskMessageType.ASK_SHOOT_TARGET:
						MessageManager.SendMessage(this, new OpenUIBlockMessage(new SelectTargetUiBlock(m_mainUiBlock.Messages, m_mainUiBlock.Map.Rct, amng)));
						break;
					case EAskMessageType.ASK_DESTINATION:
						MessageManager.SendMessage(this, new OpenUIBlockMessage(new SelectDestinationUiBlock(m_mainUiBlock.Messages, m_mainUiBlock.Map.Rct, amng.Act)));
						break;
					case EAskMessageType.SELECT_THINGS:
						MessageManager.SendMessage(this, new OpenUIBlockMessage(new SelectItemsUiBlock(m_mainUiBlock.Rct, amng)));
						break;
					case EAskMessageType.SELECT_THINGS_FROM_BACK_PACK:
						MessageManager.SendMessage(this, new OpenUIBlockMessage(new BackpackUiBlock(m_mainUiBlock.Rct, amng)));
						break;
					case EAskMessageType.INVENTORY:
						MessageManager.SendMessage(this, new OpenUIBlockMessage(new EquipmentUiBlock(m_mainUiBlock.Rct)));
						break;
					case EAskMessageType.WORLD_MAP:
						MessageManager.SendMessage(this, new OpenUIBlockMessage(new MiniMapUiBlock(m_mainUiBlock.Map.Rct)));
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else if (_message is OpenUIBlockMessage)
			{
				var uiBlock = ((OpenUIBlockMessage) _message).UIBlock;
				UiBlocks.Push(uiBlock);
				MouseMove(m_lastMousePos);
			}
			else if (_message is SystemMessage)
			{
				switch (((SystemMessage) _message).Message)
				{
					case SystemMessage.ESystemMessage.EXIT_GAME:
						m_gameProvider.Exit();
						break;
					case SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK:
						UiBlocks.Pop().Dispose();
						m_pressed.Clear();
						m_downKeys.Clear();
						MessageManager.SendMessage(this, WorldMessage.Turn);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public void LoadContent(IResourceProvider _resourceProvider)
		{
			TileHelper.Init(_resourceProvider, m_gameProvider.DrawHelper);
			UIBlock.Init(m_gameProvider.DrawHelper);
		}

        

        public void Run()
		{
			UiBlocks.Clear();
			World.LetItBeeee(m_gameProvider.GetLanguageProcessors().First());
			m_mainUiBlock = new MainUiBlock(m_gameProvider.Width / Constants.TILE_SIZE, m_gameProvider.Height / Constants.TILE_SIZE);
			UiBlocks.Push(m_mainUiBlock);

			World.TheWorld.UpdateDPoint();

			MessageManager.SendMessage(this, WorldMessage.AvatarMove);
			MessageManager.SendMessage(this, "[?] - ����� ������");
		}

		public void UnloadContent() { }

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
						if (totalMilliseconds > Constants.AUTO_MOVE_REPEAT_MILLISECONDS)
						{
							m_moveKeyHoldedSince = DateTime.Now;
							pressedKeys.AddRange(m_downKeys);
						}
					}
					else
					{
						if (totalMilliseconds > Constants.AUTO_MOVE_REPEAT_AFTER)
						{
							m_isAutoRepeateMode = true;
						}
					}
				}
			}

			foreach (var pressedKey in pressedKeys)
			{
				m_pressed.Enqueue(new Tuple<ConsoleKey, EKeyModifiers>(pressedKey, m_keyModifiers));
			}

			if (m_pressed.Count > 0)
			{
				if (UiBlocks.Peek() != m_mainUiBlock || World.TheWorld.Avatar.NextAct == null)
				{
					var tuple = m_pressed.Dequeue();
					UiBlocks.Peek().KeysPressed(tuple.Item1, tuple.Item2);
					MessageManager.SendMessage(this,WorldMessage.JustRedraw);
				}
			}

			if (UiBlocks.Peek() == m_mainUiBlock)
			{
				if (!m_mainUiBlock.NeedWait)
				{
					if (!World.TheWorld.CreatureManager.FirstActiveCreature.IsAvatar || World.TheWorld.Avatar.NextAct != null  || World.TheWorld.WorldTick==0)
					{
#if DEBUG
						using (new Profiler())
#endif
						{
							World.TheWorld.GameUpdated();
						}
					}
				}
			}
		}

		public void Draw()
		{
			foreach (var uiBlock in UiBlocks.Reverse())
			{
				uiBlock.DrawBackground();
				uiBlock.DrawContent();
				uiBlock.DrawFrame();
			}
		}

		public void MouseMove(Point _pnt)
		{
			if (m_lastMousePos == _pnt) return;

			m_lastMousePos = _pnt;
			var uiBlock = UiBlocks.Peek();

			if (!uiBlock.Rct.Contains(_pnt)) return;
			var pnt = _pnt - uiBlock.Rct.LeftTop;
			uiBlock.MouseMove(pnt);
		}

		public void MouseButtonDown(Point _pnt, EMouseButton _button)
		{
			var uiBlock = UiBlocks.Peek();
			if (uiBlock.Rct.Contains(_pnt))
			{
				var pnt = _pnt - uiBlock.Rct.LeftTop;
				uiBlock.MouseButtonDown(pnt, _button);
			}
		}

		public void MouseButtonUp(Point _pnt, EMouseButton _button)
		{
			var uiBlock = UiBlocks.Peek();
			if (uiBlock.Rct.Contains(_pnt))
			{
				var pnt = _pnt - uiBlock.Rct.LeftTop;
				uiBlock.MouseButtonUp(pnt, _button);
			}
		}
	}
}