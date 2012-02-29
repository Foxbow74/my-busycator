using System.Collections.Generic;
using GameCore.Acts;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameUi.UIBlocks.Items
{
	internal class SelectItemsUiBlock : ItemsSelectorUiBlock
	{
		public SelectItemsUiBlock(Rct _rct, IEnumerable<ThingDescriptor> _items, Act _act,
		                          ESelectItemDialogBehavior _behavior)
			: base(_rct, _behavior, _act, _items)
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