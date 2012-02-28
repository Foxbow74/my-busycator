using System;
using System.Drawing;
using GameCore;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameUi.UIBlocks.Map
{
	internal class MapUiBlock : UIBlock
	{
		public MapUiBlock(Rectangle _rectangle)
			: base(_rectangle, null, Color.Black.ToFColor())
		{
			World.TheWorld.LiveMap.SetViewPortSize(new Point(ContentRectangle.Width, ContentRectangle.Height));
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		public override void  Dispose()
		{
			MessageManager.NewWorldMessage -= MessageManagerNewWorldMessage;
			base.Dispose();
		}

		private void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
			switch (_message.Type)
			{
				case WorldMessage.EType.AVATAR_MOVE:
					BackgroundColor = new FColor(1f,0.6f,0.02f,0f);
					break;
				case WorldMessage.EType.TURN:
					Redraw();
					break;
			}
		}

		private void Redraw()
		{
			TileHelper.DrawHelper.ClearTiles(Rectangle, BackgroundColor);

			var fogColor = Color.FromArgb(255, 70, 70, 70).ToFColor();
			var fogLightness = fogColor.Lightness();// *2;

			var worldLayer = World.TheWorld.Avatar.Layer;
			var ambient = worldLayer.Ambient;

			var dPoint = World.TheWorld.LiveMap.GetData();

			var width = ContentRectangle.Width;
			var height = ContentRectangle.Height;

			for (var x = 0; x < width; ++x)
			{
				for (var y = 0; y < height; ++y)
				{
					var pnt = LiveMap.WrapCellCoords(new Point(x + dPoint.X, y + dPoint.Y));
					var liveCell = World.TheWorld.LiveMap.Cells[pnt.X, pnt.Y];

					var lighted = liveCell.Lighted.Screen(ambient).Multiply(liveCell.Visibility);
					var lightness = lighted.Lightness();
					if(lightness>0)
					{
						liveCell.SetIsSeenBefore();
					}
					if (lightness > fogLightness)
					{
						var tile = liveCell.Tile.GetTile() ?? liveCell.Terrain.Tile(liveCell.LiveCoords, liveCell.Rnd);
						var color = tile.Color.Multiply(lighted).Clamp();
						tile.Draw(x + ContentRectangle.Left, y + ContentRectangle.Top, color, BackgroundColor.Multiply(lighted));
					}
					else if (liveCell.IsSeenBefore)
					{
						var tile = liveCell.FoggedTile.GetTile()??liveCell.Terrain.Tile(liveCell.LiveCoords, liveCell.Rnd);
						tile.Draw(x + ContentRectangle.Left, y + ContentRectangle.Top, fogColor.Multiply(worldLayer.GetFogColorMultiplier(liveCell)), FColor.Empty);
						DrawHelper.FogTile(x + ContentRectangle.Left, y + ContentRectangle.Top);
					}
				}
			}
			World.TheWorld.Avatar.Tile.GetTile().Draw(width / 2 + ContentRectangle.Left, height / 2 + ContentRectangle.Top, Color.White.ToFColor(), BackgroundColor);
		}

		public override void DrawFrame()
		{
		}

		public override void DrawBackground()
		{
		}

		public override void DrawContent()
		{
		}
	}
}