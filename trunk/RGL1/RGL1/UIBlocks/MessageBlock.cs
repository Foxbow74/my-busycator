using System;
using System.Collections.Generic;
using GameCore;
using GameCore.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.Messages;

namespace RGL1.UIBlocks
{
	class MessageBlock : UIBlock
	{
		private readonly List<TextPortion.TextLine> m_lines = new List<TextPortion.TextLine>();

		public MessageBlock(Rectangle _rectangle)
			: base(_rectangle, null, Color.Yellow)
		{
			MessageManager.NewMessage += MessageManagerNewMessage;
		}

		private void MessageManagerNewMessage(object _sender, Message _message)
		{
			if (_message is SimpleTextMessage)
			{
				var tm = (SimpleTextMessage) _message;
				var tp = new TextPortion(tm.Text, null);
				tp.SplitByLines((ContentRectangle.Width - 1)*Tile.Size, Fonts.Font, 0);
				m_lines.AddRange(tp.TextLines);
			}
			else if (_message is TextMessage)
			{
				var tm = (TextMessage) _message;
				tm.Text.SplitByLines((ContentRectangle.Width - 1)*Tile.Size, Fonts.Font, 0);
				m_lines.AddRange(tm.Text.TextLines);
			}
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			throw new NotImplementedException();
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			if(m_lines.Count==0) return;
			_spriteBatch.Begin();
			var lineNumber = 0;
			var max = TextLinesMax;
			//var fromLine = Math.Max(m_lines.Count - max,0);
			var fromLine = m_lines.Count - 1;
			for (var index = fromLine; index < m_lines.Count; index++)
			{
				var textLine = m_lines[index];
				if (lineNumber > TextLinesMax) break;
				DrawLine(textLine, Color, _spriteBatch, lineNumber++, 0, EAlignment.JUSTIFY);
			}
			_spriteBatch.End();
		}

		public int TextLinesMax
		{
			get { return (int)Math.Round((double)ContentRectangle.Height * Tile.Size / m_lineHeight); }

		}
	}
}