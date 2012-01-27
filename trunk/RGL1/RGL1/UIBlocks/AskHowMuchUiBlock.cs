using System;
using GameCore;
using GameCore.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks
{
	class AskHowMuchUiBlock : UIBlock
	{
		private readonly AskHowMuchMessage m_message;

		public AskHowMuchUiBlock(Rectangle _rectangle, AskHowMuchMessage _message)
			: base(new Rectangle(_rectangle.X, _rectangle.Y, _rectangle.Width, 1), null, Color.Gray, Fonts.Font)
		{
			m_message = _message;
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			throw new NotImplementedException();
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();
			DrawLine("Сколько?", Color, _spriteBatch, 0, 0, EAlignment.LEFT);
			_spriteBatch.End();
		}
	}
}
