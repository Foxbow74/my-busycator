using GameCore.Acts;

namespace GameCore.Messages
{
	public class AskShootTargerMessage : AskMessage
	{
		public AskShootTargerMessage(Act _act, int _maxDistance) : base(_act)
		{
			MaxDistance = _maxDistance;
		}

		public int MaxDistance { get; private set; }
	}

	public class AskDestinationMessage : AskMessage
	{
		public AskDestinationMessage(Act _act)
			: base(_act)
		{
		}
	}
}