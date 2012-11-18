using GameCore.Acts;

namespace GameCore.Messages
{
	public class AskMessageNg : AskMessage
	{
		public AskMessageNg(Act _act, EAskMessageType _type, params object[] _params) : base(_act, _params) { AskMessageType = _type; }

		public EAskMessageType AskMessageType { get; private set; }
	}
}