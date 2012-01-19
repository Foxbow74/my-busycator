using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RGL1.Messages
{
	public class TextMessage:Message
	{
		public TextMessage(EMessageType _type, string _text, Dictionary<string, Color> _highlights) : base(_type, _text, _highlights)
		{
		}
	}
}