namespace GameCore.Messages
{
	public class SimpleTextMessage : Message
	{
		public SimpleTextMessage(EMessageType _type, string _text)
		{
			Type = _type;
			Text = _text;
		}

		public EMessageType Type { get; protected set; }

		public string Text { get; protected set; }
	}
}