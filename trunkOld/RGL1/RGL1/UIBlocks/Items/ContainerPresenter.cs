using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.Items
{
	internal class ContainerPresenter : ILinePresenter
	{
		private readonly Container m_container;

		public ContainerPresenter(Container _container)
		{
			m_container = _container;
		}

		#region ILinePresenter Members

		public virtual void DrawLine(int _line, SpriteBatch _spriteBatch, UIBlock _uiBlock)
		{
			var cntnr = m_container == null ? "на земле" : m_container.Name;
			_uiBlock.DrawLine("*** " + cntnr.ToUpper() + " ***", Color.White, _spriteBatch, _line, 0, UIBlock.EAlignment.CENTER);
		}

		#endregion
	}
}