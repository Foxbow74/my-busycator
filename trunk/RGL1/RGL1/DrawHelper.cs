using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.UIBlocks;

namespace RGL1
{
	internal static class DrawHelper
	{
		public static void Fill(this UIBlock _uiBlock, SpriteBatch _spriteBatch, Tile _tile, Color _color)
		{
			if (_tile == null) return;

			var rectangle = _uiBlock.ContentRectangle;
			for (var i = rectangle.Left + 1; i < rectangle.Right; ++i)
			{
				for (var j = rectangle.Top + 1; j < rectangle.Bottom; ++j)
				{
					var destination = new Rectangle(i*Tile.Size, j*Tile.Size, Tile.Size, Tile.Size);
					_spriteBatch.Draw(_tile.GetTexture(), destination, _tile.Rectangle, _color);
				}
			}
		}

		public static void Clear(this Rectangle _rectangle, SpriteBatch _spriteBatch, Color _color)
		{
			var rect = new Rectangle(_rectangle.Left * Tile.Size, _rectangle.Top * Tile.Size, (_rectangle.Width) * Tile.Size, (_rectangle.Height) * Tile.Size);
			var srcRect = TileHelper.SolidTile.Rectangle;
			srcRect.Inflate(-1, -1);
			_spriteBatch.Draw(TileHelper.SolidTile.GetTexture(), rect, srcRect, _color);
		}

		public static void Clear(this UIBlock _frame, SpriteBatch _spriteBatch)
		{
			_frame.Rectangle.Clear(_spriteBatch, _frame.BackgroundColor);
		}

		public static void WriteString(this SpriteBatch _spriteBatch, string _string, int _x, int _y, Color _fore, Color _back,
		                               SpriteFont _spriteFont)
		{
			_spriteBatch.DrawString(_spriteFont, _string, new Vector2(_x, _y), _fore);
		}

		public static void FillRect(SpriteBatch _spriteBatch, Rectangle _rect, Color _color)
		{
			var srcRect = TileHelper.SolidTile.Rectangle;
			srcRect.Inflate(-1, -1);
			_spriteBatch.Draw(TileHelper.SolidTile.GetTexture(), _rect, srcRect, _color);
		}
	}
}