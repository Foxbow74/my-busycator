using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Essences;

namespace LanguagePack
{
	public static class Sklonenia
	{
		private static readonly Dictionary<int, Dictionary<ESex, Dictionary<EPadej, Dictionary<bool, string>>>> m_padejDict;

		private static readonly Dictionary<string, Dictionary<EPadej, string>> m_iskluchenia = new Dictionary<string, Dictionary<EPadej, string>>();

		private static readonly Random m_rnd = new Random();

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


			AddIskl("зубы", "зубов", "зубам", "зубов", "зубами", "зубах");
			AddIskl("он", "его", "ему", "его", "им", "нем");
			AddIskl("она", "ее", "ей", "ее", "ею", "ней");
			AddIskl("оно", "их", "им", "их", "ими", "них");
			AddIskl("они", "их", "им", "их", "ими", "них");
		}

		public static string To(this Essence _essence, EPadej _padej)
		{
			return _essence.Name.To(_padej);
		}

		private static void AddIskl(string _imen, string _rod,string _dat, string _vin, string _tvor, string _predl)
		{
			var dictionary = new Dictionary<EPadej, string>
			                 	{
			                 		{EPadej.ROD, _rod},
			                 		{EPadej.DAT, _dat},
			                 		{EPadej.PREDL, _predl},
			                 		{EPadej.TVOR, _tvor},
			                 		{EPadej.VIN, _vin}
			                 	};
			m_iskluchenia.Add(_imen, dictionary);
		}

		public static string NounToPadej(EPadej _target, string _noun, bool _isCreature, ESex _sex)
		{
			if(_target==EPadej.IMEN) return _noun;

			Dictionary<EPadej, string> dictionary;
			if(m_iskluchenia.TryGetValue(_noun, out dictionary))
			{
				return dictionary[_target];
			}

			if (_sex == ESex.PLURAL || _sex == ESex.PLURAL_FEMALE)
			{
				return PluralNounToPadej(_target, _noun, _isCreature, _sex);
			}

			bool мягкий;
			var noun = _noun;
			var sklon = GetSklon(ref noun, _isCreature, _sex, out мягкий);

			if(sklon<0)
			{
				return _noun;
			}

			if (sklon == 1 && _sex == ESex.FEMALE && _noun.EndsWith("ка"))
			{
				мягкий = true;
			}
			var твердый = !мягкий;
			string value;
			if (m_padejDict[sklon][_sex][_target].TryGetValue(твердый, out value))
			{
				noun += value;
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
								noun += твердый ? "а" : "я";
								break;
							case EPadej.DAT:
								noun += твердый ? "у" : "ю";
								break;
							case EPadej.VIN:
								if (_sex == ESex.MALE)
								{
									if (_isCreature)
									{
										noun += твердый ? "а" : "я";
									}
									else
									{
										noun = _noun;
									}
								}
								else
								{
									noun += _isCreature ? (твердый ? "а" : "я") : (твердый ? "о" : "е");
								}
								break;
							case EPadej.TVOR:
								noun += твердый ? "ом" : "ем";
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
			}
			
			return noun;
		}

