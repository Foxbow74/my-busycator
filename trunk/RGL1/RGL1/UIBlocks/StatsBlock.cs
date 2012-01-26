#region

using System;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace RGL1.UIBlocks
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

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
		}
	}
}