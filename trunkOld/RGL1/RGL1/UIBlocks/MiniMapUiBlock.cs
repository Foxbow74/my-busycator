using System;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks
{
	class MiniMapUiBlock:UIBlock
	{
		public MiniMapUiBlock(Rectangle _rectangle) : base(_rectangle, Frame.GoldFrame, Color.White, Fonts.SmallFont)
		{
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			switch (_key)
			{
				case ConsoleKey.Escape:
				case ConsoleKey.Z:
					CloseTopBlock();
					break;
			}
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			var map = World.TheWorld.Map.WorldMap;
			var size = new Vector2(map.GetLength(0), map.GetLength(1));

			var halfSize = new Vector2(size.X/2, size.Y/2);

			var rsz = Math.Min(ContentRectangle.Width * Tile.Size / size.X, ContentRectangle.Height * Tile.Size / size.Y);
			var rectSize = new Vector2(rsz, rsz);
			var halfContentRect = new Vector2(ContentRectangle.Left*Tile.Size + ContentRectangle.Width*Tile.Size/2, ContentRectangle.Top*Tile.Size + ContentRectangle.Height*Tile.Size/2);

			_spriteBatch.Begin();
			for (var i = 0; i < size.X; ++i)
			{
				for(var j=0;j<size.Y;++j)
				{
					var type = map[i, j];
					var pnt = new Vector2(i-halfSize.X, j-halfSize.Y);
					Color color;
					switch (type)
					{
						case EMapBlockTypes.NONE:
							color = Color.Black;
							break;
						case EMapBlockTypes.GROUND:
							color = Color.Brown;
							break;
						case EMapBlockTypes.SEA:
							color = Color.Blue;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					var rect = new Rectangle(
						(int)Math.Round(halfContentRect.X + pnt.X * rectSize.X), 
						(int)Math.Round(halfContentRect.Y + pnt.Y * rectSize.Y),
						(int)Math.Round(rectSize.X), 
						(int)Math.Round(rectSize.Y));
					DrawHelper.FillRect(_spriteBatch, rect, color);
				}
			}
			_spriteBatch.End();
		}
	}
}
