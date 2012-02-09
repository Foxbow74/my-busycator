#region

using System;
using System.Drawing;
using GameCore;
using GameCore.Messages;

#endregion

namespace GameUi.UIBlocks
{
	internal class ConfirmQuitBlock : UIBlock
	{
		public ConfirmQuitBlock() : base(new Rectangle(2, 2, 15, 5), Frame.SimpleFrame, Color.Black)
		{
			ContentRectangle = new Rectangle(ContentRectangle.Left + 1, ContentRectangle.Top, ContentRectangle.Width - 1*2,
			                                 ContentRectangle.Height);
		}

		public override void DrawContent()
		{
			DrawLine(new TextPortion.TextLine("Уверен? (д/н)", 0, null), Color.White, 1, 0, EAlignment.CENTER);
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if (_modifiers != EKeyModifiers.NONE) return;
			if (_key == ConsoleKey.Y || _key == ConsoleKey.L)
			{
				MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.EXIT_GAME));
			}
			if (_key == ConsoleKey.N || _key == ConsoleKey.Y || _key == ConsoleKey.Escape)
			{
				MessageManager.SendMessage(this, new SystemMessage(SystemMessage.ESystemMessage.CLOSE_TOP_UI_BLOCK));
			}
		}
	}
}