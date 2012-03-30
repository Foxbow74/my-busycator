using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	public abstract class UiBlockWithText : UIBlock
	{
		protected UiBlockWithText(Rct _rct, Frame _frame, FColor _color, EFonts _font = EFonts.COMMON) : base(_rct, _frame, _color)
		{
			Font = _font;
			LineHeight = DrawHelper.MeasureString(_font, "Ay").Height;
		}

		protected float LineHeight { get; private set; }

		public EFonts Font { get; private set; }

		public int TextLinesMax { get { return (int) Math.Round((double) ContentRct.Height*ATile.Size/LineHeight); } }

		protected static string JoinCommandCaptions(IEnumerable<string> _s) { return string.Join("   -   ", _s); }

		public float DrawLine(string _text, FColor _color, int _lineNumber, float _indent, EAlignment _alignment) { return DrawLine(new TextPortion.TextLine(_text, 0, null), _color, _lineNumber, _indent, _alignment); }

		protected void DrawLine(TextPortion.TextLine _textLine, FColor _color, float _x, float _y)
		{
			var line = _textLine.Text;
			var part = line.Split(TextPortion.Punctuation).ToArray();
			var processedChars = 0;
			var x = _x + _textLine.Left;
			for (var partIndex = 0; partIndex < part.Length; partIndex++)
			{
				var color = _color;
				var addStr = part[partIndex];
				FColor highlight;
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

		protected float DrawLine(TextPortion.TextLine _textLine,
		                         FColor _color,
		                         int _lineNumber,
		                         float _indent,
		                         EAlignment _alignment)
		{
			var line = _textLine.Text;
			var part = line.Split(TextPortion.Punctuation).ToArray();
			var processedChars = 0;
			var x = (float) ContentRct.Left*ATile.Size;

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
					x += ContentRct.Width*ATile.Size - lineSize.Width - _indent;
					break;
				case EAlignment.CENTER:
					x += ContentRct.Width*ATile.Size/2f - lineSize.Width/2f;
					break;
			}

			var y = ContentRct.Top*ATile.Size + _lineNumber*LineHeight;

			for (var partIndex = 0; partIndex < part.Length; partIndex++)
			{
				var color = _color;
				var addStr = part[partIndex];
				FColor highlight;
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