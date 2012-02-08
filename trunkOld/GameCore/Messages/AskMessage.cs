using GameCore.Acts;

namespace GameCore.Messages
{
	public abstract class AskMessage : Message
	{
		protected AskMessage(Act _act)
		{
			Act = _act;
		}

		public Act Act { get; private set; }
	}
}