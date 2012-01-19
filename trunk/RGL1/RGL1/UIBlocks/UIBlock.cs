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
			if (BlockFrame != null) BlockFrame.Draw(_spriteBatch, Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
		}

		public void PreDraw(SpriteBatch _spriteBatch)
		{
			if (BlockFrame != null) BlockFrame.Draw(_spriteBatch, Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
		}

		public void DrawText(TextPortion _textPortion, SpriteBatch _spriteBatch, SpriteFont _font, Color _color)
		{
			var lineHeight = _font.MeasureString("!g").Y; 

			float left = (Rectangle.Left + 1) * Tile.Size;
			float width = (Rectangle.Width - 2) * Tile.Size;

			float top = (Rectangle.Top + 1) * Tile.Size;
			float height = (Rectangle.Height - 2) * Tile.Size;


			var lines = _textPortion.Text.Split(new[] { Environment.NewLine, "\t" }, StringSplitOptions.RemoveEmptyEntries);
			float y = 0;

			var fromPart = _textPortion.FromPart;

			for (var lineIndex = _textPortion.FromLine; lineIndex < lines.Length; lineIndex++)
			{
				var line = lines[lineIndex];
				var x = NEW_LINE;
				var part = line.Split(new[] {' ', ',', '.', ':', ';'});
				var processedChars = 0;
				for (var partIndex = fromPart; partIndex < part.Length; partIndex++)
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

					if (size.X > (width - x))
					{
						x = 0;
						y += size.Y;
					}
					_spriteBatch.DrawString(_font, addStr, new Vector2(left + x, top + y), color);
					x += size.X;
					if ((y + lineHeight) >= height)
					{
						_textPortion.Update(lineIndex, partIndex);
						return;
					}
				}
				fromPart = 0;
				y += lineHeight + 2;
			}
			_textPortion.Update(int.MaxValue, int.MaxValue);
		}

		public virtual void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			
		}

		public virtual void Dispose()
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