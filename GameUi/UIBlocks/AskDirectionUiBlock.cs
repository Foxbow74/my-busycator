using System;
using System.Drawing;
using GameCore;
using GameCore.Messages;



namespace GameUi.UIBlocks
{
	internal class AskDirectionUiBlock : UIBlock
	{
		private readonly AskDirectionMessage m_message;

		public AskDirectionUiBlock(Rectangle _rectangle, AskDirectionMessage _message)
			: base(new Rectangle(_rectangle.X, _rectangle.Y, _rectangle.Width, 1), null, Color.Gray, EFonts.COMMON)
		{
			m_message = _message;
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var dPoint = KeyTranslator.GetDirection(_key);
			if (dPoint != null)
			{
				m_message.Act.AddParameter(m_message.Point + dPoint);
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
			
			DrawLine("Выбери направление:", Color,0, 0, EAlignment.LEFT);
			
		}
	}
}