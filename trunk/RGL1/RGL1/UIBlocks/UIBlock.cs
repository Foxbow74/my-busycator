using System;
using System.Linq;
using GameCore;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.Messages;

namespace RGL1.UIBlocks
{
	class UIBlock:IDisposable
	{
		const float NEW_LINE = 20;

		public UIBlock(Rectangle _rectangle, Frame _frame, Color _color)
		{
			BlockFrame = _frame;
			Color = _color;
			Rectangle = _rectangle;
			ContentRectangle = _rectangle;
			if(_frame!=null) ContentRectangle.Inflate(1,1);
		}

		public Rectangle Rectangle { get; private set; }

		public Rectangle ContentRectangle { get; private set; }

		protected internal Color Color { get; private set; }

		protected Frame BlockFrame { get; private set; }

		public virtual void Draw(GameTime _gameTime, SpriteBatch _spriteBatch)
		{
			DrawFrame(_spriteBatch);
		}

		public virtual void DrawBackground(SpriteBatch _spriteBatch)
		{
			this.Clear(_spriteBatch, Color.Black);
		}

		public void DrawFrame(SpriteBatch _spriteBatch)
		{
			if (BlockFrame != null) BlockFrame.Draw(_spriteBatch, Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
		}

		public void DrawText(TextPortion _textPortion, SpriteBatch _spriteBatch, SpriteFont _font, Color _color, float _y)
		{
			var lineHeight = _font.MeasureString("!g").Y;

			float width = (ContentRectangle.Width - 1) * Tile.Size;
			_textPortion.SplitByLines(width, _font, NEW_LINE);


			float left = ContentRectangle.Left * Tile.Size;

			var top = ContentRectangle.Top  * Tile.Size;
			var height = (ContentRectangle.Height - 1) * Tile.Size - _y;

			foreach (var textLine in _textPortion.TextLines)
			{

				DrawLine(textLine, _font, _color, _spriteBatch, left, top + _y);

				_y += lineHeight + 2;
				if (_y >= height)
				{
					return;
				}
			}
		}

		protected static void DrawLine(TextPortion.TextLine _textLine, SpriteFont _font, Color _color, SpriteBatch _spriteBatch, float _x, float _y)
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
				var size = _font.MeasureString(addStr);
				_spriteBatch.DrawString(_font, addStr, new Vector2(x, _y), color);
				x += size.X;
			}
		}

		public virtual void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			
		}

		public virtual void Dispose()
		{
		}

		public virtual void DrawContent(SpriteBatch _spriteBatch)
		{
		}
	}


	class OpenUIBlockMessage : Message
	{
		public UIBlock UIBlock { get; private set; }

		public OpenUIBlockMessage(UIBlock _block)
			: base(EMessageType.SYSTEM, null, null)
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
			: base(EMessageType.SYSTEM, null, null)
		{
			Message = _message;
		}
	}
}