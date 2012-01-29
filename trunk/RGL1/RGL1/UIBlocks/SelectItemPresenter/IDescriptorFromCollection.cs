using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.ThingPresenter
{
	internal interface IDescriptorFromCollection
	{
		void DrawLine(int _line, SpriteBatch _spriteBatch, SelectItemsUiBlock _selectItemsUiBlock);
	}
}