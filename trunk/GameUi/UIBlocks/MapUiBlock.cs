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
		private Point m_mouse;

		public MapUiBlock(Rct _rct)
			: base(_rct, null, FColor.Black)
		{
			World.TheWorld.LiveMap.SetViewPortSize(new Point(ContentRct.Width, ContentRct.Height));
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		public override void MouseMove(Point _pnt)
		{
			base.MouseMove(_pnt);
			m_mouse = _pnt;
			Redraw();
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
			var halfScreen = new Point(ContentRct.Width, ContentRct.Height) / 2;
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
					var lightness = lighted.Lightness();
					if (lightness > worldLayer.FogLightness || avatarScreenPoint == screenPoint)
					{
						liveCell.SetIsSeenBefore();
						var terrainTile = liveCell.Terrain.GetTile((int)Math.Abs((liveCell.LiveCoords.GetHashCode() * liveCell.Rnd)));
						terrainTile.Draw(screenPoint, terrainTile.Color.Multiply(lighted).Clamp().UpdateAlfa(visibility.A));

						foreach (var tileInfoProvider in liveCell.TileInfoProviders)
						{
							var tile = tileInfoProvider.Tile.GetTile();
							var color = tile.Color.LerpColorsOnly(tileInfoProvider.LerpColor, tileInfoProvider.LerpColor.A).Multiply(lighted).Clamp().UpdateAlfa(visibility.A);
							tile.Draw(screenPoint, color, tileInfoProvider.Direction);
						}
					}
					else if (liveCell.IsSeenBefore)
					{
						var fogColorMultiplier = worldLayer.GetFogColorMultiplier(liveCell); 
						
						var terrainTile = liveCell.Terrain.GetTile((int)Math.Abs((liveCell.LiveCoords.GetHashCode() * liveCell.Rnd)));
						var fColor = worldLayer.FogColor.Multiply(fogColorMultiplier);
						//if(fColor.Lightness()<0.1f) continue;

						terrainTile.Draw(screenPoint, fColor);

						foreach (var tileInfoProvider in liveCell.FoggedTileInfoProviders)
						{
							var tile = tileInfoProvider.Tile.GetTile();
							tile.Draw(screenPoint, fColor, tileInfoProvider.Direction);
						}
						DrawHelper.FogTile(screenPoint);
					}
				}
			}
			World.TheWorld.Avatar.Tile.GetTile().Draw(avatarScreenPoint, FColor.White);
		}

		internal static FColor GetLighted(LiveMapCell liveCell, FColor visibility, FColor ambient)
		{
			if(visibility.A>0)
			{
				visibility.UpdateAlfa(visibility.A + 0.2f);
			}

			return liveCell.Lighted.Screen(ambient).Multiply(visibility);
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