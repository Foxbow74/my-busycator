using System;
using GameCore;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class MapUiBlock : UIBlock
	{
		private Point m_dPoint;

		public MapUiBlock(Rct _rct)
			: base(_rct, null, FColor.Black)
		{
			World.TheWorld.LiveMap.SetViewPortSize(new Point(ContentRct.Width, ContentRct.Height));
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		public override void Resize(Rct _newRct)
		{
			base.Resize(_newRct);
			World.TheWorld.LiveMap.SetViewPortSize(new Point(ContentRct.Width, ContentRct.Height));
			World.TheWorld.LiveMap.Reset();
			m_dPoint = World.TheWorld.LiveMap.GetData();
		}

		public override void Dispose()
		{
			MessageManager.NewWorldMessage -= MessageManagerNewWorldMessage;
			base.Dispose();
		}

		private void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
			switch (_message.Type)
			{
				case WorldMessage.EType.TURN:
					m_dPoint = World.TheWorld.LiveMap.GetData();
					Redraw();
					break;
				case WorldMessage.EType.JUST_REDRAW:
					Redraw();
					break;
			}
		}

		private void Redraw()
		{
			var halfScreen = new Point(ContentRct.Width, ContentRct.Height)/2;
			var avatarScreenPoint = halfScreen + ContentRct.LeftTop;

			TileHelper.DrawHelper.ClearTiles(Rct, BackgroundColor);

			var worldLayer = World.TheWorld.Avatar.Layer;
			var ambient = worldLayer.Ambient;

			var width = ContentRct.Width;
			var height = ContentRct.Height;

			for (var x = 0; x < width; ++x)
			{
				for (var y = 0; y < height; ++y)
				{
					var xy = new Point(x, y);

					var liveCellCoords = LiveMap.WrapCellCoords(xy + m_dPoint);
					var liveCell = World.TheWorld.LiveMap.Cells[liveCellCoords.X, liveCellCoords.Y];
					var screenPoint = xy + ContentRct.LeftTop;


					var visibility = liveCell.Visibility;

					var lighted = GetLighted(liveCell, visibility, ambient);


					//lighted = FColor.White;
					var lightness = lighted.Lightness();

					if (lightness > worldLayer.FogLightness || avatarScreenPoint == screenPoint)
					{
						liveCell.SetIsSeenBefore();
						var eTerrains = liveCell.Terrain;
						var terrainTile = eTerrains.GetTile((int)Math.Abs((liveCell.LiveCoords.GetHashCode() * liveCell.Rnd)));
						var tcolor = terrainTile.Color.Multiply(lighted).Clamp().Lerp(terrainTile.Color.Multiply(0.7f), 1f - visibility.A);
						terrainTile.Draw(screenPoint, tcolor);
						//terrainTile.Draw(screenPoint, candidate);

						foreach (var tileInfoProvider in liveCell.TileInfoProviders)
						{
							var tile = tileInfoProvider.Tile.GetTile();
							var color = tile.Color.LerpColorsOnly(tileInfoProvider.LerpColor, tileInfoProvider.LerpColor.A).Multiply(lighted).Clamp().UpdateAlfa(visibility.A);
							if (World.TheWorld.Avatar[1, 0].WorldCoords == liveCell.WorldCoords)
							{
							}
							tile.Draw(screenPoint, color, tileInfoProvider.Direction);
						}
					}
					else if (liveCell.IsSeenBefore)
					{
						var fogColorMultiplier = worldLayer.GetFogColorMultiplier(liveCell);

						var eTerrains = liveCell.Terrain;
						var terrainTile = eTerrains.GetTile((int)Math.Abs((liveCell.LiveCoords.GetHashCode() * liveCell.Rnd)));
						var tcolor = terrainTile.Color.Multiply(0.7f);
						terrainTile.Draw(screenPoint, tcolor);
						foreach (var tileInfoProvider in liveCell.FoggedTileInfoProviders)
						{
							var tile = tileInfoProvider.Tile.GetTile();
							tile.Draw(screenPoint, tile.Color.LerpColorsOnly(tileInfoProvider.LerpColor, tileInfoProvider.LerpColor.A), tileInfoProvider.Direction);
						}
						DrawHelper.FogTile(screenPoint);
					}


					//var terrainColor = terrainTile.Color;
					//if (terrainColor.Lightness()> worldLayer.FogLightness)
					//{
					//    var candidate = terrainColor.Multiply(lighted).Clamp();
					//    if(candidate.Lightness()>terrainColor.Lightness())
					//    {
					//        terrainColor = candidate;
					//    }
					//    //terrainColor = terrainColor.Multiply(lighted).Clamp();
					//}


					//if (visibility.A>0 || avatarScreenPoint == screenPoint)
					//{
					//    terrainTile.Draw(screenPoint, terrainColor);

					//    liveCell.SetIsSeenBefore();

					//    foreach (var tileInfoProvider in liveCell.TileInfoProviders)
					//    {
					//        var tile = tileInfoProvider.Tile.GetTile();
					//        var color = tile.Color.LerpColorsOnly(tileInfoProvider.LerpColor, tileInfoProvider.LerpColor.A).Multiply(lighted).Clamp().UpdateAlfa(visibility.A);
					//        tile.Draw(screenPoint, color, tileInfoProvider.Direction);
					//    }
					//}
					//else if (liveCell.IsSeenBefore)
					//{
					//    terrainTile.Draw(screenPoint, terrainColor);

					//    var fogColorMultiplier = worldLayer.GetFogColorMultiplier(liveCell); 

					//    //var terrainTile = liveCell.Terrain.GetTile((int)Math.Abs((liveCell.LiveCoords.GetHashCode() * liveCell.Rnd)));
					//    var fColor = worldLayer.FogColor.Multiply(fogColorMultiplier);
					//    //if(fColor.Lightness()<0.1f) continue;

					//    terrainTile.Draw(screenPoint, fColor);

					//    foreach (var tileInfoProvider in liveCell.FoggedTileInfoProviders)
					//    {
					//        var tile = tileInfoProvider.Tile.GetTile();
					//        tile.Draw(screenPoint, fColor, tileInfoProvider.Direction);
					//    }
					//    DrawHelper.FogTile(screenPoint);
					//}
				}
			}
			World.TheWorld.Avatar.Tile.GetTile().Draw(avatarScreenPoint, FColor.White);
		}

		internal static FColor GetLighted(LiveMapCell _liveCell, FColor _visibility, FColor _ambient)
		{
			//if(_visibility.A>0)
			//{
			//    _visibility.UpdateAlfa(_visibility.A + 0.1f);
			//}

			return _liveCell.Lighted.Screen(_ambient).Multiply(_visibility);
		}

		public override void DrawFrame() { }

		public override void DrawBackground() { }

		public override void DrawContent() { }
	}
}