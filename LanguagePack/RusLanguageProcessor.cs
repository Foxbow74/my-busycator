using System;
using System.Collections.Generic;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Messages;

namespace LanguagePack
{
	public partial class RusLanguageProcessor : IAbstractLanguageProcessor
	{
		#region Fields

		private static readonly Dictionary<EALConst, string> m_consts = new Dictionary<EALConst, string>();
		private static readonly Dictionary<EALNouns, Noun> m_nouns = new Dictionary<EALNouns, Noun>();
		private static readonly Dictionary<EALVerbs, Verb> m_verbs = new Dictionary<EALVerbs, Verb>();

		#endregion

		#region .ctor

		static RusLanguageProcessor()
		{
			FillConstants();
			FillNouns();
			FillVerbs();
		}

		#endregion

		#region Methods

		private string ToGeneral(string _s)
		{
			if (string.IsNullOrEmpty(_s)) return _s;
			return _s.Substring(0, 1).ToUpper() + _s.Substring(1);
		}

		#endregion

		#region IAbstractLanguageProcessor Members

		public IEnumerable<string> Compile(List<XLangMessage> _xlist)
		{
			return XMessageCompiler.Compile(_xlist);
		}

		public Noun AsNoun(EALNouns _enoun)
		{
			return m_nouns[_enoun];
		}

		public string GetString(EALSentence _sentence, params Noun[] _nouns)
		{
			switch (_sentence)
			{
				case EALSentence.NONE:
					return _nouns[0].To(EPadej.IMEN);
				case EALSentence.GENERAL:
					return ToGeneral(_nouns[0].To(EPadej.IMEN));
				case EALSentence.TACTICK_CHANGED:
					return "Тактика изменена на " + _nouns[0].To(EPadej.VIN);
				default:
					throw new ArgumentOutOfRangeException("_sentence");
			}
		}

		public string GetString(EALConst _const)
		{
			return m_consts[_const];
		}

		public string GetString(EALVerbs _verb, Noun _noun, EVerbType _type)
		{
			return m_verbs[_verb].To(_noun.Sex, _type);
		}

		#endregion
	}
}