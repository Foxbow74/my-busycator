using System;
using System.Drawing;
using GameCore;

namespace GameUi.UIBlocks.Map
{
	internal partial class MapBlock
	{
		private const float FOG_VISIBILITY_LOWEST = 0.1f;

		private void UpdateFog()
		{
			var k = (World.TheWorld.WorldTick - m_lastFogUpdateWorldTick)/10000.0f;
			m_lastFogUpdateWorldTick = World.TheWorld.WorldTick;
		}

		private Color m_foggedBackColor = Color.FromArgb(255, 20, 20, 20);

		private void DrawFoggedCells()
		{
			//var m_foggedBackColor = BackgroundColor;
			var width = m_mapCells.GetLength(0);
			var height = m_mapCells.GetLength(1);
			for (var x = 0; x < width; ++x)
			{
				for (var y = 0; y < height; ++y)
				{
					var mapCell = m_mapCells[x, y];
					if (mapCell.IsVisibleNow || !mapCell.IsSeenBefore) continue;

					var tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
					var color = m_foggedBackColor.Lerp(tile.Color, FOG_VISIBILITY_LOWEST);
					tile.DrawAtCell(x + ContentRectangle.Left, y + ContentRectangle.Top, color);

					ETiles.FOG.GetTile().FogIt(x + ContentRectangle.Left, y + ContentRectangle.Top, BackgroundColor);
				}
			}
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			throw new NotImplementedException();
		}
	}
}