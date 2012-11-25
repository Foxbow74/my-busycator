using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class TurnMessageUiBlock : UiBlockWithText
	{
		private readonly List<TextPortion.TextLine> m_lines = new List<TextPortion.TextLine>();
		private readonly List<Message> m_turnMessages = new List<Message>();
		private int m_linesShown;
		private int m_visibleTill;

		public TurnMessageUiBlock(Rct _rct)
			: base(_rct, null, FColor.Yellow)
		{
			MessageManager.NewMessage += MessageManagerNewMessage;
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		public override void Dispose()
		{
			MessageManager.NewMessage -= MessageManagerNewMessage;
			MessageManager.NewWorldMessage -= MessageManagerNewWorldMessage;
			base.Dispose();
		}

		private void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
			if (_message.Type == WorldMessage.EType.AVATAR_BEGINS_TURN)
			{
				m_turnMessages.Clear();
				m_lines.Clear();
				m_linesShown = 0;
				m_visibleTill = 0;
			}
		}

		private void MessageManagerNewMessage(object _sender, Message _message)
		{
			Debug.WriteLine(_message.ToString());
			m_turnMessages.Add(_message);
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers) { throw new NotImplementedException(); }

		public override void DrawContent()
		{
			var strings = new List<string>();
			foreach (var message in m_turnMessages)
			{
				if (message is SimpleTextMessage)
				{
					var tm = (SimpleTextMessage) message;
					strings.Add(tm.Text);
				}
				else if (message is SoundTextMessage)
				{
					var tm = (SoundTextMessage)message;
					strings.Add("где-то " + tm.Text);
				}
			}

			var str = string.Join(", ", strings);
			var tp = new TextPortion(str, null);
			tp.SplitByLines((ContentRct.Width - 2)*Constants.TILE_SIZE, Font, 0, DrawHelper);

			var lines = tp.TextLines.ToArray();
			if (lines.Length == 0) return;

			var lineNumber = 0;
			var max = Math.Min(TextLinesMax, lines.Length);
			var fromLine = m_linesShown;
			for (var index = fromLine; index < max; index++)
			{
				var textLine = lines[index];
				if (lineNumber > TextLinesMax) break;
				DrawLine(textLine, ForeColor, lineNumber++, 0, EAlignment.JUSTIFY);
			}
			m_visibleTill = max;
		}
	}
}