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
		protected UIBlock(Rectangle _rectangle, Frame _frame, Color _color, EFonts _font = EFonts.COMMON)
		{
			Font = _font;
			BlockFrame = _frame;
			Color = _color;
			Rectangle = _rectangle;
			BackgroundColor = Color.Black;
			UpdateContentRectangle();
			LineHeight = DrawHelper.MeasureString(_font, "Ay").Height;
		}

		protected float LineHeight { get; private set; }

		public Color BackgroundColor { get; set; }

		public Rectangle Rectangle { get; protected set; }

		public Rectangle ContentRectangle { get; protected set; }

		protected internal Color Color { get; private set; }

		protected Frame BlockFrame { get; private set; }

		public EFonts Font { get; private set; }

		public int TextLinesMax
		{
			get { return (int) Math.Round((double) ContentRectangle.Height*ATile.Size/LineHeight); }
		}

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

		protected static string JoinCommandCaptions(IEnumerable<string> _s)
		{
			return string.Join("   -   ", _s);
		}

		protected void CloseTopBlock(ConsoleKey _consoleKey = ConsoleKey.Escape)
		{
			OnClosing(_consoleKey);
			MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK));
		}

		public virtual void DrawBackground()
		{
			TileHelper.DrawHelper.Clear(
				new Rectangle(Rectangle.Left*ATile.Size, Rectangle.Top*ATile.Size, Rectangle.Width*ATile.Size,
				              Rectangle.Height*ATile.Size), BackgroundColor);
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


		public float DrawLine(string _text, Color _color, int _lineNumber, float _indent, EAlignment _alignment)
		{
			return DrawLine(new TextPortion.TextLine(_text, 0, null), _color, _lineNumber, _indent, _alignment);
		}

		protected void DrawLine(TextPortion.TextLine _textLine, Color _color, float _x, float _y)
		{
			var line = _textLine.Text;
			var part = line.Split(TextPortion.Punctuation).ToArray();
			var processedChars = 0;
			var x = _x + _textLine.Left;
			for (var partIndex = 0; partIndex < part.Length; partIndex++)
			{
				var color = _color;
				var addStr = part[partIndex];
				Color highlight;
				if (_textLine.Highlights != null && _textLine.Highlights.TryGetValue(addStr, out highlight))
				{
					color = highlight;
				}
				processedChars += addStr.Length;
				addStr += (processedChars == 0 || processedChars >= line.Length) ? "" : line[processedChars].ToString();
				processedChars++;
				var size = DrawHelper.MeasureString(Font, addStr);
				DrawHelper.DrawString(Font, addStr, x, _y, color);
				x += size.Width;
			}
		}

		protected float DrawLine(TextPortion.TextLine _textLine, Color _color, int _lineNumber, float _indent,
		                         EAlignment _alignment)
		{
			var line = _textLine.Text;
			var part = line.Split(TextPortion.Punctuation).ToArray();
			var processedChars = 0;
			var x = (float) ContentRectangle.Left*ATile.Size;

			var lineSize = DrawHelper.MeasureString(Font, line);
			switch (_alignment)
			{
				case EAlignment.NORMAL:
					x += _indent + _textLine.Left;
					break;
				case EAlignment.LEFT:
					x += _indent;
					break;
				case EAlignment.RIGHT:
					x += ContentRectangle.Width*ATile.Size - lineSize.Width - _indent;
					break;
				case EAlignment.CENTER:
					x += ContentRectangle.Width*ATile.Size/2f - lineSize.Width/2f;
					break;
			}

			var y = ContentRectangle.Top*ATile.Size + _lineNumber*LineHeight;

			for (var partIndex = 0; partIndex < part.Length; partIndex++)
			{
				var color = _color;
				var addStr = part[partIndex];
				Color highlight;
				if (_textLine.Highlights != null && _textLine.Highlights.TryGetValue(addStr, out highlight))
				{
					color = highlight;
					processedChars += addStr.Length;
				}
				else
				{
					processedChars += addStr.Length;
					addStr += (processedChars >= line.Length) ? "" : line[processedChars].ToString();
					processedChars++;
				}
				var size = DrawHelper.MeasureString(Font, addStr);
				DrawHelper.DrawString(Font, addStr, x, y, color);
				x += size.Width + 2;
			}
			return x;
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