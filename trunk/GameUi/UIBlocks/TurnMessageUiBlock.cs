using System;
using System.Collections.Generic;
using System.Drawing;
using GameCore;
using GameCore.Messages;
using GameUi.Messages;

namespace GameUi.UIBlocks
{
	internal class TurnMessageUiBlock : UIBlock
	{
		private readonly List<TextPortion.TextLine> m_lines = new List<TextPortion.TextLine>();
		private int m_linesShown;
		private int m_visibleTill;

		public TurnMessageUiBlock(Rectangle _rectangle)
			: base(_rectangle, null, Color.Yellow)
		{
			MessageManager.NewMessage += MessageManagerNewMessage;
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		private void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
			if (_message.Type == WorldMessage.EType.AVATAR_TURN)
			{
				m_lines.Clear();
				m_linesShown = 0;
				m_visibleTill = 0;
			}
		}

		private void MessageManagerNewMessage(object _sender, Message _message)
		{
			if (_message is SimpleTextMessage)
			{
				var tm = (SimpleTextMessage) _message;
				var tp = new TextPortion(tm.Text, null);
				tp.SplitByLines((ContentRectangle.Width - 1)*ATile.Size, Font, 0);
				m_lines.AddRange(tp.TextLines);
			}
			else if (_message is TextMessage)
			{
				var tm = (TextMessage) _message;
				tm.Text.SplitByLines((ContentRectangle.Width - 1)*ATile.Size, Font, 0);
				m_lines.AddRange(tm.Text.TextLines);
			}
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			throw new NotImplementedException();
		}

		public override void DrawContent()
		{
			if (m_lines.Count == 0) return;

			var lineNumber = 0;
			var max = Math.Min(TextLinesMax, m_lines.Count);
			var fromLine = m_linesShown;
			for (var index = fromLine; index < max; index++)
			{
				var textLine = m_lines[index];
				if (lineNumber > TextLinesMax) break;
				DrawLine(textLine, Color, lineNumber++, 0, EAlignment.JUSTIFY);
			}
			m_visibleTill = max;
		}
	}
}