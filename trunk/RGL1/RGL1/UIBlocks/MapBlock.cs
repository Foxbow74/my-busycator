using System.Linq;
using GameCore;
using GameCore.LOS;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

			foreach (var tuple in visibleCelss)
			{
				var pnt = tuple.Key;
				var tile = m_mapCells[pnt.X, pnt.Y].Terrain.Tile(m_mapCells[pnt.X, pnt.Y].WorldCoords, m_mapCells[pnt.X, pnt.Y].BlockRandomSeed);
				var visibility = (float)tuple.Value.Item1;
				var color = Color.Multiply(tile.Color, visibility);
				tile.DrawAtCell(_spriteBatch, pnt.X + 1, pnt.Y + 1, color);
			}


			//foreach (var tuple in visibleCelss)
			//{
			//    var pnt = tuple.Key;
			//    var color = new Color(tuple.Value.Item2.R, tuple.Value.Item2.G, tuple.Value.Item2.B, 20);
			//    Tiles.SolidTile.DrawAtCell(_spriteBatch, pnt.X + 1, pnt.Y + 1, color);
			//}
			Tiles.HeroTile.DrawAtCell(_spriteBatch, centerX + 1, centerY + 1);
			_spriteBatch.End();
		}
	}
}
