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
			
			m_losManager = new LosManager(40);

			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		private void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
			switch (_message.Type)
			{
				case WorldMessage.EType.AVATAR_MOVE:
					BackgroundColor = FColor.Black;// new FColor(1f, World.TheWorld.Avatar.Layer.Ambient.Lerp(FColor.Black, 0.2f));
					break;
			}
		}

		public override void DrawFrame()
		{
		}

		public override void DrawBackground()
		{
			TileHelper.DrawHelper.ClearTiles(new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height), FColor.Empty);// BackgroundColor.ToGrayScale());
		}

		public override void DrawContent()
		{
			var fogColor = Color.FromArgb(255, 70, 70, 70).ToFColor();

			var layer = World.TheWorld.Avatar.Layer;

			layer.SetData(m_mapCells, World.TheWorld.Avatar.Coords);

			m_losManager.GetVisibleCelss(m_mapCells, m_center, Color.White.ToFColor());

			var blocks = layer.GetBlocksNear(World.TheWorld.Avatar.Coords);
			foreach (var block in blocks)
			{
				foreach (var lightSource in block.Item2.LightSources)
				{
					lightSource.LightCells(m_mapCells, m_center);
				}
			}
			World.TheWorld.Avatar.Light.LightCells(m_mapCells, m_center);
			//var block = World.TheWorld.Avatar.MapBlock;
			//m_losManager.LightCells(m_mapCells, block.LightSources[0] + block.BlockId * MapBlock.SIZE - World.TheWorld.Avatar.Coords + m_center););

			for (var x = 0; x < m_width; ++x)
			{
				for (var y = 0; y < m_height; ++y)
				{
					var mapCell = m_mapCells[x, y];

					var backgroundColor = BackgroundColor;
					var lighted = mapCell.Visibility;//.Multiply(mapCell.Lighted);

					var lightness = lighted.Lightness();

					if (lightness > VISIBILITY_THRESHOLD)
					{
						var tile = mapCell.Tile.GetTile() ?? mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);

						var color = tile.Color.Multiply(mapCell.Lighted).Multiply(lighted).Screen(layer.Ambient);
						tile.Draw(x + ContentRectangle.Left, y + ContentRectangle.Top, color, backgroundColor);

						if (!mapCell.IsSeenBefore) mapCell.SetIsSeenBefore();
					}
					else if (mapCell.IsSeenBefore)
					{
						if(mapCell.TerrainAttribute.IsPassable==1) continue;
						var tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
						tile.Draw(x + ContentRectangle.Left, y + ContentRectangle.Top, fogColor, FColor.Black);
						DrawHelper.FogTile(x + ContentRectangle.Left, y + ContentRectangle.Top);
					}
				}
			}

			World.TheWorld.Avatar.Tile.GetTile().Draw(m_center.X + ContentRectangle.Left, m_center.Y + ContentRectangle.Top, Color.White.ToFColor(), BackgroundColor);
		}
	}
}