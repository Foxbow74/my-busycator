using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Essences;

namespace GameUi.UIBlocks.Items
{
	internal class BackpackUiBlock : ItemsSelectorUiBlock
	{
		private readonly IEnumerable<EItemCategory> m_allowedCategories;

		public BackpackUiBlock(Rct _rct)
			: base(
				_rct,
				ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER,
				null,
				World.TheWorld.Avatar.GetBackPackItems().OrderBy(_thingDescriptor => _thingDescriptor.UiOrderIndex)) { m_allowedCategories = new EItemCategory[0]; }

		public BackpackUiBlock(Rct _rct, AskMessage _message)
			: base(_rct, _message.GetFirstParameter<ESelectItemDialogBehavior>(), _message.Act, World.TheWorld.Avatar.GetBackPackItems().OrderBy(_thingDescriptor => _thingDescriptor.UiOrderIndex))

		{
			var category = _message.GetParameters<EItemCategory>();
			m_allowedCategories = category ?? new EItemCategory[0];
		}

		protected override IEnumerable<EItemCategory> AllowedCategories { get { return m_allowedCategories; } }

		protected override int HeaderTakesLine { get { return 4; } }

		protected override void DrawHeader()
		{
			DrawLine("СОДЕРЖИМОЕ РЮКЗАКА", FColor.White, 0, 0, EAlignment.CENTER);
			DrawLine("ВЕС:", ForeColor, 2, 0, EAlignment.LEFT);
			DrawLine("ДОСТУПНЫЙ ВЕС:", ForeColor, 2, 0, EAlignment.RIGHT);
		}
	}
}