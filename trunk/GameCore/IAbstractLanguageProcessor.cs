using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Messages;

namespace GameCore
{
	public interface IAbstractLanguageProcessor
	{
		IEnumerable<string> Compile(List<XLangMessage> _xlist);
		Noun AsNoun(EEquipmentPlaces _e);
		Noun AsNoun(EItemCategory _e);
		Noun AsNoun(ETerrains _e);
		Noun AsNoun(EActionCategory _e);
		Noun AsNoun(ETactics _e);
		Noun AsNoun(EALNouns _enoun);
		string GetString(EALSentence _sentence, params Noun[] _nouns);
		string GetString(EALConst _const);
		string GetString(EALVerbs _verb, Noun _noun);
	}
}