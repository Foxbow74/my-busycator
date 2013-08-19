namespace GameCore.AbstractLanguage
{
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