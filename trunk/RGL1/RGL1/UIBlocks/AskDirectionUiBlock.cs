using System;
using GameCore;
using GameCore.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks
{
	class AskDirectionUiBlock: UIBlock
	{
		private readonly AskDirectionMessage m_message;

		public AskDirectionUiBlock(Rectangle _rectangle, AskDirectionMessage _message)
			: base(new Rectangle(_rectangle.X, _rectangle.Y, _rectangle.Width, 1), null, Color.Gray, Fonts.Font)
		{
			m_message = _message;
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var dPoint = KeyHelper.GetDirection(_key);
			if(dPoint!=null)
			{
				m_message.Act.AddParameter(m_message.Point + dPoint);
				CloseTopBlock();
				return;
			}

			if(_key==ConsoleKey.Escape)
			{
				m_message.Act.IsCancelled = true;
				CloseTopBlock();
			}
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();
			DrawLine("Выбери направление:", Color, _spriteBatch, 0, 0, EAlignment.LEFT);
			_spriteBatch.End();
		}
	}
}
