using System;

namespace RusLanguage
{
	public static class Variants
	{
		public static string ThereIsWas(ESex _sex, Random _rnd) 
		{
			switch (_sex)
			{
				case ESex.MALE:
					return "там был ";
				case ESex.FEMALE:
					return "там была ";
				case ESex.IT:
					return "там было ";
				default:
					throw new ArgumentOutOfRangeException("_sex");
			}
		}
	}
}