		private static int GetSklon(ref string _noun, bool _isCreature, ESex _sex, out bool _мягкий)
		{
			_мягкий = false;

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

			char смягчитель = vow;

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
							смягчитель = lastChar;
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
							if (_sex == ESex.MALE && _noun.EndsWith("ец"))
							{
								_noun = _noun.Substring(0, _noun.Length - 2) + "ц";
							}
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
						смягчитель = lastChar;
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

			_мягкий = "йеяью".Contains(смягчитель);
			return sklon;
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

		public static string To(this Noun _noun, EPadej _padej)
		{
			if(_noun.AlsoKnownAs!=null && YesNo())
			{
				return _noun.AlsoKnownAs.To(_padej);
			}
			if (_noun.CoName!=null)
			{
				
			}
			return _noun.Adjective.To(_padej, _noun.Sex) + NounToPadej(_padej, _noun.Text, _noun.IsCreature, _noun.Sex) + CoNameToPadej(_padej, _noun.CoName, _noun.IsCreature, _noun.Sex) + _noun.Immutable.ToText();
		}

		public static string To(this Verb _verb, ESex _sex, EVerbType _type)
		{
			var v = _verb.SameAs[m_rnd.Next(_verb.SameAs.Count)];
			var text = _type==EVerbType.IN_PROCESS?v.InProcess : v.Done;
			if(_sex==ESex.MALE) return text;
			if (_type == EVerbType.DONE)
			{
				if (text.EndsWith("ил") || text.EndsWith("ал"))
				{
					switch (_sex)
					{
						case ESex.FEMALE:
							return text + "а";
						case ESex.IT:
							return text + "о";
						case ESex.PLURAL:
						case ESex.PLURAL_FEMALE:
							return text + "и";
						default:
							throw new ArgumentOutOfRangeException("_sex");
					}
				}
				else if (text.EndsWith("ся"))
				{
					switch (_sex)
					{
						case ESex.FEMALE:
							return text.Substring(0, text.Length - 2) + "ась";
						case ESex.IT:
							return text.Substring(0, text.Length - 2) + "ось";
						case ESex.PLURAL:
						case ESex.PLURAL_FEMALE:
							return text.Substring(0, text.Length - 2) + "ись";
						default:
							throw new ArgumentOutOfRangeException("_sex");
					}
				}
			}
			else
			{
				switch (_sex)
				{
					case ESex.MALE:
					case ESex.FEMALE:
					case ESex.IT:
						return text;
					case ESex.PLURAL:
					case ESex.PLURAL_FEMALE:
						if (text.EndsWith("ит"))
						{
							return text.Substring(0, text.Length - 2) + "ят";
						}
						if (text.EndsWith("ет"))
						{
							return text.Substring(0, text.Length - 2) + "ют";
						}
						if (text.EndsWith("ется"))
						{
							return text.Substring(0, text.Length - 4) + "ются";
						}
						if (text.EndsWith("ится"))
						{
							return text.Substring(0, text.Length - 4) + "атся";
						}
						break;
					default:
						throw new ArgumentOutOfRangeException("_sex");
				}
			}
			throw new NotImplementedException(text + " " + _sex + " " + _type);
		}

		private static string CoNameToPadej(EPadej _padej, CoName _coName, bool _isCreature, ESex _sex)
		{
			if (_coName==null)
			{
				return "";
			}
			return "-" + NounToPadej(_padej, _coName.Text, _isCreature, _sex);
		}

		public static bool? YesNoAnswer { get; set; }

		private static bool YesNo()
		{
			if (YesNoAnswer.HasValue)
			{
				return YesNoAnswer.Value;
			}
			return m_rnd.Next(2)==0;
		}

		public static string ToText(this Immutable _imm)
		{
			if (_imm == null) return "";
			return " " + _imm.Text;
		}
		

		public static string To(this Adjective _adv, EPadej _padej, ESex _sex)
		{
			if (_adv == null) return "";
			var text = _adv.Text;
			if (text.EndsWith("ый"))
			{
				text = text.Substring(0, text.Length - 2);
				switch (_sex)
				{
					case ESex.MALE:
						text += new[] { "ый", "ого", "ому", "ого", "ым", "ом" }[(int)_padej];
						break;
					case ESex.FEMALE:
						text += new[] { "ая", "ой", "ой", "ой", "ой", "ой" }[(int)_padej];
						break;
					case ESex.IT:
						text += new[] { "ое", "ого", "ому", "ого", "ым", "ом" }[(int)_padej];
						break;
					case ESex.PLURAL:
						text += new[] { "ые", "ых", "ым", "ых", "ыми", "ых" }[(int)_padej];
						break;
					default:
						throw new ArgumentOutOfRangeException("_sex");
				}
			}
			else if (text.EndsWith("ий"))
			{
				text = text.Substring(0, text.Length - 2);
				switch (_sex)
				{
					case ESex.MALE:
						text += new[] { "ий", "ого", "ому", "ого", "им", "ом" }[(int)_padej];
						break;
					case ESex.FEMALE:
						text += new[] { "ая", "ой", "ой", "ой", "ой", "ой" }[(int)_padej];
						break;
					case ESex.IT:
						text += new[] { "ое", "ого", "ому", "ого", "им", "ом" }[(int)_padej];
						break;
					case ESex.PLURAL:
						text += new[] { "ие", "их", "им", "их", "ими", "их" }[(int)_padej];
						break;
					default:
						throw new ArgumentOutOfRangeException("_sex");
				}
			}
			else if (text.EndsWith("ой"))
			{
				text = text.Substring(0, text.Length - 2);
				switch (_sex)
				{
					case ESex.MALE:
						text += new[] { "ой", "ого", "ому", "ого", "ым", "ом" }[(int)_padej];
						break;
					case ESex.FEMALE:
						text += new[] { "ая", "ой", "ой", "ой", "ой", "ой" }[(int)_padej];
						break;
					case ESex.IT:
						text += new[] { "ое", "ого", "ому", "ого", "ым", "ом" }[(int)_padej];
						break;
					case ESex.PLURAL:
						text += new[] { "ые", "ых", "ым", "ых", "ыми", "ых" }[(int)_padej];
						break;
					default:
						throw new ArgumentOutOfRangeException("_sex");
				}
			}
			else
			{
				throw new NotImplementedException();
			}

			return text + " ";
		}


		private static string PluralNounToPadej(EPadej _padej, string _noun, bool _isCreature, ESex _sex)
		{
			bool мягкий;
			var last = _noun[_noun.Length - 1];

			string text;
			switch (last)
			{
				case 'ы':
					мягкий = false;
					text = _noun.Substring(0, _noun.Length - 1);
					break;
				case 'и':
					мягкий = true;
					text = _noun.Substring(0, _noun.Length - 1);
					break;
				default:
					throw new ArgumentOutOfRangeException("last");
			}
			switch (_sex)
			{
				case ESex.PLURAL:
					if(мягкий)
					{
						text += new[] { "и", "ей", "ям", "ей", "ами", "ях" }[(int)_padej];	
					}
					else
					{
						text += new[] { "ы", "ов", "ам", "ов", "ами", "ах" }[(int)_padej];	
					}
					
					break;
				default:
					throw new ArgumentOutOfRangeException("_sex");
			}
			return text;
		}
	}
}
