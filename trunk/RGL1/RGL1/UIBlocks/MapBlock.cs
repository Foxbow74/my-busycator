using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.Messages;

namespace RGL1.UIBlocks
{
	class MapBlock:UIBlock
	{
		private class  FoggedCell
		{
			private float m_fog;
			private Tile m_tile;
			private Color m_color;

			public FoggedCell(Tile _tile, Color _color)
			{
				m_fog = 1f;
				m_tile = _tile;
				m_color = _color;
			}

			public void Draw(SpriteBatch _spriteBatch, int _x, int _y)
			{
				var color = m_color * m_fog;
				m_tile.DrawAtCell(_spriteBatch, _x, _y, color);
				Tiles.FogTile.DrawAtCell(_spriteBatch, _x, _y);
			}

			public bool UpdateFog()
			{
				m_fog -= 0.05f;
				return m_fog<=0;
			}

			public bool IsFresh { get { return m_fog == 1; } }

			public void Update(Tile _tile, Color _color)
			{
				m_fog = 1;
				m_tile = _tile;
				m_color = _color;
			}
		}

		private readonly World m_world;
		private readonly MapCell[,] m_mapCells;
		private readonly LosManager m_losManager;

		private readonly Dictionary<int, FoggedCell> m_foggedCells = new Dictionary<int, FoggedCell>();

		public MapBlock(Rectangle _rectangle, World _world) : base(_rectangle, null, Color.Black)
		{
			m_world = _world;
			m_mapCells = new MapCell[ContentRectangle.Width, ContentRectangle.Height];
			m_losManager = new LosManager(m_mapCells);

			MessageManager.NewMessage += MessageManager_NewMessage;
		}

		

		void MessageManager_NewMessage(object _sender, Message _message)
		{
			if(_message is TurnMessage)
			{
				var pairs = m_foggedCells.ToArray();

				foreach (var pair in pairs)
				{
					if(pair.Value.UpdateFog())
					{
						m_foggedCells.Remove(pair.Key);
					}
				}
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
				tile.DrawAtCell(_spriteBatch, pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, color);

				if(mapCell.Item!=EItems.NONE)
				{
					tile = mapCell.Item.Tile();
					color = Color.Multiply(tile.Color, visibility * 1.1f);
					tile.DrawAtCell(_spriteBatch, pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, color);
				}

				var key = mapCell.WorldCoords.GetHashCode();
				FoggedCell cell;
				if(!m_foggedCells.TryGetValue(key, out cell))
				{
					cell = new FoggedCell(tile, color);
					m_foggedCells[key] = cell;
				}
				else
				{
					cell.Update(tile, color);
				}
			}

			for (var x = 0; x < m_mapCells.GetLength(0); ++x)
			{
				for (var y = 0; y < m_mapCells.GetLength(1); ++y)
				{
					var mapCell = m_mapCells[x, y];
					FoggedCell foggedCell;
					var key = mapCell.WorldCoords.GetHashCode();
					if (!m_foggedCells.TryGetValue(key, out foggedCell))
					{
						continue;
					}
					if (!foggedCell.IsFresh)
					{
						foggedCell.Draw(_spriteBatch, x + ContentRectangle.Left, y + ContentRectangle.Top);
					}
				}
			}

			Tiles.HeroTile.DrawAtCell(_spriteBatch, centerX + ContentRectangle.Left, centerY + ContentRectangle.Top);
			_spriteBatch.End();
		}
	}
}
