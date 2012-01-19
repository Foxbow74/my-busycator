namespace RGL1.Messages
{
	public static class MessageManager
	{
		public delegate void MessageDelegate(object _sender, Message _message);

		public static event MessageDelegate NewMessage;

		public static void SendMessage(object _sender, Message _message)
		{
			var mess = NewMessage;
			if(mess!=null)mess(_sender, _message);
		}
	}
}
