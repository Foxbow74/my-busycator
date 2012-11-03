using System;
using System.Collections.Generic;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class MessageBlock : UiBlockWithText
	{
		private readonly List<TextPortion.TextLine> m_lines = new List<TextPortion.TextLine>();

		public MessageBlock(Rct _rct)
			: base(_rct, null, FColor.Yellow) { MessageManager.NewMessage += MessageManagerNewMessage; }

		public override void Dispose()
		{
			MessageManager.NewMessage -= MessageManagerNewMessage;
			base.Dispose();
		}

		private void MessageManagerNewMessage(object _sender, Message _message)
		{
			if (!(_message is SimpleTextMessage))
			{
				throw new NotImplementedException();
			}
			var tm = (SimpleTextMessage) _message;
			var tp = new TextPortion(tm.Text, null);
			tp.SplitByLines((ContentRct.Width - 1)*Constants.TILE_SIZE, Font, 0);
			m_lines.AddRange(tp.TextLines);
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers) { throw new NotImplementedException(); }

		public override void DrawContent()
		{
			if (m_lines.Count == 0) return;

			var lineNumber = 0;
			var max = TextLinesMax;
			//var fromLine = Math.Max(m_lines.Count - max,0);
			var fromLine = m_lines.Count - 1;
			for (var index = fromLine; index < m_lines.Count; index++)
			{
				var textLine = m_lines[index];
				if (lineNumber > TextLinesMax) break;
				DrawLine(textLine, ForeColor, lineNumber++, 0, EAlignment.JUSTIFY);
			}
		}
	}
}