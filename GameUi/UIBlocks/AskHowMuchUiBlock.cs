using System;
using System.Drawing;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class AskHowMuchUiBlock : UiBlockWithText
	{
		private readonly AskHowMuchMessage m_message;


		private string m_count;

		public AskHowMuchUiBlock(Rct _rct, AskHowMuchMessage _message)
			: base(new Rct(_rct.Left, _rct.Top, _rct.Width, 1), null, FColor.Gray)
		{
			m_message = _message;
			m_count = _message.Total.ToString();
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			switch (_key)
			{
				case ConsoleKey.Backspace:
					m_count = m_count.Length > 0 ? (m_count.Substring(0, m_count.Length - 1)) : "";
					break;
				case ConsoleKey.NumPad0:
				case ConsoleKey.NumPad1:
				case ConsoleKey.NumPad2:
				case ConsoleKey.NumPad3:
				case ConsoleKey.NumPad4:
				case ConsoleKey.NumPad5:
				case ConsoleKey.NumPad6:
				case ConsoleKey.NumPad7:
				case ConsoleKey.NumPad8:
				case ConsoleKey.NumPad9:
				case ConsoleKey.D0:
				case ConsoleKey.D1:
				case ConsoleKey.D2:
				case ConsoleKey.D3:
				case ConsoleKey.D4:
				case ConsoleKey.D5:
				case ConsoleKey.D6:
				case ConsoleKey.D7:
				case ConsoleKey.D8:
				case ConsoleKey.D9:
					var name = Enum.GetName(typeof (ConsoleKey), _key);
					if (m_count.Length < m_message.Total.ToString().Length)
					{
						m_count += name.Substring(name.Length - 1, 1);
					}
					break;
				case ConsoleKey.Enter:
					var cnt = Math.Min(int.Parse(m_count), m_message.Total);
					m_message.Act.AddParameter(cnt);
					CloseTopBlock();
					break;
				case ConsoleKey.Escape:
					m_message.Act.AddParameter(0);
					CloseTopBlock();
					break;
			}
		}

		public override void DrawContent()
		{
			DrawLine(string.Format("{0}, количество ({1}): " + m_count, m_message.Descriptor.Thing.Name, m_message.Total), ForeColor,
			         0, 0, EAlignment.LEFT);
		}
	}
}