#region

using System;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;

#endregion

namespace GameUi.UIBlocks
{
	internal class ConfirmQuitBlock : UiBlockWithText
	{
		public ConfirmQuitBlock()
			: base(new Rct(2, 2, 15, 5), Frame.Frame1, FColor.Black)
		{
			ContentRct = new Rct(ContentRct.Left + 1,
			                     ContentRct.Top,
			                     ContentRct.Width - 1*2,
			                     ContentRct.Height);
		}

		public override void DrawContent() { DrawLine(new TextPortion.TextLine("Уверен? (д/н)", 0, null), FColor.White, 1, 0, EAlignment.CENTER); }

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