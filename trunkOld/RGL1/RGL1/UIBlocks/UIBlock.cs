﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks
{
	internal abstract class UIBlock : IDisposable
	{
		protected static float m_lineHeight = 10;
		protected readonly SpriteFont m_font;

		protected UIBlock(Rectangle _rectangle, Frame _frame, Color _color, SpriteFont _font = null)
		{
			m_font = _font ?? Fonts.Font;

			m_lineHeight = m_font.MeasureString("!р").Y;
			BlockFrame = _frame;
			Color = _color;
			Rectangle = _rectangle;
			UpdateContentRectangle();
			BackgroundColor = Color.Black;
		}

		public int TextLinesMax
		{
			get { return (int) Math.Round((double) ContentRectangle.Height*Tile.Size/m_lineHeight); }
		}

		public Color BackgroundColor { get; set; }

		public Rectangle Rectangle { get; protected set; }

		public Rectangle ContentRectangle { get; protected set; }

		protected internal Color Color { get; private set; }

		protected Frame BlockFrame { get; private set; }

		#region IDisposable Members

		public virtual void Dispose()
		{
		}

		#endregion

		protected void CloseTopBlock(ConsoleKey _consoleKey = ConsoleKey.Escape)
		{
			OnClosing(_consoleKey);
			MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK));
		}

		protected virtual void OnClosing(ConsoleKey _consoleKey)
		{
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

		public virtual void DrawBackground(SpriteBatch _spriteBatch)
		{
			this.Clear(_spriteBatch);
		}

		public virtual void DrawFrame(SpriteBatch _spriteBatch)
		{
			if (BlockFrame != null)
			{
				_spriteBatch.Begin();
				BlockFrame.Draw(_spriteBatch, Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
				_spriteBatch.End();
			}
		}

		internal void DrawLine(TextPortion.TextLine _textLine, Color _color, SpriteBatch _spriteBatch, float _x, float _y)
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


		internal float DrawLine(string _text, Color _color, SpriteBatch _spriteBatch, int _lineNumber, float _indent,
		                        EAlignment _alignment)
		{
			return DrawLine(new TextPortion.TextLine(_text, 0, null), _color, _spriteBatch, _lineNumber, _indent, _alignment);
		}

		internal float DrawLine(TextPortion.TextLine _textLine, Color _color, SpriteBatch _spriteBatch, int _lineNumber,
		                        float _indent, EAlignment _alignment)
		{
			var line = _textLine.Text;
			var part = line.Split(TextPortion.Punctuation).ToArray();
			var processedChars = 0;
			var x = (float) ContentRectangle.Left*Tile.Size;

			var lineSize = m_font.MeasureString(line);
			switch (_alignment)
			{
				case EAlignment.NORMAL:
					x += _indent + _textLine.Left;
					break;
				case EAlignment.LEFT:
					x += _indent;
					break;
				case EAlignment.RIGHT:
					x += ContentRectangle.Width*Tile.Size - lineSize.X - _indent;
					break;
				case EAlignment.CENTER:
					x += ContentRectangle.Width*Tile.Size/2f - lineSize.X/2f;
					break;
			}

			var y = ContentRectangle.Top*Tile.Size + _lineNumber*m_lineHeight;

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
				var size = m_font.MeasureString(addStr);
				_spriteBatch.DrawString(m_font, addStr, new Vector2(x, y), color);
				x += size.X + 2;
			}
			return x;
		}

		protected static string JoinCommandCaptions(IEnumerable<string> _s)
		{
			return string.Join("   -   ", _s);
		}

		public abstract void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers);

		public abstract void DrawContent(SpriteBatch _spriteBatch);

		#region Nested type: EAlignment

		internal enum EAlignment
		{
			NORMAL,
			LEFT,
			RIGHT,
			CENTER,
			JUSTIFY
		}

		#endregion
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