using GameCore.Acts;

namespace GameCore.Messages
{
	public class AskShootTargerMessage:AskMessage
	{
		public int MaxDistance { get; private set; }

		public AskShootTargerMessage(Act _act, int _maxDistance) : base(_act)
		{
			MaxDistance = _maxDistance;
		}
	}
}