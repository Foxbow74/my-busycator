using System;
using System.Drawing;
using GameCore;



namespace GameUi.UIBlocks
{
	class MiniMapUiBlock:UIBlock
	{
		public MiniMapUiBlock(Rectangle _rectangle) : base(_rectangle, Frame.GoldFrame, Color.White, EFonts.SMALL)
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
			var size = new Point(map.GetLength(0), map.GetLength(1));
			for (var i = 0; i < size.X; ++i)
			{
				for(var j=0;j<size.Y;++j)
				{
					var type = map[i, j];
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
					TileHelper.DrawHelper.Clear(Rectangle, color);
				}
			}
			
		}
	}
}
