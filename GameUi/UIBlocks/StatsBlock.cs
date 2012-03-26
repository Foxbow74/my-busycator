using System;
using GameCore;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class StatsBlock : UIBlock
	{
		public StatsBlock(Rct _rct)
			: base(_rct, null, FColor.Gray) { }

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers) { throw new NotImplementedException(); }

		public override void DrawContent() { }
	}
}