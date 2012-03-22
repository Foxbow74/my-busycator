using System;
using System.Linq;

namespace RusLanguage
{
	public enum EPadej
	{
		IMEN,
		ROD,
		DAT,
		VIN,
		TVOR,
		PREDL,
	}

	public static class Sklonenia
	{
		public static string ToPadej(EPadej _target, string _noun, bool _isCreature, ESex _sex)
		{
			if(_target==EPadej.IMEN) return _noun;

			var words = _noun.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
			var firstWord = words[0];
			var lastChar = firstWord[firstWord.Length - 1];
			var vow = lastChar;
			if(!"йуеыаоэяию".Contains(lastChar)) vow = ' ';
			var lastTwoChars = firstWord.Substring(firstWord.Length - 2);
			var sklon = 0;
			
			switch (_sex)
			{
				case ESex.MALE:
					if("но,фи,чи".Contains(lastTwoChars))
					{
						return _noun;
					}
					break;
				case ESex.FEMALE:
					if ("ли,".Contains(lastTwoChars))
					{
						return _noun;
					}
					break;
			}

			if ("ки,ди,ги".Contains(lastTwoChars))
			{
				return _noun;
			}
			
			switch (vow)
			{
				case 'а':
				case 'я':
					sklon = 1;
					firstWord = firstWord.Substring(0, firstWord.Length - 1);
					break;
				case 'о':
				case 'е':
					sklon = 2;
					firstWord = firstWord.Substring(0, firstWord.Length - 1);
					break;
				case ' ':
					switch (lastChar)
					{
						case 'ь':
							sklon = _sex == ESex.MALE ? 2 : 3;
							if (_sex == ESex.MALE && firstWord.EndsWith("ень"))
							{
								firstWord = firstWord.Substring(0, firstWord.Length - 3) + "н";
							}
							else
							{
								firstWord = firstWord.Substring(0, firstWord.Length - 1);
							}
							break;
						default:
							sklon = _sex == ESex.MALE ? 2 : 3;
							break;
					}
					break;
				case 'й':
					if (_sex == ESex.MALE)
					{
						sklon = 2;
						firstWord = firstWord.Substring(0, firstWord.Length - 1);
					}
					break;
			}

			if (sklon == 0)
			{
				switch (lastChar)
				{
					case 'ь':
						sklon = _sex == ESex.MALE ? 2 : 3;
						if (_sex == ESex.MALE) firstWord = firstWord.Substring(0, firstWord.Length - 1);
						break;
				}
			}
			if (sklon == 0)
			{
				if(_isCreature)
				{
					return _noun;
				}
				throw new ApplicationException();
			}

			var isGluh = false;
			switch (_sex)
			{
				case ESex.MALE:
					isGluh = "бкнстплфхчшщрд".Contains(firstWord[firstWord.Length - 1]);
					break;
				case ESex.FEMALE:
					isGluh = "зкнстплфхчшщрд".Contains(firstWord[firstWord.Length - 1]);
					break;
				case ESex.IT:
					isGluh = "кнстплфхчшщрд".Contains(firstWord[firstWord.Length - 1]);
					break;
			}
			
			if("йеяиью".Contains(lastChar))
			{
				isGluh = false;
			}

			switch (sklon)
			{
				case 1:
					switch (_target)
					{
						case EPadej.ROD:
							firstWord += isGluh ? "ы" : "и";
							break;
						case EPadej.DAT:
							firstWord += "е";
							break;
						case EPadej.VIN:
							firstWord += isGluh ? "у" : "ю";
							break;
						case EPadej.TVOR:
							firstWord += isGluh ? "ой" : "ей";
							break;
						case EPadej.PREDL:
							firstWord += "е";
							break;
						default:
							throw new ArgumentOutOfRangeException("_target");
					}
					break;
				case 2:
					switch (_target)
					{
						case EPadej.ROD:
							firstWord += isGluh ? "а" : "я";
							break;
						case EPadej.DAT:
							firstWord += isGluh ? "у" : "ю";
							break;
						case EPadej.VIN:
							if(_sex==ESex.MALE)
							{
								
							}
							else
							{
								firstWord += _isCreature ? (isGluh ? "а" : "я") : (isGluh ? "о" : "е");
							}
							break;
						case EPadej.TVOR:
							firstWord += isGluh ? "ом" : "ем";
							break;
						case EPadej.PREDL:
							firstWord += "е";
							break;
						default:
							throw new ArgumentOutOfRangeException("_target");
					}
					break;
				case 3:
					switch (_target)
					{
						case EPadej.ROD:
							firstWord += "и";
							break;
						case EPadej.DAT:
							firstWord += "и";
							break;
						case EPadej.VIN:
							firstWord += "ь";
							break;
						case EPadej.TVOR:
							firstWord += "ью";
							break;
						case EPadej.PREDL:
							firstWord += "и";
							break;
						default:
							throw new ArgumentOutOfRangeException("_target");
					}
					break;
			}

			words[0] = firstWord;
			return string.Join(" ", words);
		}
	}
}
