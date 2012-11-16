using GameCore;
using GameCore.Essences;

namespace GameUi.UIBlocks.Items
{
	internal class EssenceCategoryPresenter : ILinePresenter
	{
		private readonly EssenceCategoryAttribute m_attribute;

		public EssenceCategoryPresenter(EEssenceCategory _category)
		{
			Category = _category;
			m_attribute = EssenceCategoryAttribute.GetAttribute(_category);
		}

		public EEssenceCategory Category { get; private set; }

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