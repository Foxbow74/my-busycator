using System.Collections.Generic;
using Graphics;
using Microsoft.Xna.Framework;

namespace Common.Messages
{
	public class TextMessage:Message
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

	public class WorldMessage
	{
		public static WorldMessage Turn = new WorldMessage(EType.TURN);
		public static WorldMessage AvatarTurn = new WorldMessage(EType.AVATAR_TURN);
		public static WorldMessage AvatarMove = new WorldMessage(EType.AVATAR_MOVE);

		public enum EType
		{
			TURN,
			AVATAR_TURN,
			AVATAR_MOVE,
		}

		public EType Type { get; private set; }

		public WorldMessage(EType _type)
		{
			Type = _type;
		}
	}
}