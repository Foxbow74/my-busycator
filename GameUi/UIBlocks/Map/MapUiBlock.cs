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
		public MapUiBlock(Rct _rct)
			: base(_rct, null, Color.Black.ToFColor())
		{
			World.TheWorld.LiveMap.SetViewPortSize(new Point(ContentRct.Width, ContentRct.Height));
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
				case WorldMessage.EType.TURN:
					Redraw();
					break;
			}
		}

		private void Redraw()
		{
			TileHelper.DrawHelper.ClearTiles(Rct, BackgroundColor);

			var fogColor = Color.FromArgb(255, 70, 70, 70).ToFColor();
			var fogLightness = fogColor.Lightness();// *2;

			var worldLayer = World.TheWorld.Avatar.Layer;
			var ambient = worldLayer.Ambient;

			var dPoint = World.TheWorld.LiveMap.GetData();

			var width = ContentRct.Width;
			var height = ContentRct.Height;

			for (var x = 0; x < width; ++x)
			{
				for (var y = 0; y < height; ++y)
				{
					var pnt = LiveMap.WrapCellCoords(new Point(x + dPoint.X, y + dPoint.Y));
					var liveCell = World.TheWorld.LiveMap.Cells[pnt.X, pnt.Y];

					var lighted = liveCell.Lighted.Screen(ambient).Multiply(liveCell.Visibility);
					var lightness = lighted.Lightness();
					if(lightness>0.1f)
					{
						liveCell.SetIsSeenBefore();
					}


					
					if (lightness > fogLightness)
					{
						var terrainTile = liveCell.Terrain.Tile(liveCell.LiveCoords, liveCell.Rnd);
						terrainTile.Draw(x + ContentRct.Left, y + ContentRct.Top, terrainTile.Color.Multiply(lighted).Clamp(), BackgroundColor.Multiply(lighted));

						foreach (var tileInfoProvider in liveCell.TileInfoProviders)
						{
							var tile = tileInfoProvider.Tile.GetTile();
							var color = tile.Color.LerpColorsOnly(tileInfoProvider.LerpColor, tileInfoProvider.LerpColor.A).Multiply(lighted).Clamp();
							tile.Draw(x + ContentRct.Left, y + ContentRct.Top, color, BackgroundColor.Multiply(lighted));
						}
					}
					else if (liveCell.IsSeenBefore)
					{
						var terrainTile = liveCell.Terrain.Tile(liveCell.LiveCoords, liveCell.Rnd);
						terrainTile.Draw(x + ContentRct.Left, y + ContentRct.Top, fogColor.Multiply(worldLayer.GetFogColorMultiplier(liveCell)), FColor.Empty);

						foreach (var tileInfoProvider in liveCell.FoggedTileInfoProviders)
						{
							var tile = tileInfoProvider.Tile.GetTile();
							tile.Draw(x + ContentRct.Left, y + ContentRct.Top, fogColor.Multiply(worldLayer.GetFogColorMultiplier(liveCell)), FColor.Empty);
						}
						DrawHelper.FogTile(x + ContentRct.Left, y + ContentRct.Top);
					}
				}
			}
			World.TheWorld.Avatar.Tile.GetTile().Draw(width / 2 + ContentRct.Left, height / 2 + ContentRct.Top, Color.White.ToFColor(), BackgroundColor);
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