using System.Linq;
using Common.Messages;
using GameCore;
using GameCore.Creatures;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks
{
	partial class MapBlock:UIBlock
	{
		private readonly World m_world;
		private readonly MapCell[,] m_mapCells;
		private readonly LosManager m_losManager;
		private long m_lastFogUpdateWorldTick;

		public MapBlock(Rectangle _rectangle, World _world) : base(_rectangle, Frame.SimpleFrame, Color.Black)
		{
			m_world = _world;
			m_mapCells = new MapCell[ContentRectangle.Width, ContentRectangle.Height];
			m_losManager = new LosManager(m_mapCells);

			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
			switch (_message.Type)
			{
				case WorldMessage.EType.AVATAR_TURN:
					UpdateFog();
					break;
			}
		}

		public override void DrawFrame(SpriteBatch _spriteBatch)
		{
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			m_world.Map.SetData(m_mapCells, m_world.Avatar.Coords);

			_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			var centerX = m_mapCells.GetLength(0)/2;
			var centerY = m_mapCells.GetLength(1)/2;

			DrawVisibleCells(_spriteBatch, centerX, centerY);

			DrawFoggedCells(_spriteBatch);

			m_world.Avatar.Tile.DrawAtCell(_spriteBatch, centerX + ContentRectangle.Left, centerY + ContentRectangle.Top);
			_spriteBatch.End();
		}

		private void DrawVisibleCells(SpriteBatch _spriteBatch, int _centerX, int _centerY)
		{
			var visibleCelss = m_losManager.GetVisibleCelss(m_mapCells, _centerX, _centerY).ToArray();

			foreach (var tuple in visibleCelss)
			{
				var pnt = tuple.Key;
				var mapCell = m_mapCells[pnt.X, pnt.Y];
				var tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
				var visibility = (float)tuple.Value.Item1;
				var color = Color.Multiply(tile.Color, visibility * 1.1f);
				tile.DrawAtCell(_spriteBatch, pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, color);

				if (mapCell.Object != null)
				{
					tile = mapCell.Object.Tile;
					color = Color.Multiply(tile.Color, visibility * 1.1f);
					tile.DrawAtCell(_spriteBatch, pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, color);
				}

				if (mapCell.Creature != null)
				{
					tile = mapCell.Creature.Tile;
					color = Color.Multiply(tile.Color, visibility * 1.1f);
					tile.DrawAtCell(_spriteBatch, pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, color);
					if (mapCell.Creature is Monster)
					{
						_spriteBatch.DrawString(Tile.SmallFont, ((Monster)mapCell.Creature).NN, new Vector2(pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top)*Tile.Size, Microsoft.Xna.Framework.Color.White);
					}
				}

				UpdateFogCell(mapCell, tile, color);
			}
		}
	}
}
