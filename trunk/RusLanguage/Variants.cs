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
					break;
				case ESex.FEMALE:
					return "там была ";
					break;
				case ESex.IT:
					return "там было ";
					break;
				default:
					throw new ArgumentOutOfRangeException("_sex");
			}
		}
	}
}
