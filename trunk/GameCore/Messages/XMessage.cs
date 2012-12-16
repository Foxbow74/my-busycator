using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Creatures;

namespace GameCore.Messages
{
	public class XMessage
	{
		public EALTurnMessage Type { get; private set; }
		public Creature Actor { get; private set; }
		public object[] Params { get; private set; }

		public XMessage(EALTurnMessage _type, Creature _actor, params object[] _params)
		{
			Type = _type;
			Actor = _actor;
			Params = _params;
		}

		public T First<T>()
		{
			return Params.OfType<T>().First();
		}

		public T FirstOrDefault<T>()
		{
			return Params.OfType<T>().FirstOrDefault();
		}

		public override string ToString()
		{
			return string.Format("{0} {1} [{2}]", Actor.Name, Type, string.Join(", ", Params.Select(e => e.ToString())));
		}
	}
}
