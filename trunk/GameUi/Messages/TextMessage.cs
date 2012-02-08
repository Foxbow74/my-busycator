using System.Collections.Generic;
using System.Drawing;
using GameCore.Messages;

namespace GameUi.Messages
{
	public class TextMessage : Message
	{
		public TextMessage(EMessageType _type, string _text)
			: this(_type, _text, null)
		{
		}

		protected TextMessage(EMessageType _type, string _text, Dictionary<string, Color> _highlights)
		{
			Type = _type;
			Text = new TextPortion(_text, _highlights);
		}

		public EMessageType Type { get; protected set; }

		public Color Color { get; protected set; }

		public TextPortion Text { get; protected set; }
	}
}