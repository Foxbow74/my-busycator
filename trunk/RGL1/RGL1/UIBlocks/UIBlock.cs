using System;
using System.Linq;
using Common.Messages;
using GameCore;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks
{
	abstract class UIBlock:IDisposable
	{
		protected readonly SpriteFont m_font;
		protected readonly float m_lineHeight;

		const float NEW_LINE = 20;

		protected UIBlock(Rectangle _rectangle, Frame _frame, Color _color, SpriteFont _font = null)
		{
			m_font = _font ?? Tile.Font;

			m_lineHeight = m_font.MeasureString("!g").Y;
			BlockFrame = _frame;
			Color = _color;
			Rectangle = _rectangle;
			ContentRectangle = _rectangle;
			if(_frame!=null) ContentRectangle = new Rectangle(_rectangle.Left + 1,_rectangle.Top + 1, _rectangle.Width - 2, _rectangle.Height - 2);
		}

		public Rectangle Rectangle { get; private set; }

		public Rectangle ContentRectangle { get; protected set; }

		protected internal Color Color { get; private set; }

		protected Frame BlockFrame { get; private set; }

		public virtual void DrawBackground(SpriteBatch _spriteBatch)
		{
			this.Clear(_spriteBatch, Color.Black);
		}

		public virtual void DrawFrame(SpriteBatch _spriteBatch)
		{
			if (BlockFrame != null) BlockFrame.Draw(_spriteBatch, Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
		}

		protected void DrawLine(TextPortion.TextLine _textLine, Color _color, SpriteBatch _spriteBatch, float _x, float _y)
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
				var size = m_font.MeasureString(addStr);
				_spriteBatch.DrawString(m_font, addStr, new Vector2(x, _y), color);
				x += size.X;
			}
		}


		internal enum EAlignment
		{
			NORMAL,
			LEFT,
			RIGHT,
			CENTER,
			JUSTIFY
		}

		protected void DrawLine(string _text, Color _color, SpriteBatch _spriteBatch, int _lineNumber, int _indent, EAlignment _alignment)
		{
			DrawLine(new TextPortion.TextLine(_text, 0, null), _color, _spriteBatch, _lineNumber, _indent, _alignment);
		}

		protected void DrawLine(TextPortion.TextLine _textLine, Color _color, SpriteBatch _spriteBatch, int _lineNumber, int _indent, EAlignment _alignment)
		{
			var line = _textLine.Text;
			var part = line.Split(TextPortion.Punctuation).ToArray();
			var processedChars = 0;
			var x = (float)ContentRectangle.Left * Tile.Size;

			var lineSize = m_font.MeasureString(line);
			switch (_alignment)
			{
				case EAlignment.NORMAL:
					x += _indent + _textLine.Left;
					break;
				case EAlignment.LEFT:
					x += (float)_indent;
					break;
				case EAlignment.RIGHT:
					x += ContentRectangle.Width * Tile.Size - lineSize.X - _indent;
					break;
				case EAlignment.CENTER:
					x += ContentRectangle.Left*Tile.Size + ContentRectangle.Width*Tile.Size/2 - lineSize.X/2;
					break;
			}

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
				var size = m_font.MeasureString(addStr);
				_spriteBatch.DrawString(m_font, addStr, new Vector2(x, ContentRectangle.Top * Tile.Size + _lineNumber*m_lineHeight), color);
				x += size.X;
			}
		}


		public virtual void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			
		}

		public virtual void Dispose()
		{
		}

		public abstract void DrawContent(SpriteBatch _spriteBatch);
	}

	class OpenUIBlockMessage : Message
	{
		public UIBlock UIBlock { get; private set; }

		public OpenUIBlockMessage(UIBlock _block)
		{
			UIBlock = _block;
		}
	}

	class SystemMessage : Message
	{
		public ESystemMessage Message { get; private set; }

		public enum ESystemMessage
		{
			EXIT_GAME,
			CLOSE_TOP_UI_BLOCK,
		}

		public SystemMessage(ESystemMessage _message)
		{
			Message = _message;
		}
	}
}