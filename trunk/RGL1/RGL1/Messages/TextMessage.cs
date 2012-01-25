using System.Collections.Generic;
using GameCore.Messages;
using Microsoft.Xna.Framework;

namespace RGL1.Messages
{
	public class TextMessage : Message
	{
		public TextMessage(EMessageType _type, string _text)
			: this(_type, _text, null)
		{
		}

		public EMessageType Type { get; protected set; }

		public Color Color { get; protected set; }

		public TextPortion Text { get; protected set; }

		protected TextMessage(EMessageType _type, string _text, Dictionary<string, Color> _highlights)
		{
			Type = _type;
			Text = new TextPortion(_text, _highlights);
		}
	}
}
