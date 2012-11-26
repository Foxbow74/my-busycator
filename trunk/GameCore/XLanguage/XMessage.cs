using System.Linq;
using GameCore.Creatures;

namespace GameCore.XLanguage
{
	public class XMessage
	{
		public EXMType Type { get; private set; }
		public Creature Actor { get; private set; }
		public object[] Params { get; private set; }

		public XMessage(EXMType _type, Creature _actor, params object[] _params)
		{
			Type = _type;
			Actor = _actor;
			Params = _params;
		}

		public T First<T>()
		{
			return Params.OfType<T>().First();
		}
	}

	public enum EXMType
	{
		CREATURE_CLOSES_IT,
		CREATURE_OPENS_IT,
		CREATURE_DROPS_IT,
		CREATURE_TAKES_IT,
		CONTAINER_IS_EMPTY,
		CREATURE_DRINKS_IT,
		CREATURE_LIGHT_OFF_IT,
		CREATURE_LIGHT_ON_IT,
		CREATURE_KILLED,
		CREATURE_TAKES_DAMAGE,
		AVATAR_IS_LUCK,
		CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK,
		CREATURES_ATTACK_DAMAGE_IS_ZERO,
		CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK,
		CREATURES_ATTACK_DAMAGE_ADSORBED
	}

	public class XName
	{
		
	}
}
