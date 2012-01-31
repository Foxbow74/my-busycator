using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.Items
{
	internal interface ILinePresenter
	{
		void DrawLine(int _line, SpriteBatch _spriteBatch, UIBlock _uiBlock);
	}
}