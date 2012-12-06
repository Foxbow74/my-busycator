using System.Collections.Generic;

namespace GameCore.AbstractLanguage
{
	public class Sentence
	{
		public Sentence()
		{
			Words = new List<AbstractWord>();
		}

		public Sentence(params AbstractWord[] _abstractWords)
		{
			Words = new List<AbstractWord>(_abstractWords);
		}

		public List<AbstractWord> Words { get; private set; }
	}

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
		public CoName CoName { get; private set; }

		public static Noun operator +(Noun _a, Noun _b)
		{
			_a.AlsoKnownAs = _b;
			if (_b.Adjective == null)
			{
				_b.Adjective = _a.Adjective;
			}
			if (_b.Immutable == null)
			{
				_b.Immutable = _a.Immutable;
			}
			if (_b.OfSomething == null)
			{
				_b.OfSomething = _a.OfSomething;
			}
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

		public static Noun operator +(Noun _a, CoName _b)
		{
			_a.CoName = _b;
			return _a + new Noun(_b.Text, _a.Sex, _a.IsCreature);
		}

		public override string ToString()
		{
			return Text;
		}
	}

	public class CoName: AbstractWord
	{
		public CoName(string _text):base(_text)
		{
		}
	}


	/// <summary>
	/// исходня форма - какой?
	/// </summary>
	public class Adjective : AbstractWord
	{
		public Adjective(string _text)
			: base(_text)
		{
		}
	}


	/// <summary>
	/// неизменное при склонении слово или сочетание
	/// </summary>
	public class Immutable : AbstractWord
	{
		public Immutable(string _text)
			: base(_text)
		{
		}
	}

	/// <summary>
	/// XX чего либо, например, король "воров"
	/// </summary>
	public class OfSomething : AbstractWord
	{
		public OfSomething(string _text)
			: base(_text)
		{
		}
	}

	public enum EVerbType
	{
		IN_PROCESS,
		DONE,
	}

	/// <summary>
	/// исходная форма - что сделал
	/// </summary>
	public class Verb
	{
		public string InProcess { get; private set; }
		public string Done { get; private set; }

		public Verb(string _inProcess, string _done)
		{
			InProcess = _inProcess;
			Done = _done;
			SameAs = new List<Verb> {this};
		}

		public List<Verb> SameAs { get; private set; }

		public static Verb operator +(Verb _a, Verb _b)
		{
			_a.SameAs.Add(_b);
			return _a;
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

		public static Immutable AsIm(this string _string)
		{
			return new Immutable(_string);
		}

		public static OfSomething AsOf(this string _string)
		{
			return new OfSomething(_string);
		}

		public static CoName AsCo(this string _coname)
		{
			if (_coname == null) return null;
			return new CoName(_coname);
		}
	}
}
