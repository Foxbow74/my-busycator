using System;
using System.Drawing;
using GameCore;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class MiniMapUiBlock : UiBlockWithText
	{
		public MiniMapUiBlock(Rct _rct)
			: base(_rct, Frame.GoldFrame, Color.White.ToFColor())
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
			var map = World.TheWorld.Surface.WorldMap;
			var size = new Vector2(map.GetLength(0), map.GetLength(1));

			var halfSize = new Vector2(size.X/2, size.Y/2);

			var rsz = Math.Min(ContentRct.Width*ATile.Size/size.X, ContentRct.Height*ATile.Size/size.Y);
			var rectSize = new Vector2(rsz, rsz);
			var halfContentRect = new Vector2(ContentRct.Left*ATile.Size + ContentRct.Width*ATile.Size/2,
			                                  ContentRct.Top*ATile.Size + ContentRct.Height*ATile.Size/2);

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
							color = System.Drawing.Color.Black;
							break;
						case EMapBlockTypes.GROUND:
							color = System.Drawing.Color.Brown;
							break;
						case EMapBlockTypes.SEA:
							color = System.Drawing.Color.Blue;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					var rect = new Rct(
						(int) Math.Round(halfContentRect.X + pnt.X*rectSize.X),
						(int) Math.Round(halfContentRect.Y + pnt.Y*rectSize.Y),
						(int) Math.Round(rectSize.X),
						(int) Math.Round(rectSize.Y));
					DrawHelper.ClearText(rect, color.ToFColor());
				}

				DrawLine("[z|Esc] - выход", ForeColor, TextLinesMax - 1, 21, EAlignment.RIGHT);
			}
		}
	}
}