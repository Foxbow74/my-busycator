using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.Items
{
	class ThingCategoryPresenter : ILinePresenter
	{
		private readonly ThingCategoryAttribute m_attribute;

		public ThingCategoryPresenter(EThingCategory _category)
		{
			Category = _category;
			m_attribute = ThingCategoryAttribute.GetAttribute(_category);
		}

		public EThingCategory Category { get; private set; }

		public virtual void DrawLine(int _line, SpriteBatch _spriteBatch, UIBlock _uiBlock)
		{
			_uiBlock.DrawLine(m_attribute.DisplayName + "('" + m_attribute.C + "')", Color.Yellow, _spriteBatch, _line, 0, UIBlock.EAlignment.LEFT);
		}
	}
}