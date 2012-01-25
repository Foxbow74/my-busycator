namespace GameCore.Messages
{
	public static class MessageManager
	{
		public delegate void MessageDelegate(object _sender, Message _message);

		public static event MessageDelegate NewMessage;

		public delegate void WorldMessageDelegate(object _sender, WorldMessage _message);

		public static event WorldMessageDelegate NewWorldMessage;

		public static void SendMessage(object _sender, Message _message)
		{
			var mess = NewMessage;
			if (mess != null) mess(_sender, _message);
		}


		public static void SendMessage(object _sender, WorldMessage _message)
		{
			var mess = NewWorldMessage;
			if (mess != null) mess(_sender, _message);
		}
	}
}
