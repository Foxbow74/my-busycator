using System.Collections.Generic;
using System.Drawing;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameUi.UIBlocks.Items
{
	internal class SelectToTakeOnUiBlock : ItemsSelectorUiBlock
	{
		private readonly EquipmentPresenter m_equipmentPresenter;
		private readonly EquipmentUiBlock m_equipmentUiBlock;

		public SelectToTakeOnUiBlock(Rectangle _rectangle, EquipmentUiBlock _equipmentUiBlock,
		                             EquipmentPresenter _equipmentPresenter) :
		                             	base(
		                             	_rectangle, ESelectItemDialogBehavior.SELECT_ONE, null,
		                             	_equipmentUiBlock.Intelligent.GetBackPackItems())
		{
			m_equipmentPresenter = _equipmentPresenter;
			m_equipmentUiBlock = _equipmentUiBlock;
		}

		protected override IEnumerable<EThingCategory> AllowedCategories
		{
			get { return EquipmentPlacesAttribute.GetAttribute(m_equipmentPresenter.Place).AbleToEquip; }
		}

		protected override int HeaderTakesLine
		{
			get { return 4; }
		}

		protected override void DrawHeader()
		{
			DrawLine("ВЫБЕРИ СНАРЯЖЕНИЕ", Color.White.ToFColor(), 0, 0, EAlignment.CENTER);
			DrawLine("ВЕС:", ForeColor, 2, 0, EAlignment.LEFT);
			DrawLine("ДОСТУПНЫЙ ВЕС:", ForeColor, 2, 0, EAlignment.RIGHT);
		}

		protected override void AddCheckedItemToResult(ThingDescriptor _thingDescriptor)
		{
			m_equipmentUiBlock.Intelligent.TakeOn(m_equipmentPresenter.Place, (Item) _thingDescriptor.Thing);
			m_equipmentUiBlock.Rebuild();
		}
	}
}