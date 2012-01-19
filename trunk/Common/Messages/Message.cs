using System.Collections.Generic;
using Graphics;
using Microsoft.Xna.Framework;

namespace RGL1.Messages
{
	public enum EMessageType
	{
		DEBUG,
		INFO,
		WARNING,
		ERROR,
		EPIC,
		SYSTEM,
	}

	public abstract class Message
	{
		public EMessageType Type { get; protected set; }

		public Color Color { get; protected set; }

		public TextPortion Text { get; protected set; }

		protected Message(EMessageType _type, string _text, Dictionary<string, Color> _highlights)
		{
			Type = _type;
			Text = new TextPortion(_text, _highlights);
		}
	}
}