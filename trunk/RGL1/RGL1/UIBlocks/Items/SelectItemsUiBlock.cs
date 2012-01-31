using System.Collections.Generic;
using GameCore.Acts;
using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.Items
{
	internal class SelectItemsUiBlock : ItemsSelectorUiBlock
	{
		public SelectItemsUiBlock(Rectangle _rectangle, IEnumerable<ThingDescriptor> _items, Act _act)
			: base(_rectangle, EBehavior.SELECT_MULTIPLE | EBehavior.ALLOW_CHANGE_FILTER, _act, _items)
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