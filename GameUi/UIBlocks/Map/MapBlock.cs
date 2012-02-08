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
			World.TheWorld.Map.SetData(m_mapCells, World.TheWorld.Avatar.Coords);

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
				var tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
				var visibility = (float) tuple.Value;
				var color = tile.Color.Multiply(visibility * 1.1f);
				tile.DrawAtCell(pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, color);

				if (mapCell.Thing != null)
				{
					tile = mapCell.Thing.Tile.GetTile();
					color = tile.Color.Multiply(visibility * 1.1f);
					tile.DrawAtCell(pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, color);
				}

				if (mapCell.Creature != null)
				{
					tile = mapCell.Creature.Tile.GetTile();
					color = tile.Color.Multiply(visibility * 1.1f);
					tile.DrawAtCell(pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, color);
					//if (mapCell.Creature is Monster)
					//{
					//    _spriteBatch.DrawString(Fonts.SmallFont, ((Monster) mapCell.Creature).NN,
					//                            new Vector2(pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top)*ATile.Size,
					//                            Color.White);
					//}
				}

				if (!mapCell.IsSeenBefore) mapCell.SetIsSeenBefore();
				mapCell.IsVisibleNow = true;

				UpdateFogCell(mapCell, tile, color);
			}
		}
	}
}