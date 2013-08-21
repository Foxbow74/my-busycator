using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using LanguagePack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class HelloWorld2 : AbstractGameTest2
	{
		[TestMethod]
		public void ЗапускайтеКабанчика()
		{
			using (new Profiler())
			{
				for (int i = 0; i < 10; i++)
				{
					SendKey(ConsoleKey.Y);
				}
			}
		}
	}

	[TestClass]
	public class HelloWorld100 : AbstractGameTest100
	{
		[TestMethod]
		public void ВечныйБег()
		{
			MessageManager.NewMessage += MessageManagerOnNewMessage;

			using (new Profiler())
			{
				for (int i = 0; i < 10000; i++)
				{
					switch (World.Rnd.Next(8))
					{
						case 0:
							SendKey(ConsoleKey.NumPad4);
							break;
						case 1:
							SendKey(ConsoleKey.NumPad8);
							break;
						case 2:
							SendKey(ConsoleKey.NumPad2);
							break;
						case 3:
							SendKey(ConsoleKey.NumPad6);
							break;
						case 4:
							SendKey(ConsoleKey.NumPad1);
							break;
						case 5:
							SendKey(ConsoleKey.NumPad3);
							break;
						case 6:
							SendKey(ConsoleKey.NumPad4);
							break;
						case 7:
							SendKey(ConsoleKey.NumPad9);
							break;
					}
				}
			}
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