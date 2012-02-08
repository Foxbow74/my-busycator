using System;
using System.Drawing;
using GameCore;
using GameCore.Misc;


namespace GameUi.UIBlocks
{
	class MiniMapUiBlock:UIBlock
	{
		public MiniMapUiBlock(Rectangle _rectangle) : base(_rectangle, Frame.GoldFrame, Color.White, EFonts.COMMON)
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

		public override void DrawContent()
		{
			var map = World.TheWorld.Map.WorldMap;
			var size = new Vector2(map.GetLength(0), map.GetLength(1));

			var halfSize = new Vector2(size.X / 2, size.Y / 2);

			var rsz = Math.Min(ContentRectangle.Width * ATile.Size / size.X, ContentRectangle.Height * ATile.Size / size.Y);
			var rectSize = new Vector2(rsz, rsz);
			var halfContentRect = new Vector2(ContentRectangle.Left * ATile.Size + ContentRectangle.Width * ATile.Size / 2, ContentRectangle.Top * ATile.Size + ContentRectangle.Height * ATile.Size / 2);

			for (var i = 0; i < size.X; ++i)
			{
				for (var j = 0; j < size.Y; ++j)
				{
					var type = map[i, j];
					var pnt = new Vector2(i - halfSize.X, j - halfSize.Y);
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
					DrawHelper.Clear(rect, color);
				}

				DrawLine("[z|Esc] - выход", Color, TextLinesMax - 1, 21, EAlignment.RIGHT);
			}
			
		}
	}
}
