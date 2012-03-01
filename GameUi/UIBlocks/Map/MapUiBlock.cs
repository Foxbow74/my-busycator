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
			: base(_rct, null, FColor.Black)
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

			var fogColor = FColor.FromArgb(255, 70, 70, 70);
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
					var xy = new Point(x, y);
					var pnt = LiveMap.WrapCellCoords(xy + dPoint);
					var liveCell = World.TheWorld.LiveMap.Cells[pnt.X, pnt.Y];

					var lighted = liveCell.Lighted.Screen(ambient).Multiply(liveCell.Visibility);
					var lightness = lighted.Lightness();
					if(lightness>0.1f)
					{
						liveCell.SetIsSeenBefore();
					}


					var screenPoint = xy + ContentRct.LeftTop;

					if (lightness > fogLightness)
					{
						var terrainTile = liveCell.Terrain.Tile(liveCell.LiveCoords, liveCell.Rnd);
						terrainTile.Draw(screenPoint, terrainTile.Color.Multiply(lighted).Clamp());

						foreach (var tileInfoProvider in liveCell.TileInfoProviders)
						{
							var tile = tileInfoProvider.Tile.GetTile();
							var color = tile.Color.LerpColorsOnly(tileInfoProvider.LerpColor, tileInfoProvider.LerpColor.A).Multiply(lighted).Clamp();
							tile.Draw(screenPoint, color, tileInfoProvider.Direction);
						}
					}
					else if (liveCell.IsSeenBefore)
					{
						var terrainTile = liveCell.Terrain.Tile(liveCell.LiveCoords, liveCell.Rnd);
						terrainTile.Draw(screenPoint, fogColor.Multiply(worldLayer.GetFogColorMultiplier(liveCell)));

						foreach (var tileInfoProvider in liveCell.FoggedTileInfoProviders)
						{
							var tile = tileInfoProvider.Tile.GetTile();
							tile.Draw(screenPoint, fogColor.Multiply(worldLayer.GetFogColorMultiplier(liveCell)), tileInfoProvider.Direction);
						}
						DrawHelper.FogTile(screenPoint);
					}
				}
			}
			World.TheWorld.Avatar.Tile.GetTile().Draw(new Point(ContentRct.Width, ContentRct.Height) / 2 + ContentRct.LeftTop, FColor.White);
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