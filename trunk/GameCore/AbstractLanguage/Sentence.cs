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
}