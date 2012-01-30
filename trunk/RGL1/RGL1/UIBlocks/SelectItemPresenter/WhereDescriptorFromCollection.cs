using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.UIBlocks.ThingPresenter;

namespace RGL1.UIBlocks.SelectItemPresenter
{
	internal class WhereDescriptorFromCollection : IDescriptorFromCollection
	{
		private readonly Container m_container;

		public WhereDescriptorFromCollection(Container _container)
		{
			m_container = _container;
		}

		public virtual void DrawLine(int _line, SpriteBatch _spriteBatch, SelectItemsUiBlock _selectItemsUiBlock)
		{
			var cntnr = m_container == null ? "на земле" : m_container.Name;
			_selectItemsUiBlock.DrawLine("*** " + cntnr.ToUpper() + " ***", Color.White, _spriteBatch, _line, 0, UIBlock.EAlignment.CENTER);
		}
	}
}