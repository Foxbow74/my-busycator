using System;
using System.Collections.Generic;
using GameCore;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.PathFinding;

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
			List<Point> path = null;
			Point pathDelta = Point.Zero;

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

					var lighted = liveCell.Lighted.Screen(ambient).Multiply(liveCell.Visibility);
					var screenPoint = xy + ContentRct.LeftTop;

					var lightness = lighted.Lightness();
					if (lightness > World.FogLightness || avatarScreenPoint == screenPoint)
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

						var terrainTile = liveCell.Terrain.GetTile((int) Math.Abs((liveCell.LiveCoords.GetHashCode()*liveCell.Rnd)));
						terrainTile.Draw(screenPoint, World.FogColor.Multiply(worldLayer.GetFogColorMultiplier(liveCell)));

						foreach (var tileInfoProvider in liveCell.FoggedTileInfoProviders)
						{
							var tile = tileInfoProvider.Tile.GetTile();
							tile.Draw(screenPoint, World.FogColor.Multiply(worldLayer.GetFogColorMultiplier(liveCell)), tileInfoProvider.Direction);
						}
						DrawHelper.FogTile(screenPoint);
					}

					if(xy==m_mouse)
					{
						var opc = liveCell.OnPathMapCoords;
						path = World.TheWorld.LiveMap.PathFinder.FindPath(World.TheWorld.Avatar, opc, PathFinder.HeuristicFormula.EUCLIDEAN_NO_SQR);
						pathDelta = screenPoint - opc;
					}
				}
			}
			if(path!=null)
			{
				for (var index = 0; index < path.Count; index++)
				{
					var pathFinderNode = path[path.Count - index - 1];
					var pnt = pathFinderNode + pathDelta;
					if(index==path.Count-1)
					{
						ETiles.TARGET_CROSS.GetTile().Draw(pnt);
					}
					else
					{
						ETiles.TARGET_DOT.GetTile().Draw(pnt);
					}
				}
			}
			World.TheWorld.Avatar.Tile.GetTile().Draw(avatarScreenPoint, FColor.White);
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