using System;
using System.Collections.Generic;
using System.Drawing;
using GameCore;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameUi.UIBlocks
{
	public class MiniMapUiBlock : UiBlockWithText
	{
		static readonly Dictionary<EMapBlockTypes, FColor> m_colors = new Dictionary<EMapBlockTypes, FColor>();

		static MiniMapUiBlock()
		{
			foreach (EMapBlockTypes type in Enum.GetValues(typeof(EMapBlockTypes)))
			{
				FColor color;
				switch (type)
				{
					case EMapBlockTypes.NONE:
						color = FColor.Empty;
						break;
					case EMapBlockTypes.GROUND:
						color = new FColor(1f, 0.1f, 0.5f, 0.2f);
						break;
					case EMapBlockTypes.FOREST:
						color = new FColor(1f, 0.0f, 0.3f, 0.0f);
						break;
					case EMapBlockTypes.SEA:
						color = new FColor(1f, 0.0f, 0.2f, 0.5f);
						break;
					case EMapBlockTypes.DEEP_SEA:
						color = new FColor(1f, 0.0f, 0.1f, 0.3f);
						break;
					case EMapBlockTypes.FRESH_WATER:
						color = new FColor(1f, 0.3f, 0.4f, 0.7f);
						break;
					case EMapBlockTypes.DEEP_FRESH_WATER:
						color = new FColor(1f, 0.1f, 0.2f, 0.5f);
						break;
					case EMapBlockTypes.CITY:
						color = FColor.Black;
						//color = new FColor(1f, 0.5f, 0.4f, 0.2f);
						break;
					case EMapBlockTypes.COAST:
						color = FColor.SandyBeach;
						break;
					case EMapBlockTypes.LAKE_COAST:
						color = FColor.SandyBrown;
						break;
					case EMapBlockTypes.MOUNT:
						color = FColor.DarkGray;
						break;
					case EMapBlockTypes.SWAMP:
						color = new FColor(1f, 0.2f, 0.3f, 0.1f);
						break;
					case EMapBlockTypes.ETERNAL_SNOW:
						color = FColor.White;
						break;
					case EMapBlockTypes.SHRUBS:
						color = new FColor(1f, 0.0f, 0.4f, 0.1f);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				m_colors[type] = color;
			}
		}

		public MiniMapUiBlock(Rct _rct)
			: base(_rct, Frame.Frame2, FColor.White) { }

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
			var size = new Point(Constants.WORLD_MAP_SIZE, Constants.WORLD_MAP_SIZE);

			var halfSize = size/2;

			var rsz = Math.Min((float)ContentRct.Width * Constants.TILE_SIZE / size.X, (float)ContentRct.Height * Constants.TILE_SIZE / size.Y);
			var rectSize = new PointF(rsz, rsz);
			var halfContentRect = new Point(ContentRct.Left*Constants.TILE_SIZE + ContentRct.Width*Constants.TILE_SIZE/2,
			                                ContentRct.Top*Constants.TILE_SIZE + ContentRct.Height*Constants.TILE_SIZE/2);

			for (var i = 0; i < size.X; ++i)
			{
				for (var j = 0; j < size.Y; ++j)
				{
					var pnt = new Point(i, j) - halfSize;
					var type = World.TheWorld.Surface.GetBlockType(pnt);
                    
                    //if(World.TheWorld.Surface[pnt].SeenCells.All(_u => _u==0)) continue;
					
                    var color = GetColor(type);

					if (World.TheWorld.Avatar[0, 0].MapBlockId == pnt)
					{
						color = FColor.Crimson;
					}

					DrawHelper.DrawRect(new RectangleF(halfContentRect.X + pnt.X * rectSize.X, halfContentRect.Y + pnt.Y * rectSize.Y, rectSize.X, rectSize.Y), color);
				}
			}
			DrawLine("[z|Esc] - выход", ForeColor, TextLinesMax - 2, 21, EAlignment.RIGHT);
		}


		public static FColor GetColor(EMapBlockTypes _type)
		{
			return m_colors[_type];
		}
	}
}