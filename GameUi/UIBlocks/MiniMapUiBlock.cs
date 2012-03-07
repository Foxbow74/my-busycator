using System;
using GameCore;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class MiniMapUiBlock : UiBlockWithText
	{
		public MiniMapUiBlock(Rct _rct)
			: base(_rct, Frame.GoldFrame, FColor.White)
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
			var ground = new FColor(1f, 0.3f, 0.2f, 0f);
			var sea = new FColor(1f, 0.0f, 0.0f, 0.3f);
			var city = new FColor(1f, 0.5f, 0.4f, 0.2f);

			var size = new Point(World.TheWorld.Surface.WorldMapSize, World.TheWorld.Surface.WorldMapSize);

			var halfSize = size/2;

			var rsz = Math.Min(ContentRct.Width*ATile.Size/size.X, ContentRct.Height*ATile.Size/size.Y);
			var rectSize = new Point(rsz, rsz);
			var halfContentRect = new Point(ContentRct.Left*ATile.Size + ContentRct.Width*ATile.Size/2,
			                                  ContentRct.Top*ATile.Size + ContentRct.Height*ATile.Size/2);

			for (var i = 0; i < size.X; ++i)
			{
				for (var j = 0; j < size.Y; ++j)
				{
					var pnt = new Point(i, j) - halfSize;
					var type = World.TheWorld.Surface.GetBlockType(pnt);
					FColor color;
					switch (type)
					{
						case EMapBlockTypes.NONE:
							color = FColor.Black;
							break;
						case EMapBlockTypes.GROUND:
							color = ground;
							break;
						case EMapBlockTypes.SEA:
							color = sea;
							break;
						case EMapBlockTypes.CITY:
							color = city;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					if (World.TheWorld.AvatarBlockId == pnt)
					{
						color = FColor.White;
					}

					var rect = new Rct(
						halfContentRect +pnt*rectSize,
						rectSize.X,
						rectSize.Y);
					DrawHelper.DrawRect(rect, color);
				}
			}
			DrawLine("[z|Esc] - выход", ForeColor, TextLinesMax - 2, 21, EAlignment.RIGHT);
		}
	}
}