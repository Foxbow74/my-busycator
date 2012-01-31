using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.Items
{
	internal class BackpackUiBlock:ItemsSelectorUiBlock
	{
		public BackpackUiBlock(Rectangle _rectangle): base(_rectangle, EBehavior.ALLOW_CHANGE_FILTER, null, World.TheWorld.Avatar.GetBackPackItems().OrderBy(_thingDescriptor => _thingDescriptor.Thing.Name))
		{
		}

		protected override IEnumerable<EThingCategory> AllowedCategories
		{
			get { yield break; }
		}

		protected override int HeaderTakesLine
		{
			get { return 4; }
		}

		protected override void DrawHeader(SpriteBatch _spriteBatch)
		{
			DrawLine("СОДЕРЖИМОЕ РЮКЗАКА", Color.White, _spriteBatch, 0, 0, EAlignment.CENTER);
			DrawLine("ВЕС:", Color, _spriteBatch, 2, 0, EAlignment.LEFT);
			DrawLine("ДОСТУПНЫЙ ВЕС:", Color, _spriteBatch, 2, 0, EAlignment.RIGHT);
		}
	}
}