using System.Drawing;
using GameCore;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameUi.UIBlocks.Map
{
	internal class MapUiBlock : UIBlock
	{
		const float VISIBILITY_THRESHOLD = 33f/255;

		private FColor m_foggedBackColor = new FColor(Color.FromArgb(255, 20, 20, 20));

		private readonly LosManager m_losManager;
		private readonly MapCell[,] m_mapCells;

		private readonly int m_width;
		private readonly int m_height;
		private readonly Point m_center;

		public MapUiBlock(Rectangle _rectangle)
			: base(_rectangle, null, Color.Black.ToFColor())
		{
			m_mapCells = new MapCell[ContentRectangle.Width,ContentRectangle.Height];

			m_width = m_mapCells.GetLength(0);
			m_height = m_mapCells.GetLength(1);

			m_center = new Point(m_width / 2,m_height / 2);
			
			m_losManager = new LosManager();

			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		private void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
			switch (_message.Type)
			{
				case WorldMessage.EType.AVATAR_MOVE:
					BackgroundColor = new FColor(1f, World.TheWorld.Avatar.Layer.Ambient.Lerp(FColor.Black, 0.2f));
					//UpdateFog();
					break;
			}
		}

		public override void DrawFrame()
		{
		}

		public override void DrawBackground()
		{
			TileHelper.DrawHelper.ClearTiles(new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height), BackgroundColor.ToGrayScale());
		}

		public override void DrawContent()
		{
			World.TheWorld.Avatar.Layer.SetData(m_mapCells, World.TheWorld.Avatar.Coords);

			var visibleCelss = m_losManager.GetVisibleCelss(m_mapCells, m_center, Color.Yellow.ToFColor());

			foreach (var tuple in visibleCelss)
			{
				var pnt = tuple.Item1;
				m_mapCells[pnt.X, pnt.Y].Lighted = tuple.Item2;
				//EFrameTiles.SOLID.GetTile().Draw(pnt.X + ContentRectangle.Left, pnt.Y + ContentRectangle.Top, tuple.Item2, BackgroundColor);
			}
			//return;
			for (var x = 0; x < m_width; ++x)
			{
				for (var y = 0; y < m_height; ++y)
				{
					var mapCell = m_mapCells[x, y];

					var backgroundColor = BackgroundColor;
					var lighted = mapCell.Lighted;//.LerpColorsOnly(World.TheWorld.Avatar.Layer.Lighted, World.TheWorld.Avatar.Layer.Lighted.A);

					lighted = lighted.ScreenColorsOnly(World.TheWorld.Avatar.Layer.Ambient);

					var lightness = lighted.Lightness();

					if (lightness > VISIBILITY_THRESHOLD)
					{
						var tile = mapCell.Tile.GetTile() ?? mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);

						var color = tile.Color.Multiply(lighted).Multiply(World.TheWorld.Avatar.Layer.Lighted);
						tile.Draw(x + ContentRectangle.Left, y + ContentRectangle.Top, color, backgroundColor);

						if (!mapCell.IsSeenBefore) mapCell.SetIsSeenBefore();
					}
					else if (mapCell.IsSeenBefore)
					{
						
						var tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
						var color = tile.Color.ToGrayScale().Multiply(World.TheWorld.Avatar.Layer.Lighted);// m_foggedBackColor.Lerp(tile.Color, VISIBILITY_THRESHOLD);
						tile.Draw(x + ContentRectangle.Left, y + ContentRectangle.Top, color, FColor.Black);
						DrawHelper.FogTile(x + ContentRectangle.Left, y + ContentRectangle.Top);
					}
					else
					{
						DrawHelper.FogTile(x + ContentRectangle.Left, y + ContentRectangle.Top);
					}
				}
			}

			World.TheWorld.Avatar.Tile.GetTile().Draw(m_center.X + ContentRectangle.Left, m_center.Y + ContentRectangle.Top, Color.White.ToFColor(), BackgroundColor);
		}
	}
}