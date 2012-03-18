using System;
using GameCore;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;

namespace GameUi.UIBlocks
{
	internal class MapUiBlock : UIBlock
	{
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
					//m_dPoint = World.TheWorld.LiveMap.GetData();
					Redraw();
					break;
				case WorldMessage.EType.JUST_REDRAW:
					Redraw();
					break;
			}
		}

		private Point m_dPoint;

		private void Redraw()
		{
			var avatarCreenPoint = new Point(ContentRct.Width, ContentRct.Height) / 2 + ContentRct.LeftTop;

			TileHelper.DrawHelper.ClearTiles(Rct, BackgroundColor);
			m_dPoint = World.TheWorld.LiveMap.GetData();
			var fogColor = FColor.FromArgb(255, 150, 150, 150);
			var fogLightness = fogColor.Lightness();

			var worldLayer = World.TheWorld.Avatar.Layer;
			var ambient = worldLayer.Ambient;

			var width = ContentRct.Width;
			var height = ContentRct.Height;

			for (var x = 0; x < width; ++x)
			{
				for (var y = 0; y < height; ++y)
				{
					var xy = new Point(x, y);
					var pnt = LiveMap.WrapCellCoords(xy + m_dPoint);
					var liveCell = World.TheWorld.LiveMap.Cells[pnt.X, pnt.Y];

					var lighted = liveCell.Lighted.Screen(ambient).Multiply(liveCell.Visibility);

					//lighted.Add(FColor.Gray);// FColor.White;

					var screenPoint = xy + ContentRct.LeftTop;

					if (lighted.Lightness() > fogLightness || avatarCreenPoint == screenPoint)
					{
						liveCell.SetIsSeenBefore();
						var terrainTile = liveCell.Terrain.GetTile((int)Math.Abs((liveCell.LiveCoords.GetHashCode() * liveCell.Rnd)));
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
						var terrainTile = liveCell.Terrain.GetTile((int)Math.Abs((liveCell.LiveCoords.GetHashCode() * liveCell.Rnd)));
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
			World.TheWorld.Avatar.Tile.GetTile().Draw(avatarCreenPoint, FColor.White);
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