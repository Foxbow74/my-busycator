using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Messages;
using LanguagePack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class StoryTeller : AbstractGameTest2
	{
		[TestMethod]
		public void Битвище()
		{
			MessageManager.NewMessage += MessageManagerOnNewMessage;

			2.Repeat(() => SendKey(ConsoleKey.NumPad3));
			40.Repeat(() => SendKey(ConsoleKey.Y));
			10.Repeat(() => SendKey(ConsoleKey.NumPad3));
			100.Repeat(() =>
			           	{
							SendKey(ConsoleKey.NumPad1);
							SendKey(ConsoleKey.NumPad4);
							SendKey(ConsoleKey.NumPad7);
							SendKey(ConsoleKey.NumPad8);
							SendKey(ConsoleKey.NumPad9);
							SendKey(ConsoleKey.NumPad6);
							SendKey(ConsoleKey.NumPad3);
							SendKey(ConsoleKey.NumPad2);

							SendKey(ConsoleKey.T);
							SendKey(ConsoleKey.T);

							5.Repeat(() => SendKey(ConsoleKey.NumPad5));
			           	});

			
			//Debug.WriteLine(Avatar[0, 0].LiveMapBlock.Creatures.Count());
		}

		private readonly List<XLangMessage> m_messages = new List<XLangMessage>();

		private void MessageManagerOnNewMessage(object _sender, Message _message)
		{
			if (_message is XLangMessage)
			{
				m_messages.Add((XLangMessage)_message);
			}
			else
			{
				Debug.WriteLine(_message.ToString());
			}
		}

		protected override void AvatarBeginsTurn()
		{
			base.AvatarBeginsTurn();

			if (m_messages.Count > 0)
			{
				Debug.WriteLine(string.Join(", ", XMessageCompiler.Compile(m_messages)));
				m_messages.Clear();
			}
		}
	}
}