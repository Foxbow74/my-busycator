using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameUi.UIBlocks
{
	abstract class UiBlockWithText:UIBlock
	{
		protected UiBlockWithText(Rectangle _rectangle, Frame _frame, Color _color, EFonts _font=EFonts.COMMON) : base(_rectangle, _frame, _color)
		{
			Font = _font;
			LineHeight = DrawHelper.MeasureString(_font, "Ay").Height;
		}

		protected float LineHeight { get; private set; }

		public EFonts Font { get; private set; }

		public int TextLinesMax
		{
			get { return (int)Math.Round((double)ContentRectangle.Height * ATile.Size / LineHeight); }
		}

		public override void DrawBackground()
		{
			base.DrawBackground();
			TileHelper.DrawHelper.ClearText(new Rectangle(Rectangle.Left * ATile.Size, Rectangle.Top * ATile.Size, Rectangle.Width * ATile.Size, Rectangle.Height * ATile.Size), BackgroundColor);
		}

		protected override void OnClosing(ConsoleKey _consoleKey)
		{
			TileHelper.DrawHelper.ClearText(new Rectangle(Rectangle.Left * ATile.Size, Rectangle.Top * ATile.Size, Rectangle.Width * ATile.Size, Rectangle.Height * ATile.Size), BackgroundColor);
			base.OnClosing(_consoleKey);
		}

		protected static string JoinCommandCaptions(IEnumerable<string> _s)
		{
			return string.Join("   -   ", _s);
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
			var x = (float)ContentRectangle.Left * ATile.Size;

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
					x += ContentRectangle.Width * ATile.Size - lineSize.Width - _indent;
					break;
				case EAlignment.CENTER:
					x += ContentRectangle.Width * ATile.Size / 2f - lineSize.Width / 2f;
					break;
			}

			var y = ContentRectangle.Top * ATile.Size + _lineNumber * LineHeight;

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
}
