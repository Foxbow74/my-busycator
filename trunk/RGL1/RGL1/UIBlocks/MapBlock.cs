using System.Diagnostics;
using System.Linq;
using GameCore;
using GameCore.LOS;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = GameCore.Point;

namespace RGL1.UIBlocks
{
	class MapBlock:UIBlock
	{
		private readonly World m_world;
		private readonly MapCell[,] m_mapCells;
		private readonly LosManager m_losManager;

		public MapBlock(Rectangle _rectangle, World _world) : base(_rectangle, null, Color.Black)
		{
			m_world = _world;
			m_mapCells = new MapCell[Rectangle.Width - 2, Rectangle.Height - 2];

			m_losManager = new LosManager(m_mapCells);
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			m_world.Map.SetData(m_mapCells, m_world.Avatar.Point);

			_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			var centerX = m_mapCells.GetLength(0)/2;
			var centerY = m_mapCells.GetLength(1)/2;

			var visibleCelss = m_losManager.GetVisibleCelss(m_mapCells, centerX, centerY).ToArray();

			//for (int i = 0; i < m_mapCells.GetLength(0); i++)
			//{
			//    for (int j = 0; j < m_mapCells.GetLength(1); j++)
			//    {
			//        m_mapCells[i, j].Draw(_spriteBatch, i + 1, j + 1);
			//    }
			//}

			foreach (var tuple in visibleCelss)
			{
				var pnt = tuple.Key;
				var tile = m_mapCells[pnt.X, pnt.Y].Terrain.Tile(m_mapCells[pnt.X, pnt.Y].WorldCoords, m_mapCells[pnt.X, pnt.Y].BlockRandomSeed);
				var color = tile.Color;
				//var color = Color.White;
				tile.DrawAtCell(_spriteBatch, pnt.X + 1, pnt.Y + 1, new Color(color.R, color.G, color.B, (int)(tuple.Value * 255)));
				//_spriteBatch.DrawString(Tile.Font, tuple.Value.ToString(), new Vector2((pnt.X + 1) * Tile.Size, (pnt.Y + 1) * Tile.Size), Color.White);
				//m_mapCells[pnt.X, pnt.Y].Draw(_spriteBatch, pnt.X + 1, pnt.Y + 1);
			}
			Tiles.HeroTile.DrawAtCell(_spriteBatch, centerX + 1, centerY + 1);
			_spriteBatch.End();
		}
	}
}
