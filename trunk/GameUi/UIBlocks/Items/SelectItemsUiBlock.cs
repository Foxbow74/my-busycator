using System.Collections.Generic;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameUi.UIBlocks.Items
{
	internal class SelectItemsUiBlock : ItemsSelectorUiBlock
	{
		public SelectItemsUiBlock(Rct _rct, AskMessage _message)
			: base(_rct, _message.GetFirstParameter<ESelectItemDialogBehavior>(), _message.Act, _message.GetParameters<ThingDescriptor>()) { }

		protected override IEnumerable<EThingCategory> AllowedCategories { get { yield break; } }

		protected override int HeaderTakesLine { get { return 0; } }

		protected override void DrawHeader() { }
	}
}