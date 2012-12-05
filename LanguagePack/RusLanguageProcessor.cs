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

		internal static Dictionary<EALConst, string> Consts = new Dictionary<EALConst, string>();
		internal static Dictionary<EALNouns, Noun> Nouns = new Dictionary<EALNouns, Noun>();

		#endregion

		#region .ctor

		static RusLanguageProcessor()
		{
			FillConstants();

			Nouns.Add(EALNouns.StackOf, "кучка".AsNoun(ESex.FEMALE, false));
			Nouns.Add(EALNouns.Grave, "могила".AsNoun(ESex.FEMALE, false));
			Nouns.Add(EALNouns.Chair, "стул".AsNoun(ESex.MALE, false));
			Nouns.Add(EALNouns.Cabinet, "шкаф".AsNoun(ESex.MALE, false));
			Nouns.Add(EALNouns.ArmorRack, "стойка".AsNoun(ESex.FEMALE, false) + " для брони".AsIm());
			Nouns.Add(EALNouns.WeaponRack, "стойка".AsNoun(ESex.FEMALE, false) + " для оружия".AsIm());
			Nouns.Add(EALNouns.Barrel, "бочка".AsNoun(ESex.FEMALE, false));
			Nouns.Add(EALNouns.Stair, "лестница".AsNoun(ESex.FEMALE, false));
			Nouns.Add(EALNouns.StairUp, "лестница".AsNoun(ESex.FEMALE, false) + "вверх".AsIm());
			Nouns.Add(EALNouns.StairDown, "лестница".AsNoun(ESex.FEMALE, false) + "вниз".AsIm());
			Nouns.Add(EALNouns., );
			Nouns.Add(EALNouns., );
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
			return Nouns[_enoun];
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
			return Consts[_const];
		}

		#endregion
	}
}