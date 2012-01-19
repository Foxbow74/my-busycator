using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Graphics
{

	/// <summary>
	/// Используется для вывода текста
	/// </summary>
	public class TextPortion
	{
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
	}
}