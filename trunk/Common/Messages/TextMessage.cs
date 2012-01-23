using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Common.Messages
{
	public class TextMessage:Message
	{
		public TextMessage(EMessageType _type, string _text, Dictionary<string, Color> _highlights)
			: base(_type, _text, _highlights)
		{
		}

		public TextMessage(EMessageType _type, string _text)
			: base(_type, _text, null)
		{
		}
	}

	public class TurnMessage : Message
	{
		public TurnMessage()
			: base(EMessageType.INFO, null, null)
		{
		}
	}

	public class AvatarMovedMessage : Message
	{
		public AvatarMovedMessage()
			: base(EMessageType.INFO, null, null)
		{
		}
	}
}