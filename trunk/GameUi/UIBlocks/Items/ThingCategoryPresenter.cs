using GameCore;
using GameCore.Objects;

namespace GameUi.UIBlocks.Items
{
	internal class ThingCategoryPresenter : ILinePresenter
	{
		private readonly ThingCategoryAttribute m_attribute;

		public ThingCategoryPresenter(EThingCategory _category)
		{
			Category = _category;
			m_attribute = ThingCategoryAttribute.GetAttribute(_category);
		}

		public EThingCategory Category { get; private set; }

		#region ILinePresenter Members

		public virtual void DrawLine(int _line, UiBlockWithText _uiBlock)
		{
			_uiBlock.DrawLine(m_attribute.DisplayName + "('" + m_attribute.C + "')",
			                  FColor.Yellow,
			                  _line,
			                  0,
			                  EAlignment.LEFT);
		}

		#endregion
	}
}