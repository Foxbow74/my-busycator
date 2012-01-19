using System.Collections.Generic;
using Graphics;
using Microsoft.Xna.Framework;
using RGL1.Messages;

namespace RGL1.UIBlocks
{
	class MessageBlock:UIBlock
	{
		private readonly List<Message> m_history = new List<Message>();

		public MessageBlock(Rectangle _rectangle): base(_rectangle, Frame.SimpleFrame, Color.Yellow)
		{
			MessageManager.NewMessage += MessageManagerNewMessage;
		}

		void MessageManagerNewMessage(object _sender, Message _message)
		{
			m_history.Add(_message);
		}
	}
}
