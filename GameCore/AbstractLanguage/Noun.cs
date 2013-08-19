namespace GameCore.AbstractLanguage
{
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
			_a.Add(_b);
			return _a;
		}

		private void Add(Noun _noun)
		{
			if (AlsoKnownAs != null)
			{
				AlsoKnownAs.Add(_noun);
			}
			else
			{
				AlsoKnownAs = _noun;
				if (_noun.Adjective == null)
				{
					_noun.Adjective = Adjective;
				}
				if (_noun.Immutable == null)
				{
					_noun.Immutable = Immutable;
				}
				if (_noun.OfSomething == null)
				{
					_noun.OfSomething = OfSomething;
				}
			}
		}

		public static Noun operator +(Noun _a, Adjective _b)
		{
			if (_a.Adjective != null)
			{
				_a.Add(new Noun(_a.Text, _a.Sex, _a.IsCreature) + _b);
			}
			else
			{
				_a.Adjective = _b;
			}
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

		public Noun Clone()
		{
			return (Noun)MemberwiseClone();
		}
	}
}