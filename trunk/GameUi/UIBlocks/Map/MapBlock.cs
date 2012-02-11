using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameUi.UIBlocks.Map
{
	internal partial class MapBlock : UIBlock
	{
		private readonly LosManager m_losManager;
		private readonly MapCell[,] m_mapCells;
		private long m_lastFogUpdateWorldTick;

		public MapBlock(Rectangle _rectangle) : base(_rectangle, null, Color.Black)
		{
			m_mapCells = new MapCell[ContentRectangle.Width,ContentRectangle.Height];
			m_losManager = new LosManager();

			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		private void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
			switch (_message.Type)
			{
				case WorldMessage.EType.AVATAR_TURN:
					UpdateFog();
					break;
			}
		}

		public override void DrawFrame()
		{
		}

		public override void DrawContent()
		{
			World.TheWorld.Avatar.Layer.SetData(m_mapCells, World.TheWorld.Avatar.Coords);

			var centerX = m_mapCells.GetLength(0)/2;
			var centerY = m_mapCells.GetLength(1)/2;

			
			DrawVisibleCells(centerX, centerY);
			

			using (new Profiler("DrawFoggedCells"))
			{
				DrawFoggedCells();
			}

			World.TheWorld.Avatar.Tile.DrawAtCell(centerX + ContentRectangle.Left, centerY + ContentRectangle.Top);
		}

		private void DrawVisibleCells(int _centerX, int _centerY)
		{
			IEnumerable<Tuple<Point, float>> visibleCelss;

			using (new Profiler("GetVisibleCelss"))
			{
				visibleCelss = m_losManager.GetVisibleCelss(m_mapCells, _centerX, _centerY);
			}

			using (new Profiler("DrawVisibleCells"))
			{
				foreach (var tuple in visibleCelss)
				{
					var pnt = tuple.Item1;
					var mapCell = m_mapCells[pnt.X, pnt.Y];
					var visibility = tuple.Item2;

					var tile = mapCell.Tile.GetTile();
					if(tile==null)
					{
						tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
					}
					var color = m_foggedBackColor.Lerp(tile.Color, visibility);
					tile.DrawAtCell(pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, color);

					if (!mapCell.IsSeenBefore) mapCell.SetIsSeenBefore();
					mapCell.IsVisibleNow = true;

					UpdateFogCell(mapCell, tile, color);
				}
			}
		}
	}
}