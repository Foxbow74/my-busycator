using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Essences;

namespace GameUi.UIBlocks.Items
{
	internal class EssenceCategoryPresenter : ILinePresenter
	{
		private readonly EssenceCategoryAttribute m_attribute;

		public EssenceCategoryPresenter(EItemCategory _category)
		{
			Category = _category;
			m_attribute = EssenceCategoryAttribute.GetAttribute(_category);
		}

		public EItemCategory Category { get; private set; }

		#region ILinePresenter Members

		public virtual void DrawLine(int _line, UiBlockWithText _uiBlock)
		{
			_uiBlock.DrawLine(EALSentence.GENERAL.GetString(Category.AsNoun()) + "('" + m_attribute.C + "')",
			                  FColor.Yellow,
			                  _line,
			                  0,
			                  EAlignment.LEFT);
		}

		#endregion
	}
}