using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;

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

			DrawFoggedCells();

			World.TheWorld.Avatar.Tile.DrawAtCell(centerX + ContentRectangle.Left, centerY + ContentRectangle.Top);
		}

		private void DrawVisibleCells(int _centerX, int _centerY)
		{
			var visibleCelss = m_losManager.GetVisibleCelss(m_mapCells, _centerX, _centerY).ToArray();

			foreach (var tuple in visibleCelss)
			{
				var pnt = tuple.Key;
				var mapCell = m_mapCells[pnt.X, pnt.Y];
				var visibility = (float) tuple.Value;

				ATile tile;
				var creature = mapCell.Creature;
				if (creature != null)
				{
					tile = creature.Tile.GetTile();
				}
				else
				{
					var items = mapCell.Items.ToArray();
					if (items.Length > 0)
					{
						tile = items.Length == 1 ? items[0].Tile.GetTile() : ETiles.HEAP_OF_ITEMS.GetTile();
					}
					else
					{
						var furniture = mapCell.Furniture;

						if (furniture != null) tile = furniture.Tile.GetTile();
						else tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
					}
				}
				var color = tile.Color.Multiply(visibility*1.1f);
				tile.DrawAtCell(pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, color);

				if (!mapCell.IsSeenBefore) mapCell.SetIsSeenBefore();
				mapCell.IsVisibleNow = true;

				UpdateFogCell(mapCell, tile, color);
			}
		}
	}
}