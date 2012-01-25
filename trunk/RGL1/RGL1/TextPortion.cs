using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1
{

	/// <summary>
	/// Используется для вывода текста
	/// </summary>
	public class TextPortion
	{
		private TextLine[] m_textLines;

		public static readonly char[] Punctuation = new[] {' ', ',', '.', ':', ';', '!', '?', '[', ']'};

		public class TextLine
		{
			public Dictionary<string, Color> Highlights { get; private set; }

			public float Left { get; private set; }

			public string Text { get; set; }

			public TextLine(float _left, Dictionary<string, Color> _highlights)
			{
				Left = _left;
				Highlights = _highlights;
			}

			public TextLine(string _text, float _left, Dictionary<string, Color> _highlights)
			{
				Text = _text;
				Left = _left;
				Highlights = _highlights;
			}
		}

		public string Text { get; private set; }
		public Dictionary<string, Color> Highlights { get; private set; }

		/// <summary>
		/// Указывает с какой строки продолжать вывод порции текста если весь текст не влез в окно
		/// </summary>
		public int FromLine { get; private set; }

		/// <summary>
		/// Указывает с какого слова продолжать вывод порции текста если весь текст не влез в окно
		/// </summary>
		public int FromPart { get; private set; }

		public TextPortion(string _text):this(_text, null)
		{
		}

		public TextPortion(string _text, Dictionary<string, Color> _highlights)
		{
			Text = _text;
			Highlights = _highlights;
			Reset();
		}

		public void Update(int _newFromLine, int _newFromPart)
		{
			FromLine = _newFromLine;
			FromPart = _newFromPart;
		}

		public void Reset()
		{
			FromLine = 0;
			FromPart = 0;
		}

		public IEnumerable<TextLine> TextLines { get { return m_textLines; } }

		public void SplitByLines(float _width, SpriteFont _font, float _newLineIndent)
		{
			var textLines = new List<TextLine>();

			var paragraphs = Text.Split(new[] { Environment.NewLine, "\t" }, StringSplitOptions.RemoveEmptyEntries);

			var sb = new StringBuilder();
			foreach (var paragraph in paragraphs)
			{
				sb.Clear();
				var x = _newLineIndent;
				
				var tl = new TextLine(x, Highlights);
				textLines.Add(tl);

				var part = paragraph.Split(Punctuation);
				var processedChars = 0;
				for (var partIndex = 0; partIndex < part.Length; partIndex++)
				{
					var addStr = part[partIndex];
					processedChars += addStr.Length;
					addStr += (processedChars == 0 || processedChars >= paragraph.Length) ? "" : paragraph[processedChars].ToString(CultureInfo.InvariantCulture);
					processedChars++;
					var size = _font.MeasureString(addStr);

					if (size.X > (_width - x))
					{
						tl.Text = sb.ToString();
						sb.Clear();
						x = 0;
						tl = new TextLine(x, Highlights);
						textLines.Add(tl);
					}
					sb.Append(addStr);
					x += size.X;
				}

				if(sb.Length>0)
				{
					tl.Text = sb.ToString();
				}
			}
			m_textLines = textLines.ToArray();
		}
	}
}