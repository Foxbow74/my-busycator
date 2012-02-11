using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Messages;

namespace GameUi.UIBlocks
{
	internal class UIBlock
	{
		protected UIBlock(Rectangle _rectangle, Frame _frame, Color _color)
		{
			BlockFrame = _frame;
			Color = _color;
			Rectangle = _rectangle;
			BackgroundColor = Color.Black;
			UpdateContentRectangle();
		}

		public Color BackgroundColor { get; set; }

		public Rectangle Rectangle { get; protected set; }

		public Rectangle ContentRectangle { get; protected set; }

		protected internal Color Color { get; private set; }

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
				TileHelper.Draw(BlockFrame, Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
			}
		}

		protected void CloseTopBlock(ConsoleKey _consoleKey = ConsoleKey.Escape)
		{
			OnClosing(_consoleKey);
			MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK));
		}

		protected virtual bool ClearTextWhenClear{get { return false; }}

		public virtual void DrawBackground()
		{
			TileHelper.DrawHelper.Clear(new Rectangle(Rectangle.Left*ATile.Size, Rectangle.Top*ATile.Size, Rectangle.Width*ATile.Size,Rectangle.Height*ATile.Size), BackgroundColor, ClearTextWhenClear);
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