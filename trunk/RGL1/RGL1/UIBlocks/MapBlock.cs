using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.LOS;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.Messages;
using Point = Graphics.Point;

namespace RGL1.UIBlocks
{
	class MapBlock:UIBlock
	{
		private readonly World m_world;
		private readonly MapCell[,] m_mapCells;
		private readonly LosManager m_losManager;

		private readonly Dictionary<int, float> m_foggedTiles = new Dictionary<int, float>();
		private float m_fogD = 0f;

		public MapBlock(Rectangle _rectangle, World _world) : base(_rectangle, null, Color.Black)
		{
			m_world = _world;
			m_mapCells = new MapCell[Rectangle.Width - 2, Rectangle.Height - 2];
			m_losManager = new LosManager(m_mapCells);

			MessageManager.NewMessage += MessageManager_NewMessage;
		}

		void MessageManager_NewMessage(object _sender, Message _message)
		{
			if(_message is TurnMessage)
			{
				m_fogD = 0.05f;
			}
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
				var mapCell = m_mapCells[pnt.X, pnt.Y];
				var tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
				var visibility = (float)tuple.Value.Item1;
				var color = Color.Multiply(tile.Color, visibility * 1.1f);
				tile.DrawAtCell(_spriteBatch, pnt.X + 1, pnt.Y + 1, color);

				m_foggedTiles[mapCell.WorldCoords.GetHashCode()] = 1;
			}

			for (var x = 0; x < m_mapCells.GetLength(0); ++x)
			{
				for (var y = 0; y < m_mapCells.GetLength(1); ++y)
				{
					var mapCell = m_mapCells[x,y];
					float state;
					var key = mapCell.WorldCoords.GetHashCode();
					if(!m_foggedTiles.TryGetValue(key, out state))
					{
						continue;
					}
					
					if(state==0)
					{
						m_foggedTiles.Remove(key);
						continue;
					}
					if (state < 1)
					{
						var tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
						var color = tile.Color*state;
						//color = Color.Lerp(tile.Color, Color.Black, state);
						tile.DrawAtCell(_spriteBatch, x + 1, y + 1, color);
						Tiles.FogTile.DrawAtCell(_spriteBatch, x + 1, y + 1);
					}
					m_foggedTiles[key] = state - m_fogD;
				}
			}

			m_fogD = 0;
			Tiles.HeroTile.DrawAtCell(_spriteBatch, centerX + 1, centerY + 1);
			_spriteBatch.End();
		}
	}
}
