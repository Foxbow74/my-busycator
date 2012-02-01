using GameCore.Acts;
using GameCore.Misc;

namespace GameCore.Messages
{
	public class AskDirectionMessage : AskMessage
	{
		public AskDirectionMessage(Act _act, Point _point)
			: base(_act)
		{
			Point = _point;
		}

		public Point Point { get; private set; }
	}
}