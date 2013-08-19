using System;

namespace GameCore.AbstractLanguage
{
	public abstract class AbstractWord
	{
		private readonly string m_text;

		protected AbstractWord()
		{ }

		protected AbstractWord(string _text)
		{
			m_text = _text;
		}

		public string Text { get { return m_text; } }
	}
}
