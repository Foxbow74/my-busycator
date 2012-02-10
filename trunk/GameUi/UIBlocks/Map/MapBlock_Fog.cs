using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Mapping;

namespace GameUi.UIBlocks.Map
{
	internal partial class MapBlock
	{
		private const float FOG_VISIBILITY_LOWEST = 0.1f;

		private readonly Dictionary<int, FoggedCell> m_foggedCells = new Dictionary<int, FoggedCell>();

		private void UpdateFog()
		{
			var k = (World.TheWorld.WorldTick - m_lastFogUpdateWorldTick)/10000.0f;
			var pairs = m_foggedCells.ToArray();

			foreach (var pair in pairs)
			{
				if (pair.Value.UpdateFog(k))
				{
					m_foggedCells.Remove(pair.Key);
				}
			}
			m_lastFogUpdateWorldTick = World.TheWorld.WorldTick;
		}

		private void UpdateFogCell(MapCell _mapCell, ATile _tile, Color _color)
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

		private Color m_foggedBackColor = Color.FromArgb(255, 40, 40, 40);

		private void DrawFoggedCells()
		{
			for (var x = 0; x < m_mapCells.GetLength(0); ++x)
			{
				for (var y = 0; y < m_mapCells.GetLength(1); ++y)
				{
					var mapCell = m_mapCells[x, y];
					if (mapCell.IsSeenBefore && !mapCell.IsVisibleNow)
					{
						var tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
						var color = m_foggedBackColor.Lerp(tile.Color, FOG_VISIBILITY_LOWEST);
						tile.DrawAtCell(x + ContentRectangle.Left, y + ContentRectangle.Top, color);

						FoggedCell foggedCell;
						var key = mapCell.WorldCoords.GetHashCode();

						if (m_foggedCells.TryGetValue(key, out foggedCell) && !foggedCell.IsFresh && foggedCell.Tile != tile)
						{
							foggedCell.Draw(x + ContentRectangle.Left, y + ContentRectangle.Top, m_foggedBackColor);
						}

						ETiles.FOG.DrawAtCell(x + ContentRectangle.Left, y + ContentRectangle.Top, Color.Black);
					}
				}
			}
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			throw new NotImplementedException();
		}

		#region Nested type: FoggedCell

		private class FoggedCell
		{
			private Color m_color;
			private float m_fog;

			public FoggedCell(ATile _tile, Color _color)
			{
				m_fog = 1f;
				Tile = _tile;
				m_color = _color;
			}

			public bool IsFresh
			{
				get { return m_fog == 1; }
			}

			public ATile Tile { get; private set; }

			public void Draw(int _x, int _y, Color _backgroundColor)
			{
				var color = _backgroundColor.Lerp(m_color, m_fog);
				Tile.DrawAtCell(_x, _y, color);
				//GraphicsResourceHelper.FogTile.DrawAtCell(_x, _y, Color.Black);
			}

			public bool UpdateFog(float _d)
			{
				m_fog -= 0.05f*_d;
				return m_fog <= FOG_VISIBILITY_LOWEST;
			}

			public void Update(ATile _tile, Color _color)
			{
				m_fog = 1;
				Tile = _tile;
				m_color = _color;
			}
		}

		#endregion
	}
}