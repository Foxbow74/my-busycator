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
			var noun = words[0];
			var lastChar = noun[noun.Length - 1];
			var vow = lastChar;
			if(!"йуеыаоэяию".Contains(lastChar)) vow = ' ';
			var lastTwoChars = noun.Substring(noun.Length - 2);
			var sklon = 0;

			if (_isCreature)
			{
				switch (_sex)
				{
					case ESex.MALE:
						if ("но,фи,чи".Contains(lastTwoChars))
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

				if ("ки,ко,ди,ги".Contains(lastTwoChars))
				{
					return _noun;
				}
			}
			switch (vow)
			{
				case 'а':
				case 'я':
					sklon = 1;
					noun = noun.Substring(0, noun.Length - 1);
					break;
				case 'о':
				case 'е':
					sklon = 2;
					noun = noun.Substring(0, noun.Length - 1);
					break;
				case ' ':
					switch (lastChar)
					{
						case 'ь':
							sklon = _sex == ESex.MALE ? 2 : 3;
							if (_sex == ESex.MALE && noun.EndsWith("ень"))
							{
								noun = noun.Substring(0, noun.Length - 3) + "н";
							}
							else
							{
								noun = noun.Substring(0, noun.Length - 1);
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
						noun = noun.Substring(0, noun.Length - 1);
					}
					break;
			}

			if (sklon == 0)
			{
				switch (lastChar)
				{
					case 'ь':
						sklon = _sex == ESex.MALE ? 2 : 3;
						if (_sex == ESex.MALE) noun = noun.Substring(0, noun.Length - 1);
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
			switch (sklon)
			{
				case 1:
					switch (_sex)
					{
						case ESex.MALE:
							isGluh = "гбнстплфхчшщрд".Contains(noun[noun.Length - 1]);
							break;
						case ESex.FEMALE:
							isGluh = "цзнстплфхчшщрд".Contains(noun[noun.Length - 1]);
							break;
						case ESex.IT:
							isGluh = "кнстплфхчшщрд".Contains(noun[noun.Length - 1]);
							break;
					}
					if (isGluh && sklon == 1 && noun.EndsWith("чк"))
					{
						isGluh = false;
					}
					break;
				case 2:
					switch (_sex)
					{
						case ESex.MALE:
							isGluh = "мкгбнстплфхчшщрд".Contains(noun[noun.Length - 1]);
							break;
						case ESex.FEMALE:
							isGluh = "кзнстплфхчшщрд".Contains(noun[noun.Length - 1]);
							break;
						case ESex.IT:
							isGluh = "щкнстплфхчшрд".Contains(noun[noun.Length - 1]);
							break;
					}
					break;
				case 3:
					switch (_sex)
					{
						case ESex.MALE:
							isGluh = "гбнстплфхчшщрд".Contains(noun[noun.Length - 1]);
							break;
						case ESex.FEMALE:
							isGluh = "знстплфхчшщрд".Contains(noun[noun.Length - 1]);
							break;
						case ESex.IT:
							isGluh = "кнстплфхчшщрд".Contains(noun[noun.Length - 1]);
							break;
					}
					break;
			}


			if(_sex!=ESex.IT && "йеяиью".Contains(lastChar))
			{
				isGluh = false;
			}


			switch (sklon)
			{
				case 1:
					switch (_target)
					{
						case EPadej.ROD:
							noun += isGluh ? "ы" : "и";
							break;
						case EPadej.DAT:
							noun += "е";
							break;
						case EPadej.VIN:
							noun += isGluh ? "у" : "ю";
							break;
						case EPadej.TVOR:
							noun += isGluh ? "ой" : "ей";
							break;
						case EPadej.PREDL:
							noun += "е";
							break;
						default:
							throw new ArgumentOutOfRangeException("_target");
					}
					break;
				case 2:
					switch (_target)
					{
						case EPadej.ROD:
							noun += isGluh ? "а" : "я";
							break;
						case EPadej.DAT:
							noun += isGluh ? "у" : "ю";
							break;
						case EPadej.VIN:
							if(_sex==ESex.MALE)
							{
								
							}
							else
							{
								noun += _isCreature ? (isGluh ? "а" : "я") : (isGluh ? "о" : "е");
							}
							break;
						case EPadej.TVOR:
							noun += isGluh ? "ом" : "ем";
							break;
						case EPadej.PREDL:
							noun += "е";
							break;
						default:
							throw new ArgumentOutOfRangeException("_target");
					}
					break;
				case 3:
					switch (_target)
					{
						case EPadej.ROD:
							noun += "и";
							break;
						case EPadej.DAT:
							noun += "и";
							break;
						case EPadej.VIN:
							noun += "ь";
							break;
						case EPadej.TVOR:
							noun += "ью";
							break;
						case EPadej.PREDL:
							noun += "и";
							break;
						default:
							throw new ArgumentOutOfRangeException("_target");
					}
					break;
			}

			words[0] = noun;
			return string.Join(" ", words);
		}

		public static string ToSex(string _sentence, ESex _sex)
		{
			if(_sex==ESex.MALE) return _sentence;

			var words = _sentence.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			var firstWord = words[0];
			if (firstWord.EndsWith("ик"))
			{
				firstWord = firstWord.Substring(0, firstWord.Length - 1) + "ца";
			}
			else if (firstWord.EndsWith("вец"))
			{
				firstWord = firstWord.Substring(0, firstWord.Length - 2) + "ка";
			}
			else if (firstWord.EndsWith("ец"))
			{
				firstWord = firstWord.Substring(0, firstWord.Length - 2) + "ица";
			}
			if (firstWord.EndsWith("нин"))
			{
				firstWord = firstWord.Substring(0, firstWord.Length - 2) + "ка";
			}
			if (firstWord.EndsWith("ин"))
			{
				firstWord = firstWord.Substring(0, firstWord.Length - 2) + "йка";
			}

			words[0] = firstWord;
			return string.Join(" ", words);
		}
	}
}
