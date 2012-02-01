using System.Collections.Generic;
using GameCore.Acts;
using GameCore.Messages;
using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.Items
{
	internal class SelectItemsUiBlock : ItemsSelectorUiBlock
	{
		public SelectItemsUiBlock(Rectangle _rectangle, IEnumerable<ThingDescriptor> _items, Act _act, ESelectItemDialogBehavior _behavior)
			: base(_rectangle, _behavior, _act, _items)
		{
		}

		protected override IEnumerable<EThingCategory> AllowedCategories
		{
			get { yield break; }
		}

		protected override int HeaderTakesLine
		{
			get { return 0; }
		}

		protected override void DrawHeader(SpriteBatch _spriteBatch)
		{
		}
	}
}