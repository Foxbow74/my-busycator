using System.Collections.Generic;

namespace GameCore.AbstractLanguage
{
	public enum EWord
	{
		ADV,
		VERB,
		TITLE,
		IMMUTABLE
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

		public Noun AlsoKnownAs { get; private set; }
		public Adjective Adjective { get; set; }
		public Immutable Immutable { get; set; }
		protected OfSomething OfSomething { get; set; }

		public static Noun operator +(Noun _a, Noun _b)
		{
			_a.AlsoKnownAs = _b;
			return _a;
		}

		public static Noun operator +(Noun _a, Adjective _b)
		{
			_a.Adjective = _b;
			return _a;
		}

		public static Noun operator +(Noun _a, Immutable _b)
		{
			_a.Immutable = _b;
			return _a;
		}

		public static Noun operator +(Noun _a, OfSomething _b)
		{
			_a.OfSomething = _b;
			return _a;
		}
	}

	public class Adjective : Word
	{
		public Adjective(string _text)
			: base(_text)
		{
		}

		public override EWord Kind
		{
			get { return EWord.ADV; }
		}
	}

	public class Immutable : Word
	{
		public Immutable(string _text)
			: base(_text)
		{
		}

		public override EWord Kind
		{
			get { return EWord.IMMUTABLE; }
		}
	}


	public class OfSomething : Word
	{
		public OfSomething(string _text)
			: base(_text)
		{
		}

		public override EWord Kind
		{
			get { return EWord.IMMUTABLE; }
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

		public static Adjective AsAdj(this string _adv)
		{
			return new Adjective(_adv);
		}

		public static Adjective AsVerb(this string _verb)
		{
			return new Adjective(_verb);
		}

		public static Immutable AsIm(this string _string)
		{
			return new Immutable(_string);
		}

		public static OfSomething AsOf(this string _string)
		{
			return new OfSomething(_string);
		}
	}
}
