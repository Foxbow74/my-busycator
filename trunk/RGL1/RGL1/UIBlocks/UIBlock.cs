using System;
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
			Rectangle = _rectangle;
			BlockFrame = _frame;
			Color = _color;
		}

		public Rectangle Rectangle { get; private set; }

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
			
			float width = (Rectangle.Width - 2) * Tile.Size;
			_textPortion.SplitByLines(width, _font, NEW_LINE);


			float left = (Rectangle.Left + 1) * Tile.Size;

			float top = (Rectangle.Top + 1) * Tile.Size;
			float height = (Rectangle.Height - 2) * Tile.Size - _y;

			foreach (var textLine in _textPortion.TextLines)
			{
				var x = textLine.Left;
				var line = textLine.Text;
				var part = line.Split(new[] { ' ', ',', '.', ':', ';' });
				var processedChars = 0;
				for (var partIndex = 0; partIndex < part.Length; partIndex++)
				{
					var color = _color;
					var addStr = part[partIndex];
					Color highlight;
					if (_textPortion.Highlights != null && _textPortion.Highlights.TryGetValue(addStr, out highlight))
					{
						color = highlight;
					}
					processedChars += addStr.Length;
					addStr += (processedChars == 0 || processedChars >= line.Length) ? "" : line[processedChars].ToString();
					processedChars++;
					var size = _font.MeasureString(addStr);
					_spriteBatch.DrawString(_font, addStr, new Vector2(left + x, top + _y), color);
					x += size.X;
				}
				_y += lineHeight + 2;
				if (_y >= height)
				{
					return;
				}
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