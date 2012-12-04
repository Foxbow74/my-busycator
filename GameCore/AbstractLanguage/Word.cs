using System.Collections.Generic;

namespace GameCore.AbstractLanguage
{
	public enum EWord
	{
		ADV,
		VERB,
		TITLE
	}

	public class Sentence
	{
		public Sentence()
		{
			Words = new List<Word>();
		}

		public Sentence(params Word[] _words)
		{
			Words = new List<Word>(_words);
		}

		public List<Word> Words { get; private set; }
	}

	public abstract class Word
	{
		private readonly string m_text;

		protected Word()
		{ }

		protected Word(string _text)
		{
			m_text = _text;
		}

		public string Text { get { return m_text; } }

		public abstract EWord Kind { get; }
	}

	public class Noun
	{
		public  Noun(string _text, ESex _sex, bool _isCreature)
		{
			Sex = _sex;
			IsCreature = _isCreature;
			Text = _text;
		}

		public string Text { get; private set; }

		public ESex Sex { get; private set; }

		public bool IsCreature { get; private set; }

		public Title Title { get; set; }
		public Adverb Adverb { get; set; }

		public static Noun operator +(Noun _a, Title _b)
		{
			_a.Title = _b;
			return _a;
		}

		public static Noun operator +(Noun _a, Adverb _b)
		{
			_a.Adverb = _b;
			return _a;
		}
	}

	public class Adverb : Word
	{
		public Adverb(string _text)
			: base(_text)
		{
		}

		public override EWord Kind
		{
			get { return EWord.ADV; }
		}
	}

	public class Title : Word
	{
		public Title(string _text, ESex _sex, bool _isCreature)
			: base(_text)
		{
			Sex = _sex;
			IsCreature = _isCreature;
		}
		
		public ESex Sex { get; private set; }

		public bool IsCreature { get; private set; }

		public override EWord Kind
		{
			get { return EWord.TITLE; }
		}
	}

	public class Verb : Word
	{
		public Verb(string _text)
			: base(_text)
		{
		}

		public override EWord Kind
		{
			get { return EWord.VERB; }
		}
	}

	public static class WordUtils
	{
		public static Noun AsNoun(this string _noun, ESex _sex, bool _isCreature)
		{
			return new Noun(_noun, _sex, _isCreature);
		}

		public static Adverb AsAdv(this string _adv)
		{
			return new Adverb(_adv);
		}

		public static Adverb AsVerb(this string _verb)
		{
			return new Adverb(_verb);
		}

		public static Title AsTitle(this string _title, ESex _sex, bool _isCreature)
		{
			return new Title(_title, _sex, _isCreature);
		}
	}
}
