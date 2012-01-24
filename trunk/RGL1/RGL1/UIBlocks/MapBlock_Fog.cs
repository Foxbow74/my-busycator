using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks
{
	partial class MapBlock
	{
		private class FoggedCell
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

			public bool UpdateFog(float _d)
			{
				m_fog -= 0.05f * _d;
				return m_fog <= 0;
			}

			public bool IsFresh { get { return m_fog == 1; } }

			public void Update(Tile _tile, Color _color)
			{
				m_fog = 1;
				m_tile = _tile;
				m_color = _color;
			}
		}

		private readonly Dictionary<int, FoggedCell> m_foggedCells = new Dictionary<int, FoggedCell>();

		private void UpdateFog()
		{
			var k = (m_world.WorldTick - m_lastFogUpdateWorldTick) / 10000.0f;
			var pairs = m_foggedCells.ToArray();

			foreach (var pair in pairs)
			{
				if (pair.Value.UpdateFog(k))
				{
					m_foggedCells.Remove(pair.Key);
				}
			}
			m_lastFogUpdateWorldTick = m_world.WorldTick;
		}

		private void UpdateFogCell(MapCell _mapCell, Tile _tile, Color _color)
		{
			var key = _mapCell.WorldCoords.GetHashCode();
			FoggedCell cell;
			if (!m_foggedCells.TryGetValue(key, out cell))
			{
				cell = new FoggedCell(_tile, _color);
				m_foggedCells[key] = cell;
			}
			else
			{
				cell.Update(_tile, _color);
			}
		}

		private void DrawFoggedCells(SpriteBatch _spriteBatch)
		{
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
		}
	}
}
