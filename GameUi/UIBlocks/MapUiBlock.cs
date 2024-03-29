﻿using System;
using GameCore;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using Shader;

namespace GameUi.UIBlocks
{
	internal class MapUiBlock : UIBlock
	{
		readonly LosManagerEx m_lm = new LosManagerEx();

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
			World.TheWorld.UpdateDPoint();
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
					World.TheWorld.UpdateDPoint();
                    m_lm.Recalc(World.TheWorld.LiveMap);
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

			var worldLayer = World.TheWorld.Avatar.GeoInfo.Layer;

			var width = ContentRct.Width;
			var height = ContentRct.Height;

			for (var x = 0; x < width; ++x)
			{
				for (var y = 0; y < height; ++y)
				{
					var xy = new Point(x, y);

					var liveCellCoords = LiveMap.WrapCellCoords(xy + World.TheWorld.DPoint);
					var liveCell = World.TheWorld.LiveMap.Cells[liveCellCoords.X, liveCellCoords.Y];
					var screenPoint = xy + ContentRct.LeftTop;
					var visibilityA = 1f;// liveCell.Visibility.A;
					var finalLighted = liveCell.FinalLighted;

					if (liveCell.IsVisibleNow)
					{
						var eTerrains = liveCell.Terrain;
						var terrainTile = eTerrains.GetTile((int)Math.Abs((liveCell.LiveCoords.GetHashCode() * liveCell.Rnd)));
						var tcolor = terrainTile.Color.Multiply(finalLighted).Clamp().Lerp(terrainTile.Color.Multiply(0.7f), 1f -visibilityA);
						terrainTile.Draw(screenPoint, tcolor);

						foreach (var tileInfoProvider in liveCell.TileInfoProviders)
						{
							var tile = tileInfoProvider.Tileset.GetTile(tileInfoProvider.TileIndex);
							var color = tile.Color.LerpColorsOnly(tileInfoProvider.LerpColor, tileInfoProvider.LerpColor.A).Multiply(finalLighted).Clamp().UpdateAlfa(visibilityA);
							tile.Draw(screenPoint, color, tileInfoProvider.Direction, tileInfoProvider.IsCorpse);
						}
					}
					else if (liveCell.IsSeenBefore)
					{
						var fogColorMultiplier = worldLayer.GetFogColorMultiplier(liveCell);

						var eTerrains = liveCell.Terrain;
						var terrainTile = eTerrains.GetTile((int)Math.Abs((liveCell.LiveCoords.GetHashCode() * liveCell.Rnd)));

						var tcolor = terrainTile.Color.Multiply(fogColorMultiplier).ToGrayScale();
						terrainTile.Draw(screenPoint, tcolor);
						foreach (var tileInfoProvider in liveCell.FoggedTileInfoProviders)
						{
							var tile = tileInfoProvider.Tileset.GetTile(tileInfoProvider.TileIndex);
							tile.Draw(screenPoint, tile.Color.LerpColorsOnly(tileInfoProvider.LerpColor, tileInfoProvider.LerpColor.A).ToGrayScale(), tileInfoProvider.Direction, tileInfoProvider.IsCorpse);
						}
						DrawHelper.FogTile(screenPoint);
					}
				}
			}
			World.TheWorld.Avatar.Tileset.GetTile(World.TheWorld.Avatar.TileIndex).Draw(avatarScreenPoint, FColor.White);
		}

		public override void DrawFrame() { }

		public override void DrawBackground() { }

		public override void DrawContent() { }
	}
}