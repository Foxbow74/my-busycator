using GameCore.Acts;
using GameCore.Objects;

namespace GameCore.Messages
{
	public class AskHowMuchMessage : AskMessage
	{
		public AskHowMuchMessage(Act _act, ThingDescriptor _descriptor, int _total)
			: base(_act)
		{
			Descriptor = _descriptor;
			Total = _total;
		}

		public ThingDescriptor Descriptor { get; private set; }

		public int Total { get; private set; }
	}
}