using System.Collections.Generic;
using System.Drawing;
using GameCore.Acts;
using GameCore.Messages;
using GameCore.Objects;

namespace GameUi.UIBlocks.Items
{
	internal class SelectItemsUiBlock : ItemsSelectorUiBlock
	{
		public SelectItemsUiBlock(Rectangle _rectangle, IEnumerable<ThingDescriptor> _items, Act _act,
		                          ESelectItemDialogBehavior _behavior)
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

		protected override void DrawHeader()
		{
		}
	}
}