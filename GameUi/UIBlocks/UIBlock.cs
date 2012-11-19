using System;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using UnsafeUtils;

namespace GameUi.UIBlocks
{
	public class UIBlock
	{
		protected UIBlock(Rct _rct, Frame _frame, FColor _color)
		{
			BlockFrame = _frame;
			ForeColor = _color;
			Rct = _rct;
			BackgroundColor = new FColor(1, 0, 0, 0);
			UpdateContentRct();
		}

		public FColor BackgroundColor { get; set; }

		public Rct Rct { get; protected set; }

		public Rct ContentRct { get; protected set; }

		protected internal FColor ForeColor { get; private set; }

		protected Frame BlockFrame { get; private set; }

		public static IDrawHelper DrawHelper { get; private set; }

		public virtual void Resize(Rct _newRct)
		{
			Rct = _newRct;
			UpdateContentRct();
		}

		public static void Init(IDrawHelper _drawHelper) { DrawHelper = _drawHelper; }

		protected void UpdateContentRct()
		{
			ContentRct = Rct;
			if (BlockFrame != null)
			{
				ContentRct = ContentRct.Inflate(-1, -1);
			}
		}

		protected virtual void OnClosing(ConsoleKey _consoleKey) { }

		public virtual void DrawFrame()
		{
			if (BlockFrame != null)
			{
				Draw(BlockFrame, Rct.Left, Rct.Top, Rct.Width, Rct.Height);
			}
		}

		public void Draw(Frame _frame, int _col, int _row, int _width, int _height)
		{
			_frame.TopLeft.Draw(new Point(_col, _row));
			_frame.TopRight.Draw(new Point(_col + _width - 1, _row));
			_frame.BottomLeft.Draw(new Point(_col, _row + _height - 1));
			_frame.BottmoRight.Draw(new Point(_col + _width - 1, _row + _height - 1));

			for (var i = 1; i < _width - 1; i++)
			{
				_frame.Top.Draw(new Point(_col + i, _row));
				_frame.Bottom.Draw(new Point(_col + i, _row + _height - 1));
			}
			for (var j = 1; j < _height - 1; j++)
			{
				_frame.Left.Draw(new Point(_col, _row + j));
				_frame.Right.Draw(new Point(_col + _width - 1, _row + j));
			}
		}

		protected void CloseTopBlock(ConsoleKey _consoleKey = ConsoleKey.Escape)
		{
			OnClosing(_consoleKey);
			MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK));
		}

		public virtual void DrawBackground() { TileHelper.DrawHelper.ClearTiles(Rct, BackgroundColor); }

		public virtual void Dispose() { }

		public virtual void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers) { }

		public virtual void DrawContent() { }

		public virtual void MouseMove(Point _pnt) { }

		public virtual void MouseButtonDown(Point _pnt, EMouseButton _button) { }

		public virtual void MouseButtonUp(Point _pnt, EMouseButton _button) { }
	}

	internal class OpenUIBlockMessage : Message
	{
		public OpenUIBlockMessage(UIBlock _block) { UIBlock = _block; }

		public UIBlock UIBlock { get; private set; }
	}

	internal class SystemMessage : Message
	{
		#region ESystemMessage enum

		public enum ESystemMessage
		{
			EXIT_GAME,
			CLOSE_TOP_UI_BLOCK,
		}

		#endregion

		public SystemMessage(ESystemMessage _message) { Message = _message; }

		public ESystemMessage Message { get; private set; }

		public override string ToString()
		{
			return Message.ToString();
		}
	}
}