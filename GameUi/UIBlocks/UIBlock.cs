using System;
using System.Drawing;
using GameCore;
using GameCore.Messages;

namespace GameUi.UIBlocks
{
	internal class UIBlock
	{
		protected UIBlock(Rectangle _rectangle, Frame _frame, FColor _color)
		{
			BlockFrame = _frame;
			ForeColor = _color;
			Rectangle = _rectangle;
			BackgroundColor = new FColor(1,0,0,0);
			UpdateContentRectangle();
		}

		public FColor BackgroundColor { get; set; }

		public Rectangle Rectangle { get; protected set; }

		public Rectangle ContentRectangle { get; protected set; }

		protected internal FColor ForeColor { get; private set; }

		protected Frame BlockFrame { get; private set; }

		public static IDrawHelper DrawHelper { get; private set; }

		public static void Init(IDrawHelper _drawHelper)
		{
			DrawHelper = _drawHelper;
		}

		protected void UpdateContentRectangle()
		{
			if (BlockFrame != null)
			{
				ContentRectangle = new Rectangle(Rectangle.Left + 1, Rectangle.Top + 1, Rectangle.Width - 2, Rectangle.Height - 2);
			}
			else
			{
				ContentRectangle = Rectangle;
			}
		}

		protected virtual void OnClosing(ConsoleKey _consoleKey)
		{
		}

		public virtual void DrawFrame()
		{
			if (BlockFrame != null)
			{
				Draw(BlockFrame, Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
			}
		}

		public void Draw(Frame _frame, int _col, int _row, int _width, int _height)
		{
			_frame.TopLeft.DrawAtCell(_col, _row, BackgroundColor);
			_frame.TopRight.DrawAtCell(_col + _width - 1, _row, BackgroundColor);
			_frame.BottomLeft.DrawAtCell(_col, _row + _height - 1, BackgroundColor);
			_frame.BottmoRight.DrawAtCell(_col + _width - 1, _row + _height - 1, BackgroundColor);

			for (var i = 1; i < _width - 1; i++)
			{
				_frame.Top.DrawAtCell(_col + i, _row, BackgroundColor);
				_frame.Bottom.DrawAtCell(_col + i, _row + _height - 1, BackgroundColor);
			}
			for (var j = 1; j < _height - 1; j++)
			{
				_frame.Left.DrawAtCell(_col, _row + j, BackgroundColor);
				_frame.Right.DrawAtCell(_col + _width - 1, _row + j, BackgroundColor);
			}
		}

		protected void CloseTopBlock(ConsoleKey _consoleKey = ConsoleKey.Escape)
		{
			OnClosing(_consoleKey);
			MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK));
		}

		public virtual void DrawBackground()
		{
			TileHelper.DrawHelper.ClearTiles(Rectangle, BackgroundColor);
		}

		public void Dispose()
		{
			//throw new NotImplementedException();
		}

		public virtual void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
		}

		public virtual void DrawContent()
		{
		}

	}

	internal class OpenUIBlockMessage : Message
	{
		public OpenUIBlockMessage(UIBlock _block)
		{
			UIBlock = _block;
		}

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

		public SystemMessage(ESystemMessage _message)
		{
			Message = _message;
		}

		public ESystemMessage Message { get; private set; }
	}
}