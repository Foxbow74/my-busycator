using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;

namespace LanguagePack
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
		static Sklonenia()
		{
			m_padejDict = new Dictionary<int, Dictionary<ESex, Dictionary<EPadej, Dictionary<bool, string>>>>();

			for (var i = 1; i <= 3; ++i)
			{
				m_padejDict.Add(i, new Dictionary<ESex, Dictionary<EPadej, Dictionary<bool, string>>>());
				foreach (ESex sex in Enum.GetValues(typeof(ESex)))
				{
					m_padejDict[i].Add(sex, new Dictionary<EPadej, Dictionary<bool, string>>());
					foreach (EPadej padej in Enum.GetValues(typeof(EPadej)))
					{
						m_padejDict[i][sex].Add(padej, new Dictionary<bool, string>());
					}
				}
			}

			m_padejDict[1][ESex.FEMALE][EPadej.ROD][true] = "ы";
			m_padejDict[1][ESex.FEMALE][EPadej.ROD][false] = "и";

			m_padejDict[1][ESex.FEMALE][EPadej.VIN][true] = "у";
			m_padejDict[1][ESex.FEMALE][EPadej.VIN][false] = "у";

			m_padejDict[1][ESex.FEMALE][EPadej.DAT][true] = "е";
			m_padejDict[1][ESex.FEMALE][EPadej.DAT][false] = "е";

			m_padejDict[1][ESex.FEMALE][EPadej.TVOR][true] = "ой";
			m_padejDict[1][ESex.FEMALE][EPadej.TVOR][false] = "ой";

			m_padejDict[1][ESex.FEMALE][EPadej.PREDL][true] = "е";
			m_padejDict[1][ESex.FEMALE][EPadej.PREDL][false] = "е";

			m_padejDict[1][ESex.MALE][EPadej.ROD][true] = "ы";
			m_padejDict[1][ESex.MALE][EPadej.ROD][false] = "и";

			m_padejDict[1][ESex.MALE][EPadej.VIN][true] = "у";
			m_padejDict[1][ESex.MALE][EPadej.VIN][false] = "ю";

			m_padejDict[1][ESex.MALE][EPadej.DAT][true] = "е";
			m_padejDict[1][ESex.MALE][EPadej.DAT][false] = "е";

			m_padejDict[1][ESex.MALE][EPadej.TVOR][true] = "ой";
			m_padejDict[1][ESex.MALE][EPadej.TVOR][false] = "ей";

			m_padejDict[1][ESex.MALE][EPadej.PREDL][true] = "е";
			m_padejDict[1][ESex.MALE][EPadej.PREDL][false] = "е";
		}

		private static Dictionary<int, Dictionary<ESex, Dictionary<EPadej, Dictionary<bool, string>>>> m_padejDict;

		public static string ToPadej(EPadej _target, string _noun, bool _isCreature, ESex _sex)
		{
			if(_target==EPadej.IMEN) return _noun;


			var sklon = GetSklon(ref _noun, _isCreature, _sex);
			if(sklon<0)
			{
				return _noun;
			}
			
			var isGluh = false;
			var lastChar = _noun[_noun.Length - 1];

			switch (sklon)
			{
				case 1:
					switch (_sex)
					{
						case ESex.MALE:
							isGluh = "гбнстплфхчшщрд".Contains(lastChar);
							break;
						case ESex.FEMALE:
							isGluh = "вцзнстплфхчшщрд".Contains(lastChar);
							break;
						case ESex.IT:
							isGluh = "кнстплфхчшщрд".Contains(lastChar);
							break;
					}
					if (isGluh && sklon == 1 && _noun.EndsWith("чк"))
					{
						isGluh = false;
					}
					break;
				case 2:
					switch (_sex)
					{
						case ESex.MALE:
							isGluh = "мкгбнстплфхчшщрд".Contains(lastChar);
							break;
						case ESex.FEMALE:
							isGluh = "кзнстплфхчшщрд".Contains(lastChar);
							break;
						case ESex.IT:
							isGluh = "щкнстплфхчшрд".Contains(lastChar);
							break;
					}
					break;
				case 3:
					switch (_sex)
					{
						case ESex.MALE:
							isGluh = "гбнстплфхчшщрд".Contains(lastChar);
							break;
						case ESex.FEMALE:
							isGluh = "знстплфхчшщрд".Contains(lastChar);
							break;
						case ESex.IT:
							isGluh = "кнстплфхчшщрд".Contains(lastChar);
							break;
					}
					break;
			}


			if(_sex!=ESex.IT && "йеяиью".Contains(lastChar))
			{
				isGluh = false;
			}

			//switch (sklon)
			//{
			//    case 1:
			//        _noun = _noun.Substring(0, _noun.Length - 1);
			//        break;
			//    case 2:
			//        break;
			//    case 3:
			//        _noun = _noun.Substring(0, _noun.Length - 1);
			//        break;
			//}

			string value;
			if (m_padejDict[sklon][_sex][_target].TryGetValue(isGluh, out value))
			{
				_noun += value;
			}
			else
			{

				switch (sklon)
				{
					case 1:
						throw new ApplicationException("не должно сюда попасть");
					case 2:
						switch (_target)
						{
							case EPadej.ROD:
								_noun += isGluh ? "а" : "я";
								break;
							case EPadej.DAT:
								_noun += isGluh ? "у" : "ю";
								break;
							case EPadej.VIN:
								if (_sex == ESex.MALE)
								{

								}
								else
								{
									_noun += _isCreature ? (isGluh ? "а" : "я") : (isGluh ? "о" : "е");
								}
								break;
							case EPadej.TVOR:
								_noun += isGluh ? "ом" : "ем";
								break;
							case EPadej.PREDL:
								_noun += "е";
								break;
							default:
								throw new ArgumentOutOfRangeException("_target");
						}
						break;
					case 3:
						switch (_target)
						{
							case EPadej.ROD:
								_noun += "и";
								break;
							case EPadej.DAT:
								_noun += "и";
								break;
							case EPadej.VIN:
								_noun += "ь";
								break;
							case EPadej.TVOR:
								_noun += "ью";
								break;
							case EPadej.PREDL:
								_noun += "и";
								break;
							default:
								throw new ArgumentOutOfRangeException("_target");
						}
						break;
				}
			}
			
			return _noun;
		}

		private static int GetSklon(ref string _noun, bool _isCreature, ESex _sex)
		{
			var lastChar = _noun[_noun.Length - 1];
			var vow = lastChar;
			if (!"йуеыаоэяию".Contains(lastChar)) vow = ' ';
			var lastTwoChars = _noun.Substring(_noun.Length - 2);
			var sklon = 0;

			if (_isCreature)
			{
				switch (_sex)
				{
					case ESex.MALE:
						if ("но,фи,чи".Contains(lastTwoChars))
						{
							return -1;
						}
						break;
					case ESex.FEMALE:
						if ("ли,".Contains(lastTwoChars))
						{
							return -1;
						}
						break;
				}

				if ("ки,ко,ди,ги".Contains(lastTwoChars))
				{
					return -1;
				}
			}
			switch (vow)
			{
				case 'а':
				case 'я':
					sklon = 1;
					_noun = _noun.Substring(0, _noun.Length - 1);
					break;
				case 'о':
				case 'е':
					sklon = 2;
					_noun = _noun.Substring(0, _noun.Length - 1);
					break;
				case ' ':
					switch (lastChar)
					{
						case 'ь':
							sklon = _sex == ESex.MALE ? 2 : 3;
							if (_sex == ESex.MALE && _noun.EndsWith("ень"))
							{
								_noun = _noun.Substring(0, _noun.Length - 3) + "н";
							}
							else
							{
								_noun = _noun.Substring(0, _noun.Length - 1);
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
						_noun = _noun.Substring(0, _noun.Length - 1);
					}
					break;
			}

			if (sklon == 0)
			{
				switch (lastChar)
				{
					case 'ь':
						sklon = _sex == ESex.MALE ? 2 : 3;
						if (_sex == ESex.MALE) _noun = _noun.Substring(0, _noun.Length - 1);
						break;
				}
			}
			if (sklon == 0)
			{
				if (_isCreature)
				{
					return -1;
				}
				throw new ApplicationException();
			}
			return sklon;
		}

		public static string Пунктов(this int _cnt)
		{
			var last = _cnt%10;
			string result;
			if(last==1)
			{
				result =  "пункт";
			}
			else if(last>1 && last<5)
			{
				result = "пункта";
			}
			else
			{
				result = "пунктов";
			}
			return string.Format("{0} {1}", _cnt, result);
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
