using System;
using System.Drawing;
using GameCore;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class AskDirectionUiBlock : UiBlockWithText
	{
		private readonly AskDirectionMessage m_message;

		public AskDirectionUiBlock(Rectangle _rectangle, AskDirectionMessage _message)
			: base(new Rectangle(_rectangle.X, _rectangle.Y, _rectangle.Width, 1), null, Color.Gray.ToFColor())
		{
			m_message = _message;
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var dPoint = KeyTranslator.GetDirection(_key);
			if (dPoint != null)
			{
				m_message.Act.AddParameter(LiveMap.WrapCellCoords(m_message.Point + dPoint));
				CloseTopBlock();
				return;
			}

			if (_key == ConsoleKey.Escape)
			{
				m_message.Act.IsCancelled = true;
				CloseTopBlock();
			}
		}

		public override void DrawContent()
		{
			DrawLine("Выбери направление:", ForeColor, 0, 0, EAlignment.LEFT);
		}
	}
}