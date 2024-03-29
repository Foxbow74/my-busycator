﻿using System;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Messages;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class AskDirectionUiBlock : UiBlockWithText
	{
		private readonly AskMessage m_message;

		public AskDirectionUiBlock(Rct _rct, AskMessage _message)
			: base(new Rct(_rct.Left, _rct.Top, _rct.Width, 1), null, FColor.Gray) { m_message = _message; }

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var dPoint = KeyTranslator.GetDirection(_key);
			if (dPoint != null)
			{
				m_message.Act.AddParameter(dPoint);
				CloseTopBlock();
				return;
			}

			if (_key == ConsoleKey.Escape)
			{
				m_message.Act.IsCancelled = true;
				CloseTopBlock();
			}
		}

		public override void DrawContent() { DrawLine(EALConst.PLEASE_CHOOSE_DIRECTION.GetString(), ForeColor, 0, 0, EAlignment.LEFT); }
	}
}