using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.UIBlocks;

namespace RGL1
{
	static class DrawHelper
	{
		public static void DrawAtPoint(this Tile _tile, SpriteBatch _spriteBatch, int _x, int _y)
		{
			if(_tile==null) return;

			var destination = new Rectangle(_x, _y, Tile.Size, Tile.Size);
			_spriteBatch.Draw(_tile.Texture, destination, _tile.Rectangle, _tile.Color);
		}

		public static void Fill(this UIBlock _frame, SpriteBatch _spriteBatch, Tile _tile, Color _color)
		{
			if (_tile == null) return;

			for (var i = _frame.Rectangle.Left + 1; i < _frame.Rectangle.Right; ++i)
			{
				for (var j = _frame.Rectangle.Top + 1; j < _frame.Rectangle.Bottom;++j )
				{
					var destination = new Rectangle(i*Tile.Size, j*Tile.Size, Tile.Size, Tile.Size);
					_spriteBatch.Draw(_tile.Texture, destination, _tile.Rectangle, _color);
				}
			}
		}

		public static void DrawAtCell(this Tile _tile, SpriteBatch _spriteBatch, int _col, int _row)
		{
			if (_tile==null) return;
			_tile.DrawAtPoint(_spriteBatch, _col * Tile.Size, _row * Tile.Size);
		}

		public static void WriteString(this SpriteBatch _spriteBatch, string _string, int _x, int _y, Color _fore, Color _back, SpriteFont _spriteFont)
		{
			_spriteBatch.DrawString(_spriteFont, _string, new Vector2(_x-1, _y-1), _back);
			_spriteBatch.DrawString(_spriteFont, _string, new Vector2(_x-1, _y), _back);
			_spriteBatch.DrawString(_spriteFont, _string, new Vector2(_x, _y-1), _back);

			_spriteBatch.DrawString(_spriteFont, _string, new Vector2(_x+1, _y+1), _back);
			_spriteBatch.DrawString(_spriteFont, _string, new Vector2(_x+1, _y), _back);
			_spriteBatch.DrawString(_spriteFont, _string, new Vector2(_x, _y+1), _back);
			
			_spriteBatch.DrawString(_spriteFont, _string, new Vector2(_x, _y), _fore);
		}

		public static void Draw(this Frame _frame, SpriteBatch _spriteBatch, int _col, int _row, int _width, int _height)
		{
			_frame.TopLeft.DrawAtCell(_spriteBatch, _col, _row);
			_frame.TopRight.DrawAtCell(_spriteBatch, _col + _width - 1, _row);
			_frame.BottomLeft.DrawAtCell(_spriteBatch, _col, _row + _height - 1);
			_frame.BottmoRight.DrawAtCell(_spriteBatch, _col + _width - 1, _row + _height - 1);

			for (int i = 1; i < _width; i++)
			{
				_frame.Top.DrawAtCell(_spriteBatch, _col + i, _row);
				_frame.Bottom.DrawAtCell(_spriteBatch, _col + i, _row + _height - 1);
			}
			for (int j = 1; j < _height; j++)
			{
				_frame.Top.DrawAtCell(_spriteBatch, _col , _row +j);
				_frame.Bottom.DrawAtCell(_spriteBatch, _col + _width - 1, _row + j);
			}
		}
	}
}
