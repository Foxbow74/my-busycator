﻿#region

using System;
using System.Drawing;
using GameCore;

#endregion

namespace GameUi.UIBlocks
{
	internal class StatsBlock : UIBlock
	{
		public StatsBlock(Rectangle _rectangle) : base(_rectangle, null, Color.Gray)
		{
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			throw new NotImplementedException();
		}

		public override void DrawContent()
		{
		}
	}
}